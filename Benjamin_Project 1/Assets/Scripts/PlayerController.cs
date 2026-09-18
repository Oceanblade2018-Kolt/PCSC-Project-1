using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public bool isAttacking = false;
    public bool takingDamage = false;

    public int health = 5;
    public float speed = 5.0f;
    public float jumpHeight = 10f;
    public float jumpDetectionDistance = 1.1f;
    public float interactDistance = 6f;
    public float hazardCooldown = 3f;


    CinemachinePositionComposer cineCam;
    Camera playerCam;
    PlayerInput PlayerInput;
    Rigidbody rb;

    public Weapon currentWeapon;
    public Transform weaponSlot;
    public GameObject pickupObject;

    Ray jumpRay;
    Ray interactRay;
    RaycastHit interactHit;
    Vector2 moveInput;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Initializing Component Data
        rb = GetComponent<Rigidbody>();
        PlayerInput = GetComponent<PlayerInput>();
        playerCam = Camera.main;

        //Setting up new move Vector
        moveInput = new Vector2();

        jumpRay = new Ray(transform.position, -transform.up);
        interactRay = new Ray(playerCam.transform.position, playerCam.transform.forward);

        weaponSlot = transform.GetChild(0);

        
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
        if(health <= 0)
        {

        }
            //die

        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        interactRay.origin = playerCam.transform.position;
        interactRay.direction = playerCam.transform.forward;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.tag == "Weapon")
            {
                pickupObject = interactHit.collider.gameObject;
            }
        }
        else
        
            pickupObject = null;

        if (currentWeapon)
            if (currentWeapon.holdToAttack && isAttacking)
                currentWeapon.fire();

            Vector3 tempMove = rb.linearVelocity;

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

    public void Reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if(currentWeapon)
            if (currentWeapon.holdToAttack)
            {
                if (context.ReadValueAsButton())
                    isAttacking = true;
                else
                    isAttacking = false;
            }
        else if (context.ReadValueAsButton())
            currentWeapon.fire();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (pickupObject)
            {
                if (pickupObject.tag == "Weapon")
                {
                    pickupObject.GetComponent<Weapon>().equip(this);
                }


            }
            else if (currentWeapon)
            {
                Reload();
            }
        }
    }

    public void DropWeapon()
    {
        if (currentWeapon)
            currentWeapon.unequip();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ammo")
        {
            if(currentWeapon && currentWeapon.ammo < currentWeapon.maxAmmo)
            {

                int ammoFill = currentWeapon.maxAmmo - currentWeapon.ammo;
                if(currentWeapon.maxAmmo - currentWeapon.ammo < currentWeapon.ammoRefill)
                {
                    currentWeapon.ammo += ammoFill;
                }
                else
                {
                    currentWeapon.ammo += currentWeapon.ammoRefill;
                }

                Destroy(collision.gameObject);
            }
        }

        if(collision.gameObject.tag == "Hazard")
        {
            health--;
        }
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Hazard")
        {
            if(!takingDamage)
                StartCoroutine("damageCooldown");
        }

    }
    IEnumerator damageCooldown()
    {
        takingDamage = true;
        yield return new WaitForSeconds(hazardCooldown);
        health--;
        takingDamage = false;
    }
}