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
                instance = FindObjectOfType<MiniMap>();

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
            transform.position = cityMiniMapPosition.position;
            transform.parent = null; 
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

            if (!rotateWithPlayer)
            {
                Quaternion fixedRotation = Quaternion.identity;
                transform.rotation = fixedRotation;
            }

            Vector3 playerPosition = player.transform.position + offsetFromPlayer;
            playerPosition.y = transform.position.y;
            transform.position = playerPosition;
        }

        else if (currentScene.name == "City")
        {
            transform.parent = null;

            transform.position = Vector3.Lerp(transform.position, cityMiniMapPosition.position, smoothSpeed * Time.deltaTime);
        }
    }
}
