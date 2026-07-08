using UnityEngine;

public class FlecheGuidage : MonoBehaviour
{
    public Transform joueur; // la Main Camera

    void Update()
    {
        Transform cible = TrouverDechetLePlusProche();
        if (cible == null)
        {
            // Plus de déchets : on cache la flèche
            foreach (Renderer r in GetComponentsInChildren<Renderer>())
                r.enabled = false;
            return;
        }

        foreach (Renderer r in GetComponentsInChildren<Renderer>())
            r.enabled = true;

        // La flèche se place devant le joueur et pointe vers la cible
        Vector3 posJoueur = joueur.position;
        posJoueur.y = 0.05f;
        transform.position = posJoueur + joueur.forward.normalized * 1.5f
                             * new Vector3(1, 0, 1).magnitude;

        Vector3 dir = cible.position - transform.position;
        dir.y = 0;
        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir) * Quaternion.Euler(0, -90, 0);
    }

    Transform TrouverDechetLePlusProche()
    {
        Transform plusProche = null;
        float minDist = Mathf.Infinity;
        string[] tags = { "Emballage", "Verre", "Aliment" };

        foreach (string t in tags)
        {
            foreach (GameObject g in GameObject.FindGameObjectsWithTag(t))
            {
                float d = Vector3.Distance(joueur.position, g.transform.position);
                if (d < minDist) { minDist = d; plusProche = g.transform; }
            }
        }
        return plusProche;
    }
}