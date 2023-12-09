using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMap : MonoBehaviour
{
    public GameObject player;
    public Transform cityMiniMapPosition;
    public Transform seabedMiniMapPosition;

    public bool rotateWithPlayer = false;
    public float smoothSpeed = 5.0f;

    public Vector3 offsetFromPlayer;

    private static MiniMap instance;

    public static MiniMap Instance
    {
        get
        {
            if (instance == null)
            {
                // Try to find an existing instance in the scene
                instance = FindObjectOfType<MiniMap>();

                // If no instance was found, create a new one
                if (instance == null)
                {
                    GameObject MiniMapObject = new GameObject("MiniMap");
                    instance = MiniMapObject.AddComponent<MiniMap>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("MiniMap instance created and marked as DontDestroyOnLoad.");
        }
        else
        {
            Debug.Log("Destroying duplicate MiniMap instance.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "City")
        {
            // Set the minimap position to the city's position
            transform.position = cityMiniMapPosition.position;
            transform.parent = null; // Remove the parent relationship
        }
    }

    private void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (player == null)
        {
            player = FindAnyObjectByType<GameObject>();
        }

        if (currentScene.name == "SeaBed")
        {
            transform.position = seabedMiniMapPosition.position;

            // Detach the minimap from the player's rotation
            if (!rotateWithPlayer)
            {
                // Set the minimap's rotation to its initial rotation
                Quaternion fixedRotation = Quaternion.identity;
                transform.rotation = fixedRotation;
            }

            // Update the minimap camera's position to keep the player centered
            Vector3 playerPosition = player.transform.position + offsetFromPlayer;
            playerPosition.y = transform.position.y; // Maintain the minimap's height
            transform.position = playerPosition;
        }

        else if (currentScene.name == "City")
        {
            // If in the city scene, detach the minimap from the player
            transform.parent = null;

            // Smoothly interpolate the position towards the city's position
            transform.position = Vector3.Lerp(transform.position, cityMiniMapPosition.position, smoothSpeed * Time.deltaTime);
        }
    }
}
