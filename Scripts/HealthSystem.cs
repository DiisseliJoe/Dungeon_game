using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    //Players health should always be divisible by 4. 4,8,12, etc
    //Reason is health bar is made of Heart sprites that have quarters in it so 
    //one heart equals 4 health/hit points.
    public int HealthMax;
    public int Health;
    private float Deathtime;

    // Used for spawning at changed health. example summoning skeletons without fullhealth
    public HealthSystem(int healthMax, int healthcurrent)
    {
        HealthMax = healthMax;
        Health = healthcurrent;

    }
    // Used for spawning fullhealth 
    public HealthSystem(int healthMax)
    {
        HealthMax = healthMax;
        Health = HealthMax;
    }
    //Assing specific health ammount 
    public void AssingHealth(int health)
    {
        Health = health; 
    }
    //Damages target
    public void DealDamage(int damageAmount)
    {
        Health -= damageAmount;
        if (Health <= 0)
        {
            Deathtime = Time.time + 0.2f;
        }
    }
    //heals target 
    public void Heal(int healAmount)
    {
        Health += healAmount;
        if (Health > HealthMax)
        {
            Health = HealthMax;
        }
    }

    //makes sure health never goes above max health
    //Destroy object after death time if below 0 health
    void Update()
    {
        if (Health > HealthMax)
        {
            Health = HealthMax;
        }
        if (Health <= 0)
        {
            if (Time.time > Deathtime)
            {
                Destroy(gameObject);
            }
        }
    }

}
