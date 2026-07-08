using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDepot : MonoBehaviour
{
    [Tooltip("Tag du déchet attendu : Emballage, Verre ou Aliment")]
    public string tagAttendu;

    [Header("Sons")]
    public AudioClip sonSucces;
    public AudioClip sonErreur;

    private Renderer rendConteneur;
    private Color couleurOrigine;
    private AudioSource audioSource;

    // Évite qu'un même déchet soit compté plusieurs fois.
    private readonly HashSet<int> dechetsTraites = new HashSet<int>();

    private void Start()
    {
        rendConteneur = GetComponentInParent<Renderer>();
        audioSource = GetComponentInParent<AudioSource>();

        if (rendConteneur != null)
            couleurOrigine = rendConteneur.material.color;
        else
            Debug.LogWarning("Renderer du conteneur introuvable.", gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject dechet = TrouverObjetDechet(other);
        string tagDechet = TrouverTagDechet(other);

        if (dechet == null || string.IsNullOrEmpty(tagDechet))
            return;

        int identifiant = dechet.GetInstanceID();

        if (!dechetsTraites.Add(identifiant))
            return;

        // Informe GrabController avant de détruire l'objet.
        if (GrabController.Instance != null)
            GrabController.Instance.NotifierObjetDepose(dechet);

        // Désactive les colliders immédiatement pour éviter un double dépôt.
        foreach (Collider colliderDechet in dechet.GetComponentsInChildren<Collider>())
            colliderDechet.enabled = false;

        if (tagDechet == tagAttendu)
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.TriCorrect();

            StartCoroutine(FlashCouleur(Color.green));

            if (audioSource != null && sonSucces != null)
                audioSource.PlayOneShot(sonSucces);
        }
        else
        {
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.TriIncorrect();

            StartCoroutine(FlashCouleur(Color.red));

            if (audioSource != null && sonErreur != null)
                audioSource.PlayOneShot(sonErreur);
        }

        Destroy(dechet);
    }

    private GameObject TrouverObjetDechet(Collider other)
    {
        Rigidbody rigidbodyDechet = other.attachedRigidbody;

        if (rigidbodyDechet != null)
            return rigidbodyDechet.gameObject;

        if (EstTagDechet(other.gameObject))
            return other.gameObject;

        return null;
    }

    private string TrouverTagDechet(Collider other)
    {
        if (EstTagDechet(other.gameObject))
            return other.gameObject.tag;

        if (other.attachedRigidbody != null &&
            EstTagDechet(other.attachedRigidbody.gameObject))
        {
            return other.attachedRigidbody.gameObject.tag;
        }

        return "";
    }

    private bool EstTagDechet(GameObject objet)
    {
        return objet.CompareTag("Emballage") ||
               objet.CompareTag("Verre") ||
               objet.CompareTag("Aliment");
    }

    private IEnumerator FlashCouleur(Color nouvelleCouleur)
    {
        if (rendConteneur == null)
            yield break;

        rendConteneur.material.color = nouvelleCouleur;

        yield return new WaitForSeconds(0.6f);

        rendConteneur.material.color = couleurOrigine;
    }
}