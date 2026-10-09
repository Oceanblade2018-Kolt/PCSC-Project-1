using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private EnemyData data;
    //convert these to a scriptableobject for easy read and modifications

    //PlayerController player;
    public bool isAttacking = false;
    public bool takingDamage = false;
    public bool touchedHazardPool = false;
    public bool touchedBasicEnemy = false;
    public bool sprinting = false;
    public bool canSprint = true;
    public bool sprintStop = false;
    public bool staminaStop = false;
    public bool regenStamina = false;
    public bool toggleSprint = true;
    public bool clickJump = false;

    public int health = 5;
    public int maxHealth = 5;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float sprintCost = 0.1f;
    public float sprintBoost = 2.0f;
    public float speed = 5.0f;
    public float jumpHeight = 10f;
    public float jumpDetectionDistance = 1.1f;
    public float interactDistance = 6f;


    //public float maxSlopeAngle = 45f;
    //private RaycastHit groundHit;


    public float sprintCooldown = 2f;
    public float staminaRegen = 5f;
    public float staminaCooldown = 2f;
    //public float interactDistanceDown = 6f;

    public float hazardPoolCooldown = 3f;
    public float basicEnemyCooldown = 0.5f;

    //public float dynamicPhysics;
    //GetComponent<collider>().PhysicsMaterial.

    CinemachinePositionComposer cineCam;
    Camera playerCam;
    PlayerInput PlayerInput;
    Rigidbody rb;
    public GameManager gameManager;
    //public GameManager gm;
    public Weapon currentWeapon;
    public Transform weaponSlot;
    public GameObject pickupObject;


    Ray jumpRay;
    Ray interactRay;

    public PhysicsMaterial physics;

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
        /*physics.dynamicFriction = 0;
        physics.staticFriction = 0;
        physics.frictionCombine = PhysicsMaterialCombine.Minimum;*/


    }
    private void FixedUpdate()
    {
        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.w = playerCam.transform.rotation.w;
        transform.rotation = playerRotation;
        /*//Debug.Log("Before Velocity: " + rb.linearVelocity);
        Vector3 tempMove = rb.linearVelocity;
        tempMove.x = (moveInput.x * speed);
        tempMove.z = (moveInput.y * speed);
        if (rb.linearVelocity.y > 1f)
        {
            Debug.Log("before movement: Y= " + rb.linearVelocity.y);
        }
        rb.linearVelocity = (tempMove.x * transform.right) + (tempMove.y * transform.up) + (tempMove.z * transform.forward);
        //Debug.Log("After Velocity: " + rb.linearVelocity);*/
        /*Vector3 moveDirection = (moveInput.x * transform.right) + (moveInput.y * transform.forward);
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, jumpDetectionDistance))
        {
            moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        }
        Vector3 tempMove = rb.linearVelocity;
        Vector3 horizontalMove = moveDirection.normalized * moveInput.magnitude * speed;
        tempMove.x = horizontalMove.x;
        tempMove.z = horizontalMove.z;
        rb.linearVelocity = new Vector3(tempMove.x, tempMove.y, tempMove.z);*/
    }

    // Update is called once per frame
    void Update()
    {

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


        //bool onSlope = Physics.Raycast(transform.position, -transform.up, out groundHit, 1.1f);


        Vector3 tempMove = rb.linearVelocity;
        tempMove.x = (moveInput.x * speed);
        tempMove.z = (moveInput.y * speed);

        //Vector3 moveDirection = (tempMove.x * transform.right) + (tempMove.z * transform.forward);
        //if (Physics.Raycast(transform.position, -transform.up, out RaycastHit slopeHit, 1.1f))
        //{
        //    moveDirection = Vector3.ProjectOnPlane(moveDirection, slopeHit.normal);
        //}



        if (sprinting)
        {
            if(moveInput.y == 1 && stamina > 0)
            {
                tempMove.z += sprintBoost;

                stamina -= sprintCost * Time.deltaTime;
                if (stamina < 0)
                    stamina = 0;
                StopCoroutine("staminaCD");

                //if (stamina <= 0)
                //{
                //    canSprint = false;
                //    StartCoroutine("sprintCD");
                //    StartCoroutine("staminaCD");
                //    sprinting = false;

                //}

            }
            else
            {
                canSprint = false;
                sprinting = false;
            }

        }
        /*if (!sprinting)
        {
            if (regenStamina)
            {
                stamina += staminaRegen * Time.deltaTime;

                if (stamina >= maxStamina)
                {
                    stamina = maxStamina;
                    regenStamina = false;

                }
            }




        }*/

        if (!sprinting)
        {
            if (!regenStamina && !staminaStop && stamina < maxStamina)
            {
                StartCoroutine("staminaCD");
            }
            if (!canSprint && !sprintStop)
            {
                StartCoroutine("sprintCD");
            }
            if (regenStamina)
            {
                stamina += staminaRegen * Time.deltaTime;

                if (stamina >= maxStamina)
                {
                    stamina = maxStamina;
                    regenStamina = false;
                }
            }
        }


        rb.linearVelocity = (tempMove.x * transform.right) + ( tempMove.y * transform.up) + (tempMove.z * transform.forward);
        

        //rb.linearVelocity = moveDirection + (tempMove.y * transform.up);
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
    }

    public void Move(InputAction.CallbackContext context)
    {

        moveInput = context.ReadValue<Vector2>();
    }

    public void Jump()
    {
        clickJump = true;
        if(Physics.Raycast(jumpRay, jumpDetectionDistance))
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        }
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

    public void Sprint(InputAction.CallbackContext context)
    {
        if (canSprint)
        {
            if (toggleSprint)
            {
                sprinting = !sprinting;
            }
            else if (!toggleSprint)
            {
                /*//if (context.ReadValueAsButton() && canSprint)
                //    sprinting = true;
                //else
                //    sprinting = false;*/
                sprinting = context.ReadValueAsButton();

                if (!sprinting)
                    canSprint = false;
            }
        }

    }

    public void changeFireMode()
    {
        if (currentWeapon)
        {
            if(currentWeapon.fireModes >= 2)
            {
                if(currentWeapon.weaponID == 1)
                {
                    currentWeapon.GetComponent<Rifle>().changeFireMode();
                }

            }
        }
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

        if (collision.gameObject.tag == "Hazard")
        {
            health--;
        }
        if (collision.gameObject.tag == "eprojectile")
        {
            health -= 20;
        }
        if (collision.gameObject.tag == "enemy")
        {
            health -= 5;
        }
        if (collision.gameObject.tag == "Level End")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
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
        health -= (int)15;
        takingDamage = false;
        //make damage a variable in both enemy and enemy data
    }
    IEnumerator sprintCD()
    {
        sprintStop = true;
        yield return new WaitForSeconds(sprintCooldown);

        canSprint = true;
        sprintStop = false;
    }
    IEnumerator staminaCD()
    {
        staminaStop = true;
        yield return new WaitForSeconds(staminaCooldown);

        regenStamina = true;
        staminaStop = false;
    }
}