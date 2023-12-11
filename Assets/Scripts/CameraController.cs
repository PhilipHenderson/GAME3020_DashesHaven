using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMoveSpeed = 5f;
    public float cameraRotationSpeed = 30f;
    public float cameraYPosition = 10f;
    public float cameraAngle = 45f;

    public Transform pivotPoint;

    private bool isCursorLocked = false;

    private float rotationInput = 0f;
    private Vector3 movementInput = Vector3.zero;

    private static CameraController instance;
    public static CameraController Instance
    {
        get
        {
            if (instance == null)
            {
                // Try to find an existing instance
                instance = FindObjectOfType<CameraController>();

                // If no instance, create a new one
                if (instance == null)
                {
                    GameObject cameraControllerObject = new GameObject("CameraController");
                    instance = cameraControllerObject.AddComponent<CameraController>();
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
            Debug.Log("CameraController instance created and marked as DontDestroyOnLoad.");
        }
        else
        {
            Debug.Log("Destroying duplicate CameraController instance.");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        Vector3 initialPosition = new Vector3(transform.position.x, cameraYPosition, transform.position.z);
        transform.position = initialPosition;

        Quaternion initialRotation = Quaternion.Euler(cameraAngle, transform.rotation.eulerAngles.y, 0);
        transform.rotation = initialRotation;
    }

    void Update()
    {
        // Camera movement (W and S)
        float moveInput = Input.GetAxis("Vertical");
        movementInput = transform.forward * moveInput;

        // Camera movement (A and D)
        float strafeInput = Input.GetAxis("Horizontal");
        Vector3 strafeMovement = transform.right * strafeInput;

        // Combine  movements
        Vector3 totalMovement = movementInput + strafeMovement;
        Vector3 moveVector = new Vector3(totalMovement.x, 0f, totalMovement.z).normalized * cameraMoveSpeed * Time.deltaTime;
        transform.position += moveVector;

        // Camera rotation Q and E
        rotationInput = Input.GetAxis("RotateCamera");

        RotateCameraAroundPivot();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }
    }

    void ToggleCursorLock()
    {
        isCursorLocked = !isCursorLocked;
        Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.Confined;
        Cursor.visible = true;
    }

    void RotateCameraAroundPivot()
    {
        // Rotate camera around pivot point
        transform.RotateAround(pivotPoint.position, Vector3.up, rotationInput * cameraRotationSpeed * Time.deltaTime);
    }
}
