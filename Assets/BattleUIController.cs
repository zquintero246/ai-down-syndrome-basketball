using UnityEngine;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour
{
    public GameObject root;          // Panel raíz del UI de batalla
    public Image wildImage;          // Imagen del enemigo
    public Text messageText;         // Texto de mensaje

    public void ShowIntro(WildInstance wi)
    {
        root.SetActive(true);

        if (wi.creature != null && wi.creature.frontSprite != null)
            wildImage.sprite = wi.creature.frontSprite;
        else
            wildImage.sprite = null;

        string nameToShow = wi.creature != null ? wi.creature.creatureName : "???";
        messageText.text = $"¡Un {nameToShow} salvaje apareció!  (Nv. {wi.level})";
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}

