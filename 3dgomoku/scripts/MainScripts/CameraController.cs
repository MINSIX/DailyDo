using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Vector3 targetPosition = new Vector3(4.5f, 4.5f, 4.5f); // Default target position
    private Vector3 currentPosition= new Vector3(4.5f, 4.5f, 4.5f); // Added variable to store current camera position
    public float rotationSpeed = 5f; // Adjust the rotation speed as needed
    private bool isRotating = false;
    private float rotationOffset = 0f;

    void Start()
    {
        currentPosition = targetPosition; // Initialize currentPosition with the default target position
    }

    void Update()
    {
        HandleRotationInput();

        if (!isRotating)
        {
            if (Input.GetKey(KeyCode.G))
            {
                CameraTarget(new Vector3(11f, 12f, 11f));
            }
            else if (Input.GetKey(KeyCode.H))
            {
                CameraTarget(new Vector3(-1f, 12f, -1f));
            }
            else if (Input.GetKey(KeyCode.J))
            {
                CameraTarget(new Vector3(12f, 0.5f, 12f));
            }
            else if (Input.GetKey(KeyCode.K))
            {
                CameraTarget(new Vector3(-3f, 1f, -3f));
            }
            else
            {
                transform.position = currentPosition;
                // No special key pressed, revert to the default target position
                SetCameraTarget(player.position);
            }
        }
    }

    void HandleRotationInput()
    {
        if (Input.GetKey(KeyCode.Q))
        {

            isRotating = true;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Adjust the rotation based on mouse movement
            rotationOffset += mouseX * rotationSpeed;
            transform.RotateAround(player.position, Vector3.up, mouseX * rotationSpeed);
            transform.RotateAround(player.position, transform.right, -mouseY * rotationSpeed);
            SetCameraTarget(player.position);
        }
        else
        {
            isRotating = false;
        }
    }

    void SetCameraTarget(Vector3 newTargetPosition)
    {
        currentPosition = newTargetPosition;
        transform.position = currentPosition;
        transform.LookAt(newTargetPosition); // Use targetPosition for rotation
    }
    void CameraTarget(Vector3 newTargetPosition)
    {
        transform.position = newTargetPosition;
        transform.LookAt(targetPosition);
    }
}
