using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CityMapGenerator : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject player;
    public GameObject playerPrefab;
    public GameObject playerCitySpawnPosition;

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
    public int buildingCount = 50;
    public float buildingSpawnRadius = 10.0f;
    public float buildingRadius = 2.0f; 
    public int edgeBuffer = 10;

    [Header("Other City Objects Spawn Settings")]
    public int lamppostCount = 20;
    public float lamppostSpawnRadius = 5.0f;

    [Header("Other City Object Settings")]
    public float lamppostOffsetY;
    public float buildingOffsetY;

    [Header("Portal And Bank Settings")]
    public GameObject portal;
    public GameObject bank;
    public Vector3 portalSpawnPos;
    public Vector3 bankSpawnPos;


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

    [Header("Home Settings")]
    public GameObject plot1;
    public GameObject plot2;
    public GameObject plot3;
    public GameObject house1;
    public GameObject house2;
    public GameObject house3;
    public Vector3 house1Pos;
    public Vector3 house2Pos;
    public Vector3 house3Pos;
    public bool house1Purchased = false;
    public bool house2Purchased = false;
    public bool house3Purchased = false;

    [Header("Production Building Settings")]
    public GameObject woodProductionBuildingPrefab;
    public GameObject stoneProductionBuildingPrefab;
    public GameObject coralProductionBuildingPrefab;
    public GameObject foodProductionBuildingPrefab;
    public Vector3 woodProductionPos;
    public Vector3 stoneProductionPos;
    public Vector3 coralProductionPos;
    public Vector3 foodProductionPos;
    public bool woodProductionPurchased = false;
    public bool stoneProductionPurchased = false;
    public bool coralProductionPurchased = false;
    public bool foodProductionPurchased = false;
    private GameObject woodProductionBuilding;
    private GameObject stoneProductionBuilding;
    private GameObject coralProductionBuilding;
    private GameObject foodProductionBuilding;


    private void Awake()
    {
        popup1 = GameObject.FindGameObjectWithTag("Popup1");
        popup2 = GameObject.FindGameObjectWithTag("Popup2");
        popup3 = GameObject.FindGameObjectWithTag("Popup3");

        if (player == null)
        {
            player = Instantiate(playerPrefab);
        }
        else
        {
            Debug.Log("Player Already In Scene");
        }
    }

    void Start()
    {
        playerCitySpawnPosition = GameObject.FindGameObjectWithTag("CitySpawnPosition");
        player = GameObject.FindGameObjectWithTag("Player");
        PositionPlayer(playerCitySpawnPosition.transform.position);
        width = widthAndHeight;
        height = widthAndHeight;
        SpawnCityObjects();
        SpawnPortalAndBank();
        GenerateCityTileMap();
        SpawnCityWalls();
        SpawnPopupWindows();
        spawnPlotsAndHouses();
        CreatingProductionBuildings();
    }

    private void CreatingProductionBuildings()
    {
        woodProductionBuilding = Instantiate(woodProductionBuildingPrefab, woodProductionPos, Quaternion.identity);
        stoneProductionBuilding = Instantiate(stoneProductionBuildingPrefab, stoneProductionPos, Quaternion.identity);
        coralProductionBuilding = Instantiate(coralProductionBuildingPrefab, coralProductionPos, Quaternion.identity);
        foodProductionBuilding = Instantiate(foodProductionBuildingPrefab, foodProductionPos, Quaternion.identity);
    }

    private void SpawnPortalAndBank()
    {
        portalSpawnPos.x = portal.transform.position.x + 10.0f;
        portal.GetComponent<Portal>().destinationSceneName = "SeaBed";
        bank = Instantiate(bank, bankSpawnPos, Quaternion.identity);
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
            building.transform.position = new Vector3(position.x, position.y + buildingOffsetY, position.z);
            building.transform.parent = buildingParent.transform;
        }
    }

    void SpawnLampposts()
    {
        GameObject lamppostParent = new GameObject("Lampposts");

        int rowsIn = 10; 

        int spacing = 20; 

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                if ((x < rowsIn || x >= width - rowsIn || y < rowsIn || y >= height - rowsIn) &&
                    (x % spacing == 0 && y % spacing == 0))
                {

                    float positionX = x; 
                    float positionY = 0.0f;
                    float positionZ = y;

                    Vector3 position = new Vector3(positionX, positionY + lamppostOffsetY, positionZ);
                    GameObject lamppost = Instantiate(lamppostPrefab, position, Quaternion.identity);
                    lamppost.transform.parent = lamppostParent.transform;
                }
            }
        }
    }

    bool IsCenterValid(Vector2 center, int buffer)
    {
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
            Instantiate(house1, house1Pos + new Vector3(0,1.30f,0), Quaternion.identity);
    }
    void SpawnPlot2()
    {
        if (plot2 != null)
            Instantiate(plot2, house2Pos, Quaternion.identity);
    }
    void SpawnHouse2()
    {
        if (house2 != null)
            Instantiate(house2, house2Pos + new Vector3(0, 1.30f, 0), Quaternion.identity);
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

    public void PositionPlayer(Vector3 position)
    {
        player.transform.position = position;
    }
}

