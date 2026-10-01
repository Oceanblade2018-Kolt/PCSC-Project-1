using UnityEngine;

public class Rifle : Weapon
{
    [Header("Rifle Stats")]
    public float SemiAutoROF = 1;
    public float FullAutoROF = 0.1f;


    public void changeFireMode()
    {
        if(fireModes >= 2 && canFire)
        {
            currentFireMode++;

            if(currentFireMode >= fireModes)
            {
                currentFireMode = 0;
            }

            if(currentFireMode == 0)
            {
                holdToAttack = false;

                rof = SemiAutoROF;
            }
            else if(currentFireMode == 1)
            {
                holdToAttack = true;
                rof = FullAutoROF;

            }



        }


    }

    //public new void fire()
    //{

    //}


}
