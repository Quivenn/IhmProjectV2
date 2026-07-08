using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public int erreurs = 0;
    public int bonsTris = 0;

    void Awake()
    {
        Instance = this;
    }

    public void TriCorrect()
    {
        score += 1;
        bonsTris += 1;
        UIManager.Instance.MettreAJourScore();
    }

    public void TriIncorrect()
    {
        score -= 1;
        erreurs += 1;
        UIManager.Instance.MettreAJourScore();
    }
}