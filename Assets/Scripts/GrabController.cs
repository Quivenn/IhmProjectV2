using UnityEngine;

public class GrabController : MonoBehaviour
{
    public float distancePrise = 5f;
    public float distanceTenue = 2f;

    private Rigidbody objetTenu;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // clic gauche
        {
            if (objetTenu == null) Ramasser();
            else Lacher();
        }

        if (objetTenu != null)
        {
            // L'objet suit un point devant la caméra
            Vector3 cible = cam.transform.position + cam.transform.forward * distanceTenue;
            objetTenu.MovePosition(Vector3.Lerp(objetTenu.position, cible, 15f * Time.deltaTime));
        }
    }

    void Ramasser()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, distancePrise))
        {
            if (hit.collider.CompareTag("Emballage") ||
                hit.collider.CompareTag("Verre") ||
                hit.collider.CompareTag("Aliment"))
            {
                objetTenu = hit.collider.GetComponent<Rigidbody>();
                objetTenu.useGravity = false;
                objetTenu.linearDamping = 10f; // "drag" dans les versions < Unity 6
                UIManager.Instance.AfficherNomDechet(hit.collider.tag);
                Transform icone = objetTenu.transform.Find("Icone");
                if (icone != null) icone.gameObject.SetActive(false);
            }
        }
    }

    void Lacher()
    {
        objetTenu.useGravity = true;
        objetTenu.linearDamping = 0f;
        Transform icone = objetTenu.transform.Find("Icone");
        if (icone != null) icone.gameObject.SetActive(true);
        objetTenu = null;
        UIManager.Instance.CacherNomDechet();
    }
}