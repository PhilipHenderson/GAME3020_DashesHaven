using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float swimSpeed = 5.0f;
    private Transform fishTransform;

    private void Start()
    {
        fishTransform = transform;
    }

    private void Update()
    {
        // Move the fish forward
        fishTransform.Translate(Vector3.up * swimSpeed * Time.deltaTime);
    }
}