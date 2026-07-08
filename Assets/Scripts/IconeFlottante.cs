using UnityEngine;
using TMPro;

public class IconeFlottante : MonoBehaviour
{
    private TextMeshPro texte;

    void Start()
    {
        // Crée un texte 3D au dessus de l'objet
        GameObject go = new GameObject("Icone");
        go.transform.SetParent(transform);
        go.transform.localPosition = new Vector3(0, 2.5f, 0);

        texte = go.AddComponent<TextMeshPro>();
        texte.fontSize = 3;
        texte.alignment = TextAlignmentOptions.Center;

        if (CompareTag("Emballage")) { texte.text = "Emballage"; texte.color = Color.yellow; }
        else if (CompareTag("Verre")) { texte.text = "Verre"; texte.color = Color.green; }
        else if (CompareTag("Aliment")) { texte.text = "Aliment"; texte.color = new Color(0.6f, 0.4f, 0.2f); }
    }

    void LateUpdate()
    {
        // Billboard : le texte fait toujours face à la caméra
        if (texte != null && Camera.main != null)
            texte.transform.rotation = Camera.main.transform.rotation;
    }
}