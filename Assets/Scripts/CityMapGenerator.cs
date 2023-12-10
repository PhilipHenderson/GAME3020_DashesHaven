using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CityMapGenerator : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject player;

    [Header("Tile Settings")]
    public int widthAndHeight;
    public float scale = 2.0f;
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


    [Header("City Wall Settings")]
    public Transform[] wallTransforms = new Transform[4];
    public GameObject cityWallPrefab;
    public float wallHeight = 5.0f;
    public float wallOffset = 5.0f;

    [Header("Building Prefabs")]
    public int buildingGroundHeight = 3;
    public GameObject Shop;
    public GameObject mainCityBuildingPrefab;

    [Header("Building Prefabs")]
    public GameObject popup1;
    public GameObject popup2;
    public GameObject popup3;

    [Header("House1 Settings")]
    public GameObject plot1;
    public GameObject house1;
    public Vector3 house1Pos;
    public bool house1Purchased = false;

    [Header("House2 Settings")]
    public GameObject plot2;
    public GameObject house2;
    public Vector3 house2Pos;
    public bool house2Purchased = false;

    [Header("House3 Settings")]
    public GameObject house3;
    public GameObject plot3;
    public Vector3 house3Pos;
    public bool house3Purchased = false;

    private void Awake()
    {
        popup1 = GameObject.FindGameObjectWithTag("Popup1");
        popup2 = GameObject.FindGameObjectWithTag("Popup2");
        popup3 = GameObject.FindGameObjectWithTag("Popup3");
    }

    void Start()
    {
        width = widthAndHeight;
        height = widthAndHeight;
        GenerateCityTileMap();
        SpawnCityObjects();
        SpawnCityWalls();
        SpawnPopupWindows();
        spawnPlotsAndHouses();
        spawnPos.x = portal.transform.position.x + 10.0f;
        portal.GetComponent<Portal>().destinationSceneName = "SeaBed";
        Instantiate(player, new Vector3(10, 3, 25), Quaternion.identity);
    }

    private void Update()
    {
        if (portal.GetComponent<Portal>().destinationSceneName != "SeaBed")
            portal.GetComponent<Portal>().destinationSceneName = "SeaBed";
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
        SpawnResidentialBuildings();
        SpawnLampposts();
        SpawnPortal();
        SpawnMainCityBuilding();
        SpawnShop();
        // Add more city objects as needed.
    }

    void SpawnMainCityBuilding()
    {
        Vector3 centerPosition = new Vector3(width / 2, -1.0f, height / 2);
        GameObject mainCityBuilding = Instantiate(mainCityBuildingPrefab, centerPosition, Quaternion.identity);
        // Adjust the Y-position to ensure the main city building is above the city tiles.
        mainCityBuilding.transform.position = new Vector3(centerPosition.x, centerPosition.y + buildingOffsetY, centerPosition.z);
    }

    void SpawnResidentialBuildings()
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
            Vector3 position = new Vector3(randomBuildingPosition.x + offset.x, -1.0f, randomBuildingPosition.y + offset.y);

            GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity);
            // Adjust the Y-position to ensure buildings are above the city tiles.
            building.transform.position = new Vector3(position.x, position.y + buildingOffsetY, position.z);
            building.transform.parent = buildingParent.transform;
        }
    }

    void SpawnLampposts()
    {
        GameObject lamppostParent = new GameObject("Lampposts");

        // Define the number of rows in from the perimeter to place lampposts.
        int rowsIn = 10; // Adjust this value as needed.

        // Define the spacing between lampposts (about 20 tiles).
        int spacing = 20; // Adjust this value as needed.

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Check if the current tile is in the outer rows and meets the spacing condition.
                if ((x < rowsIn || x >= width - rowsIn || y < rowsIn || y >= height - rowsIn) &&
                    (x % spacing == 0 && y % spacing == 0))
                {
                    // Calculate the position of the current tile.
                    float positionX = x; // Adjust this if your tiles have different sizes.
                    float positionY = 0.0f;
                    float positionZ = y;

                    // Create a lamppost at the calculated position.
                    Vector3 position = new Vector3(positionX, positionY + lamppostOffsetY, positionZ);
                    GameObject lamppost = Instantiate(lamppostPrefab, position, Quaternion.identity);
                    lamppost.transform.parent = lamppostParent.transform;
                }
            }
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
        Instantiate(portal, new Vector3(5.0f, 1.0f, 8.0f), Quaternion.identity);
    }

    void SpawnCityWalls()
    {
        InstantiateWall(new Vector3(50.0f, wallOffset, 25.0f), Quaternion.identity);
        InstantiateWall(new Vector3(0.0f, wallOffset, 25.0f), Quaternion.Euler(0, 0, 0));
        InstantiateWall(new Vector3(25.0f, wallOffset, 50.0f), Quaternion.Euler(0, 90, 0));
        InstantiateWall(new Vector3(25.0f, wallOffset, 0.0f), Quaternion.Euler(0, 90, 0));
    }

    void InstantiateWall(Vector3 position, Quaternion rotation)
    {
        GameObject wall = Instantiate(cityWallPrefab, position, rotation);
        wall.transform.localScale = new Vector3(1.0f, wallHeight, 10.0f);
    }

    void SpawnShop()
    {
        Instantiate(Shop, new Vector3(3.8f, buildingGroundHeight , 17.0f), Quaternion.identity);
    }

    public void spawnPlotsAndHouses()
    {
        /*
         *  Tasks:
         *  
         *  create a popup window for House Buying
         * 
         *  create function that buys the house
         *  
         *  
         */
        if (plot1 != null)
        {
            SpawnPlot1();
        }
        if (house1 != null && house1Purchased == true)
        {
            SpawnHouse1();
        }
        if (plot2 != null)
        {
            SpawnPlot2();
        }
        if (house2 != null && house2Purchased == true)
        {
            SpawnHouse2();
        }
        if (plot3 != null)
        {
            SpawnPlot3();
        }
        if (house3!= null && house3Purchased == true)
        {
            SpawnHouse3();
        }
    }

    //Spawning Plots and Houses Function
    #region 
    void SpawnPlot1()
    {   
        if(plot1 != null)
            Instantiate(plot1, house1Pos, Quaternion.identity);
    }
    void SpawnHouse1()
    {
        if (house1 != null)
            Instantiate(house1, house1Pos, Quaternion.identity);
    }
    void SpawnPlot2()
    {
        if (plot2 != null)
            Instantiate(plot2, house2Pos, Quaternion.identity);
    }
    void SpawnHouse2()
    {
        if (house2 != null)
            Instantiate(house2, house2Pos, Quaternion.identity);
    }
    void SpawnPlot3()
    {
        if (plot3 != null)
            Instantiate(plot3, house3Pos, Quaternion.identity);
    }
    void SpawnHouse3()
    {
        if (house3 != null)
            Instantiate(house3, house3Pos, Quaternion.identity);
    }
    #endregion

    public void SpawnPopupWindows()
    {
        popup1.SetActive(false);
        popup2.SetActive(false);
        popup3.SetActive(false);
    }

}
// Other functions and logic for spawning additional city objects can be added here.

