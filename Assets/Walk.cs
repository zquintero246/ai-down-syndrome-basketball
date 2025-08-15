using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Transform cameraTransform; // arrastra la cámara aquí

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Entrada
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Dirección de la cámara proyectada en el plano horizontal
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0f;
        camRight.Normalize();

        // Dirección final de movimiento
        Vector3 moveDir = camForward * vertical + camRight * horizontal;

        // Mover al jugador
        rb.MovePosition(rb.position + moveDir * speed * Time.fixedDeltaTime);
    }
}

