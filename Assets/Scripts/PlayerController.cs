using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float vitesse = 4f;
    public float sensibilite = 2f;

    private float rotX = 0f;
    private float rotY = 0f;

    void Update()
    {
        // Déplacement ZQSD / flèches
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;
        move.y = 0;
        transform.position += move * vitesse * Time.deltaTime;

        // Rotation caméra avec clic droit maintenu
        if (Input.GetMouseButton(1))
        {
            rotY += Input.GetAxis("Mouse X") * sensibilite;
            rotX -= Input.GetAxis("Mouse Y") * sensibilite;
            rotX = Mathf.Clamp(rotX, -80f, 80f);
            transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
    }
}