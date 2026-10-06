using UnityEngine;
//using System;
using System.Collections.Generic;

public class Minigun : Weapon
{
    public Transform firePoint1;
    public Transform firePoint2;
    public Transform firePoint3;
    public Transform firePoint4;
    public Transform firePoint5;
    public Transform firePoint6;
    public Transform firePoint7;
    public Transform firePoint8;
    public Transform firePoint9;

    /*
    public string firePoints1 = "firePoint1";
    public string firePoints2 = "firePoint2";
    public string firePoints3 = "firePoint3";
    public string firePoints4 = "firePoint4";
    public string firePoints5 = "firePoint5";
    public string firePoints6 = "firePoint6";
    public string firePoints7 = "firePoint7";
    public string firePoints8 = "firePoint8";
    public string firePoints9 = "firePoint9";
    
    //string firePoints;
    string[] firePoints = { firePoints1, firePoints2, firePoints3, firePoints4, firePoints5, firePoints6, firePoints7, firePoints8, firePoints9 };
    string chosenFirePoints;
    //char chosenFirePoints;
    //string[] firePoints = { firePoints1, firePoints2 };

    static System.Random rnd = new System.Random();
    public int randomFirePoint;
    void Start()
    {
        //string[] firePoints = { firePoints1, firePoints2, firePoints3, firePoints4, firePoints5, firePoints6, firePoints7, firePoints8, firePoints9 };
        //randomFirePoint = (int)rnd.Next(9);
        
    }

    void Update()
    {
        randomFirePoint = rnd.Next(firePoints.Length);
        string chosenFirePoints = firePoints[randomFirePoint];
        //randomFirePoint = System.Random.Shared.Next(firePoints.Length);
        Debug.Log(chosenFirePoints);
    }
    */

    void Update()
    {
        //fire();
    }
    public override void fire()
    {
        if (canFire && !reloading && clip > 0)
        {
            Transform[] firePoints = { firePoint1, firePoint2, firePoint3, firePoint4, firePoint5, firePoint6, firePoint7, firePoint8, firePoint9 };
            int randomFirePoint = Random.Range(0, firePoints.Length);
            Transform chosenFirePoint = firePoints[randomFirePoint];
            GameObject p = Instantiate(projectile, chosenFirePoint.position, chosenFirePoint.rotation   /*projectile, firePoint.position, firePoint.rotation*/);
            p.GetComponent<Rigidbody>().AddForce(firingDirection.transform.forward * projVelocity);
            Destroy(p, projLifeSpan);
            canFire = false;
            clip--;
            StartCoroutine("cooldownFire");
            Debug.Log("Fired from: " + chosenFirePoint.name);
        }



    }
/*    public void Test()
    {
        randomFirePoint = rnd.Next(firePoints.Length);
        char chosenFirePoints = firePoints[randomFirePoint];
    }*/
}
