using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public TMP_Text txtScore;
    public TMP_Text txtNomDechet;

    void Awake()
    {
        Instance = this;
    }

    public void MettreAJourScore()
    {
        txtScore.text = "Score: " + ScoreManager.Instance.score;
    }

    public void AfficherNomDechet(string tag)
    {
        txtNomDechet.text = tag;
    }

    public void CacherNomDechet()
    {
        txtNomDechet.text = "";
    }
}