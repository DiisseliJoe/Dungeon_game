using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS SCRIPT IS PLAYER CONTROLLER. ALL PLAYER RELATED FUNCTIONS ARE HERE.
public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public GameObject SwordRange;
    public BoxCollider2D SwordCollider;
    public float speed = 10;
    //backlash variables
    private float backlash = 2;
    public Vector3 BacklashDirection;
    //Sword gameobject direction floats
    private float SwordDirX = 0.0f;
    private float SwordDirY = -0.3f;
    // AttackSpeed
    private float AttackSpeed = 0.5f;
    private float NextAttack;
    private float AttackTime = 0.3f;
    //hit freeze
    private float FreezeTime = 0.2f;
    private float DAC; //Disable attack collider
    public bool EnemyAttacked = false;
    private float FreezeCooldown;
    //hit colors
    private Color originalColor;
    private Color HitColor = Color.red;
    //invulrenable frames variables
    public bool InvFrame = false;
    private float InvTime;

    private void Awake()
    {
        this.gameObject.GetComponent<HealthSystem>().Health = PlayerPrefs.GetInt("Health");
        if (this.gameObject.GetComponent<HealthSystem>().Health == 0)
        {
            this.gameObject.GetComponent<HealthSystem>().HealthMax = 25;
            this.gameObject.GetComponent<HealthSystem>().Health = 25;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<PlayerHealthBar>().UpdateHealthBar();
        //Moves "Sword" infront of player
        //turns collider off to wait for attack
        SwordRange.transform.position = new Vector3(this.transform.position.x + SwordDirX, this.transform.position.y + SwordDirY, 0);
        SwordCollider.enabled = false;

        originalColor = this.gameObject.GetComponent<SpriteRenderer>().color;

    }

    // Update is called once per frame
    void Update()
    {
        //After cooldown turns freeze off
        if (Time.time > FreezeCooldown && EnemyAttacked)
        {
            EnemyAttacked = false;
            this.gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        }
        //Pushback if hit
        if (EnemyAttacked)
        {
            BacklashDirection = BacklashDirection.normalized * backlash * Time.deltaTime;
            this.transform.position = this.transform.position + BacklashDirection;
        }
        //inv frame
        if (InvTime < Time.time)
        {
            InvFrame = false;
        }

        //Movement is possible if player is not hit
        if (!EnemyAttacked)
        {
            //inputs for movement
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            //Melee Attack
            if (Input.GetButtonDown("Fire1") && Time.time > NextAttack)
            {
                animator.SetTrigger("IsAttacking");
                NextAttack = Time.time + AttackSpeed;
                DAC = Time.time + AttackTime;
                SwordCollider.enabled = true;
                Debug.Log("true");

            }
            //Disables sword collider after attack 
            if (Time.time > DAC)
            {
                SwordCollider.enabled = false;

            }

            //if no keys are pressed instantly stop movement
            //removes "sliding"
            if (Input.GetAxisRaw("Horizontal") == 0)
            {
                h = 0;
            }
            if (Input.GetAxisRaw("Vertical") == 0)
            {
                v = 0;
            }


            //Changes animator falues to change animation
            //also moves "Sword" to face direction player faces
            if (h >= 0)
            {
                animator.SetFloat("RightSpeed", Mathf.Abs(h));
                if (h > 0.01)
                {
                    SwordDirX = 0.3f; SwordDirY = 0.0f;
                    SwordRange.transform.position = new Vector3(this.transform.position.x + SwordDirX, this.transform.position.y + SwordDirY, 0);
                }
            }
            if (h <= 0)
            {
                animator.SetFloat("LeftSpeed", Mathf.Abs(h));
                if (h < -0.01)
                {
                    SwordDirX = -0.3f; SwordDirY = 0.0f;
                    SwordRange.transform.position = new Vector3(this.transform.position.x + SwordDirX, this.transform.position.y + SwordDirY, 0);
                }
            }
            if (v >= 0)
            {
                animator.SetFloat("UpSpeed", Mathf.Abs(v));
                if (v > 0.01)
                {
                    SwordDirX = 0.0f; SwordDirY = 0.3f;
                    SwordRange.transform.position = new Vector3(this.transform.position.x + SwordDirX, this.transform.position.y + SwordDirY, 0);
                }
            }
            if (v <= 0)
            {
                animator.SetFloat("DownSpeed", Mathf.Abs(v));
                if (v < -0.01)
                {
                    SwordDirX = 0.0f; SwordDirY = -0.3f;
                    SwordRange.transform.position = new Vector3(this.transform.position.x + SwordDirX, this.transform.position.y + SwordDirY, 0);
                }
            }

            //Moves player slower speed if moving towards corners
            if (h != 0 && v != 0)
            {
                float CornerSpeed = (speed / 100) * 85;
                Vector3 tempVect = new Vector3(h, v, 0);
                tempVect = tempVect.normalized * CornerSpeed * Time.deltaTime;
                this.transform.position = this.transform.position + tempVect;
            }//regular movement
            else
            {
                Vector3 tempVect = new Vector3(h, v, 0);
                tempVect = tempVect.normalized * speed * Time.deltaTime;
                this.transform.position = this.transform.position + tempVect;
            }
        }
  
    }

    public void EnemyHitPlayer()
    {
        EnemyAttacked = true;
        FreezeCooldown = Time.time + FreezeTime;
        InvFrame = true;
        InvTime = Time.time + FreezeTime * 2;
        this.gameObject.GetComponent<SpriteRenderer>().color = HitColor;
    }


}
