using UnityEngine;

public class GrabController : MonoBehaviour
{
    public static GrabController Instance { get; private set; }

    [Header("Préhension")]
    [SerializeField] private float distancePrise = 5f;
    [SerializeField] private float distanceTenue = 2f;
    [SerializeField] private float vitesseSuivi = 15f;

    private Rigidbody objetTenu;
    private Camera cam;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Plusieurs GrabController sont présents.");
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        cam = Camera.main;

        if (cam == null)
            Debug.LogError("Aucune caméra portant le tag MainCamera n'a été trouvée.");
    }

    private void Update()
    {
        if (cam == null)
            return;

        // Empêche les interactions pendant l'utilisation du menu.
        if (Cursor.lockState != CursorLockMode.Locked)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (objetTenu == null)
                Ramasser();
            else
                Lacher();
        }

        if (objetTenu != null)
            SuivreCamera();
    }

    private void Ramasser()
    {
        Vector3 centreEcran = new Vector3(
            Screen.width * 0.5f,
            Screen.height * 0.5f,
            0f
        );

        Ray rayon = cam.ScreenPointToRay(centreEcran);

        if (!Physics.Raycast(rayon, out RaycastHit hit, distancePrise))
            return;

        if (!EstUnDechet(hit.collider.gameObject))
            return;

        Rigidbody rigidbodyTrouve =
            hit.collider.GetComponentInParent<Rigidbody>();

        if (rigidbodyTrouve == null)
        {
            Debug.LogWarning("Le déchet sélectionné ne possède pas de Rigidbody.");
            return;
        }

        objetTenu = rigidbodyTrouve;

        objetTenu.useGravity = false;
        objetTenu.linearVelocity = Vector3.zero;
        objetTenu.angularVelocity = Vector3.zero;
        objetTenu.linearDamping = 10f;

        if (UIManager.Instance != null)
            UIManager.Instance.AfficherNomDechet(
                ObtenirTagDechet(hit.collider.gameObject)
            );

        Transform icone = objetTenu.transform.Find("Icone");

        if (icone != null)
            icone.gameObject.SetActive(false);
    }

    private void SuivreCamera()
    {
        Vector3 positionCible =
            cam.transform.position +
            cam.transform.forward * distanceTenue;

        Vector3 nouvellePosition = Vector3.Lerp(
            objetTenu.position,
            positionCible,
            vitesseSuivi * Time.deltaTime
        );

        objetTenu.MovePosition(nouvellePosition);
    }

    private void Lacher()
    {
        if (objetTenu == null)
        {
            NettoyerSaisie();
            return;
        }

        objetTenu.useGravity = true;
        objetTenu.linearDamping = 0f;

        Transform icone = objetTenu.transform.Find("Icone");

        if (icone != null)
            icone.gameObject.SetActive(true);

        NettoyerSaisie();
    }

    /*
     * Appelée par ZoneDepot juste avant la destruction
     * du déchet placé dans un conteneur.
     */
    public void NotifierObjetDepose(GameObject objetDepose)
    {
        if (objetTenu == null)
            return;

        if (objetTenu.gameObject != objetDepose)
            return;

        NettoyerSaisie();
    }

    private void NettoyerSaisie()
    {
        objetTenu = null;

        if (UIManager.Instance != null)
            UIManager.Instance.CacherNomDechet();
    }

    private bool EstUnDechet(GameObject objet)
    {
        return objet.CompareTag("Emballage") ||
               objet.CompareTag("Verre") ||
               objet.CompareTag("Aliment");
    }

    private string ObtenirTagDechet(GameObject objet)
    {
        if (EstUnDechet(objet))
            return objet.tag;

        if (objet.transform.parent != null &&
            EstUnDechet(objet.transform.parent.gameObject))
        {
            return objet.transform.parent.tag;
        }

        return "";
    }
}