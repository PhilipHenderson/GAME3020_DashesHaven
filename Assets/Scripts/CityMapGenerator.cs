using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CityMapGenerator : MonoBehaviour
{
    [Header("Tile Settings")]
    public int widthAndHeight;
    public float scale = 1.0f;
    public bool IsOccupied { get; private set; }

    private int width;
    private int height;

    [Header("City Tile and Objects")]
    public GameObject cityTilePrefab;
    public GameObject buildingPrefab;
    public GameObject roadPrefab;
    public GameObject lamppostPrefab;

    [Header("Building Spawn Settings")]
    public int buildingCount = 50; // Number of buildings in the city.
    public float buildingSpawnRadius = 10.0f;
    public float buildingRadius = 2.0f; // Adjust this value based on the size of your buildings.
    public int edgeBuffer = 10;

    [Header("Other City Objects Spawn Settings")]
    public int lamppostCount = 20;
    public float lamppostSpawnRadius = 5.0f;

    [Header("Other City Object Settings")]
    public float lamppostOffsetY;
    public float buildingOffsetY;

    [Header("Portal Settings")]
    public GameObject portal;
    public Vector3 spawnPos;

    [Header("Portal Settings")]
    public GameObject player;

    [Header("City Wall Settings")]
    public Transform[] wallTransforms = new Transform[4];
    public GameObject cityWallPrefab;
    public float wallHeight = 5.0f;
    public float wallOffset = 5.0f;

    void Start()
    {
        width = widthAndHeight;
        height = widthAndHeight;
        GenerateCityTileMap();
        SpawnCityObjects();
        SpawnCityWalls();
        spawnPos.x = portal.transform.position.x + 10.0f;
        Instantiate(player, new Vector3(10, 5, 10), Quaternion.identity);
    }

    void GenerateCityTileMap()
    {
        GameObject tileParent = new GameObject("CityTiles");
        HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;

                float heightValue = Mathf.PerlinNoise(xCoord, yCoord);

                Vector3 position = new Vector3(x, heightValue, y);
                GameObject newTile = Instantiate(cityTilePrefab, position, Quaternion.identity);
                newTile.tag = "CityTile";
                newTile.transform.parent = tileParent.transform;

                // Check if the position is occupied and mark it as such.
                Vector2 tilePosition = new Vector2(x, y);
                if (!occupiedPositions.Contains(tilePosition))
                {
                    occupiedPositions.Add(tilePosition);
                }
            }
        }
    }

    void SpawnCityObjects()
    {
        SpawnBuildings();
        SpawnLampposts();
        SpawnPortal();
        // Add more city objects as needed.
    }

    void SpawnBuildings()
    {
        GameObject buildingParent = new GameObject("Buildings");
        HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();

        for (int i = 0; i < buildingCount; i++)
        {
            Vector2 randomBuildingPosition;

            do
            {
                randomBuildingPosition = new Vector2(Random.Range(edgeBuffer, width - edgeBuffer), Random.Range(edgeBuffer, height - edgeBuffer));
            } while (!IsCenterValid(randomBuildingPosition, edgeBuffer) || occupiedPositions.Contains(randomBuildingPosition));

            occupiedPositions.Add(randomBuildingPosition);

            Vector2 offset = Random.insideUnitCircle * buildingSpawnRadius;
            Vector3 position = new Vector3(randomBuildingPosition.x + offset.x, 0.0f, randomBuildingPosition.y + offset.y);

            GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity);
            // Adjust the Y-position to ensure buildings are above the city tiles.
            building.transform.position = new Vector3(position.x, position.y + buildingOffsetY, position.z);
            building.transform.parent = buildingParent.transform;
        }
    }

    void SpawnLampposts()
    {
        GameObject lamppostParent = new GameObject("Lampposts");

        for (int i = 0; i < lamppostCount; i++)
        {
            Vector2 randomLamppostPosition;

            do
            {
                randomLamppostPosition = new Vector2(Random.Range(edgeBuffer, width - edgeBuffer), Random.Range(edgeBuffer, height - edgeBuffer));
            } while (!IsCenterValid(randomLamppostPosition, edgeBuffer));

            Vector2 offset = Random.insideUnitCircle * lamppostSpawnRadius;
            Vector3 position = new Vector3(randomLamppostPosition.x + offset.x, 0.0f, randomLamppostPosition.y + offset.y);

            GameObject lamppost = Instantiate(lamppostPrefab, position, Quaternion.identity);
            // Adjust the Y-position to ensure lampposts are above the city tiles.
            lamppost.transform.position = new Vector3(position.x, position.y + lamppostOffsetY, position.z);
            lamppost.transform.parent = lamppostParent.transform;
        }
    }

    bool IsCenterValid(Vector2 center, int buffer)
    {
        // Check if the center is at least 'buffer' units away from the map edges.
        return center.x >= buffer && center.x <= (width - buffer) && center.y >= buffer && center.y <= (height - buffer);
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }

    public void SpawnPortal()
    {
        Instantiate(portal, new Vector3(5.0f, 1.0f, 5.0f), Quaternion.identity);
    }

    void SpawnCityWalls()
    {
        InstantiateWall(new Vector3(100.0f, wallOffset, 50.0f), Quaternion.identity);
        InstantiateWall(new Vector3(0.0f, wallOffset, 50.0f), Quaternion.Euler(0, 0, 0));
        InstantiateWall(new Vector3(50.0f, wallOffset, 100.0f), Quaternion.Euler(0, 90, 0));
        InstantiateWall(new Vector3(50.0f, wallOffset, 0.0f), Quaternion.Euler(0, 90, 0));
    }

    void InstantiateWall(Vector3 position, Quaternion rotation)
    {
        GameObject wall = Instantiate(cityWallPrefab, position, rotation);
        wall.transform.localScale = new Vector3(1.0f, wallHeight, 10.0f);
    }
}
    // Other functions and logic for spawning additional city objects can be added here.

