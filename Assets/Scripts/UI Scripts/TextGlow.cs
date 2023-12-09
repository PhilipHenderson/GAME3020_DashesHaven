using UnityEngine;
using TMPro;

public class TextGlowController : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public Color outlineColor = Color.yellow;
    public float outlineWidth = 0.2f;

    void Start()
    {
        // Ensure a TextMeshPro component is assigned
        if (textMeshPro == null)
        {
            textMeshPro = GetComponent<TMP_Text>();
            if (textMeshPro == null)
            {
                Debug.LogError("TextMeshPro component not found!");
                return;
            }
        }

        // Set the outline color and width
        textMeshPro.outlineColor = outlineColor;
        textMeshPro.outlineWidth = outlineWidth;
    }

    // You can add more methods to modify the glow/outline effect dynamically
    public void SetOutlineColor(Color color)
    {
        textMeshPro.outlineColor = color;
    }

    public void SetOutlineWidth(float width)
    {
        textMeshPro.outlineWidth = width;
    }
}
