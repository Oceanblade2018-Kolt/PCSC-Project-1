using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AdvancedEnemy : MonoBehaviour
{



    [SerializeField] private EnemyData data;

    public GameObject pistol;


    public bool isFollowing = false;
    public bool takingDamage = false;



    //[SerializeField] private int health = 3;
    private float shootDistance;

    public int health = 3;


    public PlayerController player;
    public NavMeshAgent agent;
    public EnemyWeapon weapon;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        shootDistance = data.stoppingDistance;
        health = 6;
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();


        ApplyConfiguration();
    }


    void Update()
    {
        if (health <= 0)
        {
            //DIE
            Destroy(pistol);
            Destroy(gameObject);
        }

        float targetDistance = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

        isFollowing = targetDistance <= data.detectionRange;

        if (isFollowing)
        {
            agent.destination = player.transform.position;
            
        }
        ApplyConfiguration();


        if (Vector3.Distance(gameObject.transform.position, player.transform.position) < shootDistance-0.5)
        {
            //print("This is working I think");
            weapon.enemyFire();
        }
        else
        {
            //agent.updateRotation = false;
        }


        //test

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction), 360f * Time.deltaTime);
        }


    }

    public void ApplyConfiguration()
    {
        agent.speed = data.speed;
        agent.acceleration = data.acceleration;
        agent.angularSpeed = data.angularSpeed;
        agent.stoppingDistance = data.stoppingDistance;
        agent.radius = data.radius;
        agent.height = data.height;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "projectile")
        {
            health -= 2;
        }
        else if (collision.gameObject.tag == "eprojectile")
        {
            health--;
        }
    }



}
