using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float jumpHeight = 10f;
    public float jumpDetectionDistance = 1.1f;

    CinemachinePositionComposer cineCam;
    Camera playerCam;
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

        playerCam = Camera.main;
        cineCam = GameObject.Find("CinemachineCamera").GetComponent<CinemachinePositionComposer>();
    }
    private void FixedUpdate()
    {
        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.w = playerCam.transform.rotation.w;
        transform.rotation = playerRotation;

    }

    // Update is called once per frame
    void Update()
    {

        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        Vector3 tempMove = rb.linearVelocity;

        //rb.linearVelocity = (moveInput.x * speed) + (moveInput.y * speed) + (moveInput.z * speed);
        tempMove.x = (moveInput.x * speed);
        tempMove.z = (moveInput.y * speed);

        rb.linearVelocity = (tempMove.x * transform.right) + (tempMove.y * transform.up) + (tempMove.z * transform.forward);
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
    public void shoulderSwap()
    {
        cineCam.TargetOffset.x *= -1;
    }
}