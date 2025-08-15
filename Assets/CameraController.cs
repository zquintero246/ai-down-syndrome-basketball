using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    public Transform target;         // Personaje a seguir
    public Vector3 offset = new Vector3(-5f, 7f, -5f); // Posición relativa
    public bool cameraEnabled = true; // Para activar/desactivar cámara

    void LateUpdate()
    {
        if (!cameraEnabled || target == null)
            return;

        // Coloca la cámara en posición exacta respecto al jugador
        transform.position = target.position + offset;

        // Mira al personaje
        transform.LookAt(target.position);
    }

    // Método para activar/desactivar en runtime
    public void ToggleCamera(bool state)
    {
        cameraEnabled = state;
    }
}
