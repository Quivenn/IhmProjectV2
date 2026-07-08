using UnityEngine;
using TMPro;

public class GameSession : MonoBehaviour
{
    public TMP_Text txtErreurs, txtTemps;
    public GameObject ecranFin;
    public TMP_Text txtResultat, txtStats;

    public int scoreObjectif = 5; // succès si score >= objectif

    private float tempsEcoule = 0f;
    private bool partieFinie = false;

    void Update()
    {
        if (partieFinie) return;

        tempsEcoule += Time.deltaTime;
        int min = (int)(tempsEcoule / 60);
        int sec = (int)(tempsEcoule % 60);
        txtTemps.text = string.Format("{0:00}:{1:00}", min, sec);
        txtErreurs.text = "Erreurs: " + ScoreManager.Instance.erreurs;

        // Fin de partie : plus aucun déchet dans la scène
        bool resteDechets =
            GameObject.FindGameObjectsWithTag("Emballage").Length +
            GameObject.FindGameObjectsWithTag("Verre").Length +
            GameObject.FindGameObjectsWithTag("Aliment").Length > 0;

        if (!resteDechets && tempsEcoule > 3f)
            FinDePartie();
    }

    void FinDePartie()
    {
        partieFinie = true;
        ecranFin.SetActive(true);

        bool succes = ScoreManager.Instance.score >= scoreObjectif;
        txtResultat.text = succes ? "SUCCÈS !" : "ÉCHEC";
        txtResultat.color = succes ? Color.green : Color.red;

        txtStats.text =
            "Score final : " + ScoreManager.Instance.score +
            "\nBons tris : " + ScoreManager.Instance.bonsTris +
            "\nErreurs : " + ScoreManager.Instance.erreurs +
            "\nTemps : " + txtTemps.text;

        DataLogger.Instance.Enregistrer(tempsEcoule);
    }
}