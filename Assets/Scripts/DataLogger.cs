using UnityEngine;
using System.IO;

public class DataLogger : MonoBehaviour
{
    public static DataLogger Instance;

    [Tooltip("Configuration testée : AvecFleches ou SansFleches")]
    public string configuration = "AvecFleches";

    void Awake()
    {
        Instance = this;
    }

    public void Enregistrer(float tempsTotal)
    {
        string chemin = Path.Combine(Application.persistentDataPath, "resultats_st2ihm.csv");

        if (!File.Exists(chemin))
            File.AppendAllText(chemin,
                "Date;Configuration;TempsTotal;Erreurs;BonsTris;Score\n");

        string ligne = string.Format("{0};{1};{2:F1};{3};{4};{5}\n",
            System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            configuration,
            tempsTotal,
            ScoreManager.Instance.erreurs,
            ScoreManager.Instance.bonsTris,
            ScoreManager.Instance.score);

        File.AppendAllText(chemin, ligne);
        Debug.Log("Données enregistrées : " + chemin);
    }
}