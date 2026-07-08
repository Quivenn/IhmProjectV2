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

    // À brancher sur le bouton
    public void GenererDechets()
    {
        GameObject[] prefabs = { prefabEmballage, prefabVerre, prefabAliment };
        for (int i = 0; i < 3; i++)
        {
            GameObject p = prefabs[Random.Range(0, prefabs.Length)];
            Vector3 pos = new Vector3(
                Random.Range(zoneSpawnX.x, zoneSpawnX.y),
                0.5f,
                Random.Range(zoneSpawnZ.x, zoneSpawnZ.y));
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