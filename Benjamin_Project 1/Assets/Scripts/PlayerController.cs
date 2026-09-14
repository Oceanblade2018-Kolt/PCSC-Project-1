using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpHeight = 10f;
    public float jumpDetectionDistance = 1.1f;

    PlayerInput PlayerInput;
    Rigidbody rb;
    Ray jumpRay;

    Vector2 moveInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initializing Component Data
        rb = GetComponent<Rigidbody>();
        PlayerInput = GetComponent<PlayerInput>();

        //Setting up new move Vector
        moveInput = new Vector2();
        jumpRay = new Ray(transform.position, -transform.up);
    }

    // Update is called once per frame
    void Update()
    {
        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        Vector3 tempMove = rb.linearVelocity;

        //rb.linearVelocity = (moveInput.x * speed) + (moveInput.y * speed) + (moveInput.z * speed);
        tempMove.x = (moveInput.x * speed) * transform.right.x;
        tempMove.z = (moveInput.y * speed) * transform.forward.z;

        rb.linearVelocity = tempMove;
    }

    public void Move(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump()
    {
        if(Physics.Raycast(jumpRay, jumpDetectionDistance))
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }

        //rb.AddForce(Vector3.up * jumpHeight);
    }

}