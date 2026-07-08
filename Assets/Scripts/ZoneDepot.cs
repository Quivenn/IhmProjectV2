using UnityEngine;

public class ZoneDepot : MonoBehaviour
{
    [Tooltip("Tag du déchet attendu : Emballage, Verre ou Aliment")]
    public string tagAttendu;

    public AudioClip sonSucces;
    public AudioClip sonErreur;

    private Renderer rendConteneur;
    private Color couleurOrigine;
    private AudioSource audioSource;

    void Start()
    {
        rendConteneur = transform.parent.GetComponent<Renderer>();
        couleurOrigine = rendConteneur.material.color;
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

        Destroy(other.gameObject); // le déchet disparaît une fois déposé
    }

    System.Collections.IEnumerator FlashCouleur(Color c)
    {
        rendConteneur.material.color = c;
        yield return new WaitForSeconds(0.6f);
        rendConteneur.material.color = couleurOrigine;
    }
}