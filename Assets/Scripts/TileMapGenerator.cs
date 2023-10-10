using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapGenerator : MonoBehaviour
{
    [Header("Tile Map Settings")]
    public int widthAndHeight;
    public float scale = 1.0f;   // Scale of the Perlin noise.

    private int width;        // Width of the tile map.
    private int height;       // Height of the tile map.

    [Header("Tile and Objects")]
    public GameObject tilePrefab; // Reference to your 1x1 cube prefab.
    public GameObject seaweedPrefab; // Reference to your seaweed prefab.
    public GameObject rockPrefab; // Reference to your rock prefab.
    public GameObject coralPrefab; // Reference to your coral prefab.
    public GameObject woodScrapsPrefab; // Reference to your wood scraps prefab.
    public GameObject foodScrapsPrefab; // Reference to your food scraps prefab.
    
    [Header("Seaweed Spawn Settings")]
    public int seaweedPatchCount = 5;  // Number of seaweed patches.
    public int seaweedPerPatch = 25;  // Number of seaweed objects per patch;
    public float seaweedSpawnRadius = 5.0f; // The radius within which seaweed will be spawned.
    public int edgeBuffer = 10;  // Minimum distance from the map edges.

    [Header("Other Objects Spawn Settings")]
    public int otherObjectPatchCount = 3;  // Number of patches for other objects.
    public int objectsPerPatch = 10;  // Number of objects per patch;
    public float objectSpawnRadius = 3.0f; // The radius within which other objects will be spawned.

    void Start()
    {
        width = widthAndHeight;
        height = widthAndHeight;
        GenerateTileMap();
        SpawnObjects();
    }

    void Update()
    {
        // Check if the "R" key is pressed.
        if (Input.GetKeyDown(KeyCode.R))
        {
            RegenerateTilesAndObjects();
        }
    }

    void RegenerateTilesAndObjects()
    {
        // Destroy existing tiles and objects.
        DestroyTiles();
        DestroyObjects();
        DestroySeaweed();

        // Generate new tiles and objects.
        GenerateTileMap();
        SpawnObjects();
    }

    void GenerateTileMap()
    {
        // Create empty GameObjects to serve as parents for organization.
        GameObject tileParent = new GameObject("Tiles");
        GameObject seaweedParent = new GameObject("Seaweed");
        GameObject rockParent = new GameObject("Rocks");

        // Define the number of rows to have a gradual slope.
        int gradualSlopeRows = 10;
        float sandHeight = 1.0f; // Adjust this value for the sand height.

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Calculate a height value using Perlin noise.
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;

                // Calculate the distance from the edge of the map.
                float distanceToEdgeX = Mathf.Min(x, width - x);
                float distanceToEdgeY = Mathf.Min(y, height - y);

                // Calculate the outer rows heights.
                float outerRowHeight = 5.0f;

                // Modify the height based on the position.
                if (distanceToEdgeX <= gradualSlopeRows || distanceToEdgeY <= gradualSlopeRows)
                {
                    // Gradual slope (no gaps)
                    float t = Mathf.Min(x, y, width - x, height - y) / (float)gradualSlopeRows;
                    float heightValue = Mathf.Lerp(outerRowHeight, sandHeight, t);

                    // Create a new tile and set it as a child of the "Tiles" parent.
                    Vector3 position = new Vector3(x, heightValue, y);
                    GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
                    newTile.tag = "Tile"; // Set the tag for the tile object.
                    newTile.transform.parent = tileParent.transform;
                }
                else
                {
                    // Default height for the rest of the map.
                    float heightValue = Mathf.PerlinNoise(xCoord, yCoord);

                    // Create a new tile and set it as a child of the "Tiles" parent.
                    Vector3 position = new Vector3(x, heightValue, y);
                    GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
                    newTile.tag = "Tile"; // Set the tag for the tile object.
                    newTile.transform.parent = tileParent.transform;
                }
            }
        }
    }

    void SpawnObjects()
    {
        SpawnSeaweed();
        SpawnOtherObjects();
    }

    void SpawnSeaweed()
    {
        // Create an empty GameObject to serve as a parent for seaweed organization.
        GameObject seaweedParent = new GameObject("Seaweed");

        for (int patch = 0; patch < seaweedPatchCount; patch++)
        {
            Vector2 randomPatchCenter;

            // Generate a patch center within the specified radius.
            do
            {
                randomPatchCenter = new Vector2(Random.Range(edgeBuffer, width - edgeBuffer), Random.Range(edgeBuffer, height - edgeBuffer));
            } while (!IsCenterValid(randomPatchCenter, edgeBuffer));

            for (int i = 0; i < seaweedPerPatch; i++)
            {
                // Calculate a random position within the specified radius.
                Vector2 offset = Random.insideUnitCircle * seaweedSpawnRadius;

                Vector3 rotationAngle = new Vector3(0, Random.Range(0, 360), 0); // Random rotation around the Y-axis.

                Vector3 patchPosition = new Vector3(randomPatchCenter.x + offset.x, 0.0f, randomPatchCenter.y + offset.y);
                Vector3 seaweedPosition = GetSeaweedPositionOnPatch(patchPosition);

                GameObject Seaweed = Instantiate(seaweedPrefab, seaweedPosition, Quaternion.identity);
                Seaweed.transform.rotation = Quaternion.Euler(rotationAngle);
                Seaweed.tag = "Seaweed"; // Set the tag for the seaweed object.
                Seaweed.transform.parent = seaweedParent.transform;
            }
        }
    }

    void SpawnOtherObjects()
    {
        // Create an empty GameObject to serve as a parent for other objects organization.
        GameObject otherObjectsParent = new GameObject("Other Objects");

        for (int patch = 0; patch < otherObjectPatchCount; patch++)
        {
            Vector2 randomPatchCenter;

            // Generate a patch center within the specified radius.
            do
            {
                randomPatchCenter = new Vector2(Random.Range(edgeBuffer, width - edgeBuffer), Random.Range(edgeBuffer, height - edgeBuffer));
            } while (!IsCenterValid(randomPatchCenter, edgeBuffer));

            for (int i = 0; i < objectsPerPatch; i++)
            {
                // Calculate a random position within the specified radius.
                Vector2 offset = Random.insideUnitCircle * objectSpawnRadius;

                // Random rotation on all axes (X, Y, and Z).
                Vector3 rotationAngle = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

                Vector3 patchPosition = new Vector3(randomPatchCenter.x + offset.x, 0.0f, randomPatchCenter.y + offset.y);

                // Randomly choose an object to spawn.
                GameObject objectPrefab = GetRandomObjectPrefab();

                if (objectPrefab != null)
                {
                    GameObject spawnedObject = Instantiate(objectPrefab, patchPosition, Quaternion.identity);

                    // Adjust the Y-position to ensure objects are above the tiles.
                    float yOffset = GetObjectYOffset(spawnedObject);
                    spawnedObject.transform.position = new Vector3(patchPosition.x, yOffset, patchPosition.z);

                    spawnedObject.transform.rotation = Quaternion.Euler(rotationAngle);
                    spawnedObject.tag = "OtherObject"; // Set the tag for the other object.
                    spawnedObject.transform.parent = otherObjectsParent.transform;
                }
            }
        }
    }

    float GetObjectYOffset(GameObject objectPrefab)
    {
        // Calculate the Y-offset for the object based on its size or specific requirements.
        // You may need to adjust this based on the scale and size of your object prefabs.
        // For example, if the objects should be on top of tiles, you can set yOffset to the height of your tiles.
        return 1.0f; // Adjust this value to match your specific object sizes.
    }


    GameObject GetRandomObjectPrefab()
    {
        // Create an array of object prefabs you want to spawn (e.g., rocks, coral, wood scraps, food scraps).
        GameObject[] objectPrefabs = { rockPrefab, coralPrefab, woodScrapsPrefab, foodScrapsPrefab };

        // Randomly choose an object from the array.
        return objectPrefabs[Random.Range(0, objectPrefabs.Length)];
    }

    Vector3 GetSeaweedPositionOnPatch(Vector3 patchPosition)
    {
        // Randomly scale the seaweed on the Y-axis.
        float randomScaleY = Random.Range(1.0f, 3.0f);
        return new Vector3(patchPosition.x, patchPosition.y + randomScaleY, patchPosition.z);
    }

    bool IsCenterValid(Vector2 center, int buffer)
    {
        // Check if the patch center is at least 'buffer' units away from the map edges.
        return center.x >= buffer && center.x <= (width - buffer) && center.y >= buffer && center.y <= (height - buffer);
    }

    void DestroyTiles()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles)
        {
            Destroy(tile);
        }
    }
    void DestroyObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("OtherObject");
        foreach (GameObject obj in objects)
        {
            Destroy(obj);
        }
    }
    void DestroySeaweed()
    {
        GameObject[] seaweeds = GameObject.FindGameObjectsWithTag("Seaweed");
        foreach (GameObject seaweed in seaweeds)
        {
            Destroy(seaweed);
        }
    }


}