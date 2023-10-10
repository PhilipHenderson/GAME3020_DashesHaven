using UnityEngine;

public class Tile : MonoBehaviour
{
    // Add any properties or methods relevant to your tiles here.

    // This tag will help identify the tiles.
    private void Start()
    {
        gameObject.tag = "Tile";
    }
}
