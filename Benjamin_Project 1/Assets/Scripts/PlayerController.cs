using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    //convert these to a scriptableobject for easy read and modifications

    //PlayerController player;
    public bool isAttacking = false;
    public bool takingDamage = false;
    public bool touchedHazardPool = false;
    public bool touchedBasicEnemy = false;

    public int health = 5;
    public int maxHealth = 5;
    public float speed = 5.0f;
    public float jumpHeight = 10f;
    public float jumpDetectionDistance = 1.1f;
    public float interactDistance = 6f;

    //public float interactDistanceDown = 6f;

    public float hazardPoolCooldown = 3f;
    public float basicEnemyCooldown = 0.5f;

    //public float dynamicPhysics;
    //GetComponent<collider>().PhysicsMaterial.

    CinemachinePositionComposer cineCam;
    Camera playerCam;
    PlayerInput PlayerInput;
    Rigidbody rb;

    public Weapon currentWeapon;
    public Transform weaponSlot;
    public GameObject pickupObject;


    Ray jumpRay;
    Ray interactRay;

    //Ray interactRayDown;
    //RaycastHit interactHitDown;

    RaycastHit interactHit;
    Vector2 moveInput;
    //PhysicsMaterial phys;


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

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //phys = GetComponent<Collider>().material;


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


        //interactRayDown.origin = playerCam.transform.position;
        //interactRayDown.direction = player.transform.up;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.tag == "Weapon")
            {
                pickupObject = interactHit.collider.gameObject;
            }
        }
        else
        
            pickupObject = null;

        /*
        if (Physics.Raycast(interactRayDown, out interactHitDown, interactDistanceDown))
        {
            if (interactHitDown.collider.tag == "Weapon")
            {
                pickupObject = interactHitDown.collider.gameObject;
            }
        }
        else

            pickupObject = null;
        */

        if (currentWeapon)
            if (currentWeapon.holdToAttack && isAttacking)
                currentWeapon.fire();

            Vector3 tempMove = rb.linearVelocity;

        tempMove.x = (moveInput.x * speed);
        tempMove.z = (moveInput.y * speed);

        rb.linearVelocity = (tempMove.x * transform.right) + (tempMove.y * transform.up) + (tempMove.z * transform.forward);

        //GetComponent<Collider>().material.dynamicFriction = 1;

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

        if (collision.gameObject.tag == "Enemy")
        {
            if (!takingDamage)
                StartCoroutine("enemyCooldown");
        }
    }
    IEnumerator damageCooldown()
    {
        takingDamage = true;
        yield return new WaitForSeconds(hazardPoolCooldown);
        health--;
        takingDamage = false;
    }

    
    IEnumerator enemyCooldown()
    {
        takingDamage = true;
        yield return new WaitForSeconds(basicEnemyCooldown);
        health -= (int)2;
        takingDamage = false;
        //make damage a variable in both enemy and enemy data
    }
    
}