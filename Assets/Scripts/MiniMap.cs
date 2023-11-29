using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Image miniMapImage; // Reference to your Image UI element.
    public Camera miniMapCamera; // Reference to your mini-map Camera.
    public LayerMask miniMapLayerMask; // Layers to render in the mini-map.

    private RenderTexture renderTexture;

    void Start()
    {
        // Create a Render Texture with the desired resolution
        renderTexture = new RenderTexture(256, 256, 24); // Adjust the size as needed
        miniMapCamera.targetTexture = renderTexture; // Set the Render Texture as the target for the mini-map camera
        miniMapCamera.cullingMask = miniMapLayerMask; // Set the layers to be rendered in the mini-map camera
    }

    void Update()
    {
        if (miniMapImage != null)
        {
            // Set the Render Texture as the texture of the Image
            miniMapImage.sprite = Sprite.Create(TextureFromRenderTexture(renderTexture),
                                                new Rect(0, 0, renderTexture.width, renderTexture.height),
                                                Vector2.one * 0.5f);
        }
    }

    Texture2D TextureFromRenderTexture(RenderTexture rt)
    {
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();
        RenderTexture.active = null;
        return tex;
    }
}
