using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

    [Header("Seaweed Spawn Settings")]
    public int seaweedPatchCount = 5;
    public int seaweedPerPatch = 25;
    public int edgeBuffer = 10;
    public float seaweedSpawnRadius = 10.0f;

    void Start()
    {
        width = widthAndHeight;
        height = widthAndHeight;
        GenerateTileMap();
        SpawnSeaweedPatches();
    }

    void Update()
    {
        // Check if the "R" key is pressed.
        if (Input.GetKeyDown(KeyCode.R))
        {
            RegenerateTilesAndSeaweed();
        }
    }

    void RegenerateTilesAndSeaweed()
    {
        // Destroy existing tiles and seaweed.
        DestroyTiles();
        DestroySeaweed();

        // Generate new tiles and seaweed.
        GenerateTileMap();
        SpawnSeaweedPatches();
    }

    void GenerateTileMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Calculate a height value using Perlin noise.
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float heightValue = Mathf.PerlinNoise(xCoord, yCoord);

                // Create a new tile.
                Vector3 position = new Vector3(x, heightValue, y);
                GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
                newTile.tag = "Tile"; // Set the tag for the tile object.
            }
        }
    }

    void SpawnSeaweedPatches()
    {
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
            }
        }
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

    void DestroySeaweed()
    {
        GameObject[] seaweed = GameObject.FindGameObjectsWithTag("Seaweed");
        foreach (GameObject weed in seaweed)
        {
            Destroy(weed);
        }
    }
}
