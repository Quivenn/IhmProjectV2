using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject panneauMenu;
    public TMP_Text txtTotal, txtVerre, txtEmballage, txtAliment;

    public GameObject prefabEmballage, prefabVerre, prefabAliment;
    public Vector2 zoneSpawnX = new Vector2(-4f, 4f);
    public Vector2 zoneSpawnZ = new Vector2(-4f, 1f);

    private void Start()
    {
        FermerMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (panneauMenu.activeSelf)
                FermerMenu();
            else
                OuvrirMenu();
        }

        if (panneauMenu.activeSelf)
            MettreAJourCompteurs();
    }

    private void OuvrirMenu()
    {
        panneauMenu.SetActive(true);
        PlayerController.VerrouillerSouris(false);
    }

    private void FermerMenu()
    {
        panneauMenu.SetActive(false);
        PlayerController.VerrouillerSouris(true);
    }

    void MettreAJourCompteurs()
    {
        int nbVerre = GameObject.FindGameObjectsWithTag("Verre").Length;
        int nbEmb = GameObject.FindGameObjectsWithTag("Emballage").Length;
        int nbAli = GameObject.FindGameObjectsWithTag("Aliment").Length;

        txtTotal.text = "Total déchets : " + (nbVerre + nbEmb + nbAli);
        txtVerre.text = "Verre : " + nbVerre;
        txtEmballage.text = "Emballages : " + nbEmb;
        txtAliment.text = "Aliments : " + nbAli;
    }

    public Transform joueur; // glisser la Main Camera dans l'Inspector
    public float rayonSpawnMin = 2f;
    public float rayonSpawnMax = 6f;

    public void GenererDechets()
    {
        GameObject[] prefabs = { prefabEmballage, prefabVerre, prefabAliment };
        for (int i = 0; i < 3; i++)
        {
            GameObject p = prefabs[Random.Range(0, prefabs.Length)];

            // Point aléatoire dans un anneau autour du joueur
            Vector2 dir = Random.insideUnitCircle.normalized;
            float dist = Random.Range(rayonSpawnMin, rayonSpawnMax);
            Vector3 pos = joueur.position + new Vector3(dir.x * dist, 0f, dir.y * dist);

            // Pose sur le sol réel
            if (Physics.Raycast(pos + Vector3.up * 50f, Vector3.down, out RaycastHit hit, 100f))
                pos.y = hit.point.y + 0.5f;
            else
                pos.y = joueur.position.y; // secours : hauteur du joueur

            Instantiate(p, pos, Random.rotation);
        }
    }


    public GameObject flecheGuide;

    public void ToggleFleche(bool actif)
    {
        flecheGuide.SetActive(actif);
    }

    public void ToggleIcones(bool actif)
    {
        foreach (IconeFlottante ic in FindObjectsByType<IconeFlottante>(FindObjectsSortMode.None))
        {
            Transform icone = ic.transform.Find("Icone");
            if (icone != null) icone.gameObject.SetActive(actif);
        }
    }
}