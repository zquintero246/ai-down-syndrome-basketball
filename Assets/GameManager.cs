using UnityEngine;
using System.Collections;

public enum GameMode { Exploration, Battle }

[System.Serializable]
public struct WildInstance
{
    public CreatureSO creature;
    public int level;
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Referencias")]
    public PlayerMovement player;
    public IsometricCameraController isoCam; // tu script de cámara actual
    public Transform battleCamAnchor;        // punto fijo para la cámara en batalla
    public BattleUIController battleUI;
    public ScreenFader fader;

    [Header("Estado")]
    public GameMode mode = GameMode.Exploration;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        // Opcional: DontDestroyOnLoad(gameObject);
    }

    public void StartBattle(WildInstance wild)
    {
        if (mode == GameMode.Battle) return;
        StartCoroutine(StartBattleRoutine(wild));
    }

    IEnumerator StartBattleRoutine(WildInstance wild)
    {
        mode = GameMode.Battle;

        // 1) Fundido a negro
        if (fader) yield return fader.FadeOut();

        // 2) Congelar al jugador y "apagar" cámara isométrica
        if (player) player.enabled = false;
        if (isoCam)
        {
            isoCam.cameraEnabled = false;
            if (battleCamAnchor)
            {
                // Mover cámara a una toma fija de batalla
                isoCam.transform.position = battleCamAnchor.position;
                isoCam.transform.rotation = battleCamAnchor.rotation;
            }
        }

        // 3) Mostrar UI de batalla con el mensaje de introducción
        if (battleUI) battleUI.ShowIntro(wild);

        // 4) Fundido desde negro para revelar la escena de batalla
        if (fader) yield return fader.FadeIn();

        // Aquí ya estás “en batalla”. Podrías habilitar un sistema de menús (Luchar/Huir).
    }

    public void EndBattle()
    {
        if (mode != GameMode.Battle) return;
        StartCoroutine(EndBattleRoutine());
    }

    IEnumerator EndBattleRoutine()
    {
        // 1) Fundido a negro
        if (fader) yield return fader.FadeOut();

        // 2) Ocultar UI de batalla y volver a exploración
        if (battleUI) battleUI.Hide();
        if (isoCam)
        {
            // Reactivar la cámara isométrica
            isoCam.cameraEnabled = true;
        }
        if (player) player.enabled = true;

        mode = GameMode.Exploration;

        // 3) Fundido desde negro para volver al mundo
        if (fader) yield return fader.FadeIn();
    }
}
