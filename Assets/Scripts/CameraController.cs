using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMoveSpeed = 5f;
    public float cameraRotationSpeed = 30f;
    public float cameraYPosition = 10f;
    public float cameraAngle = 45f;

    public Transform pivotPoint; // The point around which the camera will rotate

    private bool isCursorLocked = false;
    private bool isCameraMoving = false;

    private float rotationInput = 0f;
    private Vector3 movementInput = Vector3.zero;

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
        // Camera movement based on the camera's forward direction (W and S)
        float moveInput = Input.GetAxis("Vertical");
        movementInput = transform.forward * moveInput;

        // Camera movement from side to side (A and D)
        float strafeInput = Input.GetAxis("Horizontal");
        Vector3 strafeMovement = transform.right * strafeInput;

        // Combine both movements
        Vector3 totalMovement = movementInput + strafeMovement;
        Vector3 moveVector = new Vector3(totalMovement.x, 0f, totalMovement.z).normalized * cameraMoveSpeed * Time.deltaTime;
        transform.position += moveVector;

        // Camera rotation with Q and E
        rotationInput = Input.GetAxis("RotateCamera");

        RotateCameraAroundPivot();

        if (Input.GetMouseButtonDown(0) && !isCameraMoving)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;
                StartCoroutine(MoveCamera(targetPosition));
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorLock();
        }
    }

    void ToggleCursorLock()
    {
        isCursorLocked = !isCursorLocked;
        Cursor.lockState = isCursorLocked ? CursorLockMode.Locked : CursorLockMode.Confined;
        Cursor.visible = !isCursorLocked;
    }

    IEnumerator MoveCamera(Vector3 targetPosition)
    {
        isCameraMoving = true;

        Vector3 newPosition = new Vector3(targetPosition.x, cameraYPosition, targetPosition.z);

        while (Vector3.Distance(transform.position, newPosition) > 0.1f)
        {
            Vector3 newPos = Vector3.MoveTowards(transform.position, newPosition, cameraMoveSpeed * Time.deltaTime);
            newPos.y = cameraYPosition; // Maintain the same Y position
            transform.position = newPos;

            Quaternion targetRotation = Quaternion.Euler(cameraAngle, transform.rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * cameraMoveSpeed);
            yield return null;
        }

        isCameraMoving = false;
    }

    void RotateCameraAroundPivot()
    {
        // Rotate the camera around the pivot point
        transform.RotateAround(pivotPoint.position, Vector3.up, rotationInput * cameraRotationSpeed * Time.deltaTime);
    }
}
