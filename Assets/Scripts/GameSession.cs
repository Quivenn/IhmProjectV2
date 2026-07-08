using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    [Header("HUD")]
    public TMP_Text txtErreurs;
    public TMP_Text txtTemps;

    [Header("Écran de fin")]
    public GameObject ecranFin;
    public TMP_Text txtResultat;
    public TMP_Text txtStats;

    private float tempsEcoule = 0f;
    private bool partieFinie = false;

    private void Start()
    {
        // L'écran de fin doit être caché au lancement.
        if (ecranFin != null)
            ecranFin.SetActive(false);

        MettreAJourHUD();
    }

    private void Update()
    {
        if (partieFinie)
            return;

        tempsEcoule += Time.deltaTime;
        MettreAJourHUD();

        // La partie se termine lorsqu'il ne reste plus aucun déchet.
        if (tempsEcoule > 1f && CompterDechetsRestants() == 0)
            FinDePartie();
    }

    private void MettreAJourHUD()
    {
        int minutes = (int)(tempsEcoule / 60f);
        int secondes = (int)(tempsEcoule % 60f);

        if (txtTemps != null)
            txtTemps.text = $"{minutes:00}:{secondes:00}";

        if (txtErreurs != null && ScoreManager.Instance != null)
            txtErreurs.text = "Erreurs : " + ScoreManager.Instance.erreurs;
    }

    private int CompterDechetsRestants()
    {
        return
            GameObject.FindGameObjectsWithTag("Emballage").Length +
            GameObject.FindGameObjectsWithTag("Verre").Length +
            GameObject.FindGameObjectsWithTag("Aliment").Length;
    }

    private void FinDePartie()
    {
        if (partieFinie)
            return;

        partieFinie = true;

        // Nettoyage de l'interface de saisie.
        if (UIManager.Instance != null)
            UIManager.Instance.CacherNomDechet();

        // Rend le curseur visible sur l'écran final.
        PlayerController.VerrouillerSouris(false);

        if (ecranFin != null)
            ecranFin.SetActive(true);

        if (ScoreManager.Instance == null)
        {
            Debug.LogError("ScoreManager est introuvable.");
            return;
        }

        /*
         * Réussite :
         * - au moins un déchet correctement trié ;
         * - aucune erreur pendant la session.
         *
         * Cette logique fonctionne aussi si de nouveaux déchets
         * sont générés depuis le menu.
         */
        bool succes =
            ScoreManager.Instance.bonsTris > 0 &&
            ScoreManager.Instance.erreurs == 0;

        if (txtResultat != null)
        {
            txtResultat.text = succes ? "SUCCÈS !" : "ÉCHEC";
            txtResultat.color = succes ? Color.green : Color.red;
        }

        if (txtStats != null)
        {
            txtStats.text =
                "Score final : " + ScoreManager.Instance.score +
                "\nBons tris : " + ScoreManager.Instance.bonsTris +
                "\nErreurs : " + ScoreManager.Instance.erreurs +
                "\nTemps : " + txtTemps.text;
        }

        if (DataLogger.Instance != null)
            DataLogger.Instance.Enregistrer(tempsEcoule);
        else
            Debug.LogWarning("DataLogger est introuvable : résultats non enregistrés.");
    }
}