using System.Collections;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] private EnemyData data;

    AdvancedEnemy AE;
    PlayerController player;

    public Transform playerTransform;
    public GameObject projectile;
    public Transform firePoint;
    public Transform firingDirection;


    public bool canFire = true;
    public bool reloading = false;
    /*[Header("Weapon Stats")]
    public float projLifeSpan;
    public float projVelocity;
    public float reloadCooldown;
    public float rof = 1f;
    public int clip;
    public int clipSize;*/
    //public float shootDistance;

    private void Awake()
    {
        firingDirection = player.transform;
    }
    void Start()
    {
        //firingDirection = player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enemyFire()
    {
        /*
        //if (canFire && !reloading && clip > 0)
        //{

        //    Vector3 direction = playerTransform.position - firePoint.position;
        //    direction.Normalize();

        //    GameObject p = Instantiate(projectile, firePoint.position, firePoint.rotation);
        //    p.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * projVelocity);
        //    Destroy(p, projLifeSpan);
        //    canFire = false;
        //    //clip--;
        //    StartCoroutine("cooldownFire");

        //}

        //Vector3 direction = playerTransform.position - firePoint.position;
        //direction.Normalize();
        */
        if (canFire == true)
        {
            GameObject p = Instantiate(projectile, firePoint.position, firePoint.rotation);
            p.GetComponent<Rigidbody>().AddForce(firingDirection.transform.forward * data.projVelocity);
            Destroy(p, data.projLifeSpan);
            canFire = false;
            //clip--;
            StartCoroutine("enemyCooldownFire");
        }


    }

    IEnumerator enemyCooldownFire()
    {
        yield return new WaitForSeconds(data.rof);
        canFire = true;
        //if (clip > 0)
        //{
        //    canFire = true;
        //}

    }
    //IEnumerator reloadingCooldown()
    //{
    //    yield return new WaitForSeconds(reloadCooldown);
    //    reloading = false;
    //    canFire = true;


    //}

}
