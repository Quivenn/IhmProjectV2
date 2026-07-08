using UnityEngine;

public class ZoneDepot : MonoBehaviour
{
    [Tooltip("Tag du déchet attendu : Emballage, Verre ou Aliment")]
    public string tagAttendu;

    public AudioClip sonSucces;
    public AudioClip sonErreur;

    private Renderer[] renderers;
    private Color[] couleursOrigine;
    private AudioSource audioSource;

    void Start()
    {
        // Récupère TOUS les renderers du conteneur (le parent et ses enfants),
        // en excluant ceux de la zone elle-même
        renderers = transform.parent.GetComponentsInChildren<Renderer>();

        couleursOrigine = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            couleursOrigine[i] = renderers[i].material.color;

        audioSource = transform.parent.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        bool estDechet = other.CompareTag("Emballage") ||
                         other.CompareTag("Verre") ||
                         other.CompareTag("Aliment");
        if (!estDechet) return;

        if (other.CompareTag(tagAttendu))
        {
            ScoreManager.Instance.TriCorrect();
            StartCoroutine(FlashCouleur(Color.green));
            if (sonSucces != null) audioSource.PlayOneShot(sonSucces);
        }
        else
        {
            ScoreManager.Instance.TriIncorrect();
            StartCoroutine(FlashCouleur(Color.red));
            if (sonErreur != null) audioSource.PlayOneShot(sonErreur);
        }

        Destroy(other.gameObject);
    }

    System.Collections.IEnumerator FlashCouleur(Color c)
    {
        foreach (Renderer r in renderers)
            r.material.color = c;

        yield return new WaitForSeconds(0.6f);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = couleursOrigine[i];
    }
}