using UnityEngine;

public class Playermovement : MonoBehaviour
{
   

    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Rigidbody가 없다면 경고 출력
        if (rb == null)
        {
            Debug.LogWarning("Rigidbody not found on the Player game object.");
        }
    }

    void Update()
    {
        if (rb != null)
        {
            MovePlayer();
        }
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Get the camera's forward and right vectors without vertical component
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Normalize vectors to ensure consistent movement speed in all directions
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate movement direction in world space
        Vector3 moveDirection = cameraForward * vertical + cameraRight * horizontal;

        // Normalize the movement direction to prevent faster movement diagonally
        moveDirection.Normalize();

        Vector3 movement = moveDirection * 2;

        // Stop movement when the Shift key is pressed
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            /*rb.AddForce(Vector3.down * jumpForce);
        */
            rb.velocity = new Vector3(0, -1, 0);
        }

        // Move to a specific position when the R key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(-3f, 10f, -3f);
        }

        // 점프
        if (Input.GetButton("Jump"))
        {
            /*  rb.AddForce(Vector3.up * jumpForce);
         */
            rb.velocity = new Vector3(0, 1, 0);
        }

        // 이동
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);
    }
}
