using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FonduUI : MonoBehaviour
{
    public float duree = 0.8f;
    private CanvasGroup cg;

    void OnEnable()
    {
        cg = GetComponent<CanvasGroup>();
        if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
        cg.alpha = 0;
        StartCoroutine(Fondu());
    }

    System.Collections.IEnumerator Fondu()
    {
        float t = 0;
        while (t < duree)
        {
            t += Time.deltaTime;
            cg.alpha = t / duree;
            yield return null;
        }
        cg.alpha = 1;
    }
}