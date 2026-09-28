using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{



    [SerializeField] private EnemyData data;

    public bool isFollowing = false;
    public bool takingDamage = false;

    public int health = 3;


    //[SerializeField]  private int health = 3;
    //public int maxHealth = 3;
    //public float detectionRange = 5;

    public PlayerController player;
    public NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        health = 3;

    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        //agent = GetComponent<NavMeshAgent>();
        //if (data != null)
        //{
        //    ApplyConfiguration();
        //}

        ApplyConfiguration();
    }
     
    void Update()
    {
        if (health <= 0)
        {
            //DIE
            //Destroy(gameObject);
            Object.Destroy(gameObject);
        }



        float targetDistance = Mathf.Abs(Vector3.Distance(player.transform.position, transform.position));

        isFollowing = targetDistance <= data.detectionRange;

        if (isFollowing)
        {
            agent.destination = player.transform.position;

        }
        ApplyConfiguration();

         

        //if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 14.0f)
        //{
        //    print("This is working I think");
        //}




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
        if (collision.gameObject.tag == "eprojectile")
        {
            health--;
        }
        if (collision.gameObject.tag == "projectile")
        {
            health -= 2;
        }
    }


}