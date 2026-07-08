using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Déplacement")]
    [SerializeField] private float vitesse = 4f;

    [Header("Caméra")]
    [SerializeField] private float sensibilite = 2f;
    [SerializeField] private float angleVerticalMax = 80f;

    private float rotationVerticale;
    private float rotationHorizontale;

    private void Start()
    {
        Vector3 rotationActuelle = transform.eulerAngles;

        rotationHorizontale = rotationActuelle.y;
        rotationVerticale = rotationActuelle.x;

        VerrouillerSouris(true);
    }

    private void Update()
    {
        // Aucun mouvement lorsque le menu utilise la souris.
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        Deplacer();
        TournerCamera();
    }

    private void Deplacer()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 directionAvant = transform.forward;
        directionAvant.y = 0f;
        directionAvant.Normalize();

        Vector3 directionDroite = transform.right;
        directionDroite.y = 0f;
        directionDroite.Normalize();

        Vector3 mouvement =
            directionDroite * horizontal +
            directionAvant * vertical;

        // Évite d'aller plus vite en diagonale.
        mouvement = Vector3.ClampMagnitude(mouvement, 1f);

        transform.position += mouvement * vitesse * Time.deltaTime;
    }

    private void TournerCamera()
    {
        float sourisX = Input.GetAxis("Mouse X") * sensibilite;
        float sourisY = Input.GetAxis("Mouse Y") * sensibilite;

        rotationHorizontale += sourisX;
        rotationVerticale -= sourisY;

        rotationVerticale = Mathf.Clamp(
            rotationVerticale,
            -angleVerticalMax,
            angleVerticalMax
        );

        transform.rotation = Quaternion.Euler(
            rotationVerticale,
            rotationHorizontale,
            0f
        );
    }

    public static void VerrouillerSouris(bool verrouiller)
    {
        Cursor.lockState = verrouiller
            ? CursorLockMode.Locked
            : CursorLockMode.None;

        Cursor.visible = !verrouiller;
    }
}