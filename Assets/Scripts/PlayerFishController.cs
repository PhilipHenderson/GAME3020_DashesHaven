using System.Collections;
using UnityEngine;

public class PlayerFishController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float aboveTileHeight = 0.8f; // Height above the tile to stand at.
    private bool isMoving = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isMoving) // Check for right-click (1 = right mouse button)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Tile tile = hit.collider.GetComponent<Tile>(); // Assuming you have a Tile component on your tiles.

                if (tile != null)
                {
                    Vector3 targetPosition = tile.transform.position;
                    StartCoroutine(MoveToTile(targetPosition));
                }
            }
        }
    }

    IEnumerator MoveToTile(Vector3 targetPosition)
    {
        isMoving = true;

        Vector3 destination = new Vector3(targetPosition.x, aboveTileHeight, targetPosition.z);

        while (Vector3.Distance(transform.position, destination) > 1.0f)
        {
            // Move towards the destination
            transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

            // Make the fish look at the destination
            transform.LookAt(destination);

            yield return null;
        }

        isMoving = false;
    }

}
