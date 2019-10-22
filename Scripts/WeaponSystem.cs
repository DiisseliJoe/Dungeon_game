using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public int Damage;
    //armour piercing
    //Range

    public void AsingDamage(int damage)
    {
        Damage = damage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //If gameobject attached to script is players weapon
        if (this.gameObject.layer == 10)
        {
            //if colliding with enemy layer
            if (collision.gameObject.layer == 9)
            {

                //calculates direction to push target. pushes only directly to up,down,left or right
                Vector3 temp = new Vector3(0, 0, 0);
                Vector3 PlayerPosition = this.gameObject.transform.parent.transform.position;
                if (Mathf.Abs(collision.transform.position.x - PlayerPosition.x) > Mathf.Abs(collision.transform.position.y - PlayerPosition.y))
                {
                    temp = new Vector3(collision.transform.position.x - PlayerPosition.x, 0, 0);
                }
                if (Mathf.Abs(collision.transform.position.x - PlayerPosition.x) < Mathf.Abs(collision.transform.position.y - PlayerPosition.y))
                {
                    temp = new Vector3(0, collision.transform.position.y - PlayerPosition.y, 0);
                }
                collision.gameObject.GetComponent<EnemyBehavior>().BacklashDirection = temp;
                //Sends damage ammount to dealdamage funtion to targets healthsystem
                collision.gameObject.GetComponent<HealthSystem>().DealDamage(Damage);
                //Sends information to target has been hit to freeze it for x ammount time
                collision.gameObject.GetComponent<EnemyBehavior>().PlayerHitEnemy();

            }
            //if colliding with object. example box
            if (collision.gameObject.layer == 12)
            {
                //Sends damage ammount to dealdamage funtion to targets healthsystem
                collision.gameObject.GetComponent<HealthSystem>().DealDamage(Damage);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        //if gameobject attached to script is Enemy
        if (this.gameObject.layer == 9)
        {
            //if colliding with player layer
            if (collision.gameObject.layer == 8)
            {
                //if Player is not in invulrenable frame deal damage
                if (!collision.gameObject.GetComponent<PlayerMovement>().InvFrame)
                {
                    //calculates direction to push target. pushes only directly to up,down,left or right
                    Vector3 temp = new Vector3(0, 0, 0);
                    if (Mathf.Abs(collision.transform.position.x - this.transform.position.x) > Mathf.Abs(collision.transform.position.y - this.transform.position.y))
                    {
                        temp = new Vector3(collision.transform.position.x - this.transform.position.x, 0, 0);
                    }
                    if (Mathf.Abs(collision.transform.position.x - this.transform.position.x) < Mathf.Abs(collision.transform.position.y - this.transform.position.y))
                    {
                        temp = new Vector3(0, collision.transform.position.y - this.transform.position.y, 0);
                    }
                    collision.gameObject.GetComponent<PlayerMovement>().BacklashDirection = temp;
                    //Sends damage ammount to dealdamage funtion to targets healthsystem
                    collision.gameObject.GetComponent<HealthSystem>().DealDamage(Damage);
                    //Sends information to target has been hit to freeze it for x ammount time
                    collision.gameObject.GetComponent<PlayerMovement>().EnemyHitPlayer();
                }
            }
        }
    }
}
