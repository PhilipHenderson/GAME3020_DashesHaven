using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopTileGenerator : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject playerFishController;
    public GameObject mainCam;

    [Header("Tile Settings")]
    public GameObject tilePrefab; // Prefab representing a single tile
    public int widthAndHeight; // Number of tiles in width and height
    public float tileSpacing = 1.0f; // Spacing between tiles

    [Header("GameObject Settings")]
    public GameObject sellFish;
    public GameObject popupWindow;
    public Portal portal;

    [Header("Placement Settings")]
    public Transform portalEntrencePos;
    public Transform portalPos;
    public Transform sellFishSpawnPos;

    [Header("Wall Settings")]
    public GameObject wallPrefab; // Prefab representing a wall
    public float wallHeight = 5.0f; // Height of the wall

    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        playerFishController = GameObject.FindGameObjectWithTag("Player");
        popupWindow = GameObject.FindGameObjectWithTag("PopupWindow");
        portal = Instantiate(portal, portalPos.position, Quaternion.identity);
    }

    void Start()
    {
        GenerateShopTiles();
        GenerateWalls();
        playerFishController.transform.position = portalEntrencePos.transform.position;
        mainCam.transform.position = portalEntrencePos.transform.position + new Vector3(0.0f, 8.0f, 0.0f);
        playerFishController.GetComponent<PlayerFishController>().StopAllCoroutines();
        playerFishController.GetComponent<PlayerFishController>().StartCoroutine(playerFishController.GetComponent<PlayerFishController>().MoveToTile(new Vector3(2.0f, 2.0f, 2.0f)));
        portal.destinationSceneName = "City";
        SpawnSellfishAndDesk();
    }

    void GenerateShopTiles()
    {
        for (int x = 0; x < widthAndHeight; x++)
        {
            for (int y = 0; y < widthAndHeight; y++)
            {
                Vector3 tilePosition = new Vector3(x * tileSpacing, 0, y * tileSpacing);
                GameObject newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity);
                newTile.name = "Tile_" + x + "_" + y;
                newTile.transform.SetParent(transform); // Set the parent to keep the hierarchy clean
            }
        }
    }
    void GenerateWalls()
    {
        float wallLength = (widthAndHeight - 1) * tileSpacing;

        // Spawn walls at each corner and rotate them accordingly
        SpawnWall(new Vector3(9.5f, 0, -1.0f), Quaternion.Euler(0, 0, 0), 20.0f);
        SpawnWall(new Vector3(9.5f, 0, 20.0f), Quaternion.Euler(0, 0, 0), 20.0f);
        SpawnWall(new Vector3(20.0f, 0, 9.5f), Quaternion.Euler(0, 90, 0), 20.0f);
        SpawnWall(new Vector3(-1.0f, 0, 9.5f), Quaternion.Euler(0, 90, 0), 20.0f);
    }
    void SpawnWall(Vector3 position, Quaternion rotation, float length)
    {
        GameObject wall = Instantiate(wallPrefab, position, rotation);

        // Scale the wall to cover the entire side of the grid
        wall.transform.localScale = new Vector3(length, wallHeight, 1.0f);
    }
    void SpawnSellfishAndDesk()
    {
        sellFish = Instantiate(sellFish, sellFishSpawnPos.transform.position,Quaternion.identity);
    }

}
