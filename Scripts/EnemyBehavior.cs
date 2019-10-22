using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehavior : MonoBehaviour
{

    public AIPath aiPath;
    private GameObject Player;
    //Detection variables
    private bool detected;
    public float DetectionRange;
    private Transform LastSpotted;
    private LayerMask target;
    private RaycastHit2D hit;

    public Animator animator;
    private bool PlayerAttacked;
    //backlash variables
    private float backlash = 2.5f;
    public Vector3 BacklashDirection;
    //Freeze variables
    private float FreezeCooldown;
    private float FreezeTime = 0.2f;
    //Hit colors
    private Color originalColor;
    private Color HitColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        //Searches GM and takes player object from it and adds it to this script
        GameObject GM = GameObject.FindGameObjectWithTag("GM");
        Player = GM.GetComponent<GameMaster>().Player;
        //Makes raycast only react to walls, boxes and player
        target = LayerMask.GetMask("Obstacles", "Player");
        detected = false;
        //saves objects original color
        originalColor = this.gameObject.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        //After cooldown turns freeze off
        if (Time.time > FreezeCooldown && PlayerAttacked)
        {
            PlayerAttacked = false;
            this.gameObject.GetComponent<SpriteRenderer>().color = originalColor;
        }
        //Pushback if hit
        if (PlayerAttacked)
        {
            BacklashDirection = BacklashDirection.normalized * backlash * Time.deltaTime;
            this.transform.position = this.transform.position + BacklashDirection;
        }
        //if player is in detection range cast raycast to check lineofsight
        //if spotted makes player target for astar pathfinding
        if (Vector3.Distance(Player.transform.position, transform.position) <= DetectionRange)
        { 
            Vector3 Direction = (Player.transform.position - transform.position).normalized;
            hit = Physics2D.Raycast(transform.position,Direction,DetectionRange + 1,target);
            Debug.Log(hit.collider.name);
            //if collider raycast hit is in player layer disables patrol script and enables destination setter script to pursue player
            //also marks player detected and saves the position player was last seen
            if(hit.collider.gameObject.layer == 8)
            {
                this.gameObject.GetComponent<Patrol>().enabled = false;
                this.gameObject.GetComponent<AIDestinationSetter>().enabled = true;
                this.gameObject.GetComponent<AIDestinationSetter>().target = Player.transform;
                LastSpotted = Player.transform;
                detected = true;
            }
            //if the collider raycast hit is not in player layer and detected is true, makes players last known position
            //next waypoint. Detected is changed to false. Destination setter script is disabled and patrol script is enabled
            //enemy will start patrol from closest waypoint;
            if (hit.collider.gameObject.layer != 8 && detected)
            {
                this.gameObject.GetComponent<AIDestinationSetter>().target = LastSpotted;
                detected = false;
                this.gameObject.GetComponent<AIDestinationSetter>().enabled = false;
                this.gameObject.GetComponent<Patrol>().enabled = true;
            }
        }
        //if player leaves detection range + 1 and is still "detected". Stops destination setter script and enables patrol
        if (Vector3.Distance(Player.transform.position, transform.position) >= DetectionRange + 1 && detected)
        {
            this.gameObject.GetComponent<AIDestinationSetter>().target = null;
            detected = false;
            this.gameObject.GetComponent<AIDestinationSetter>().enabled = false;
            this.gameObject.GetComponent<Patrol>().enabled = true;
        }

        // if not attacked changes animation to represent direction this object is moving
        if (!PlayerAttacked)
        {
            if (aiPath.desiredVelocity.x == 0)
            {
                animator.SetFloat("RightSpeed", 0);
                animator.SetFloat("LeftSpeed", 0);
            }
            if (aiPath.desiredVelocity.y == 0)
            {
                animator.SetFloat("UpSpeed", 0);
                animator.SetFloat("DownSpeed", 0);
            }
            if (aiPath.desiredVelocity.x >= 0.01 && Mathf.Abs(aiPath.desiredVelocity.x) > Mathf.Abs(aiPath.desiredVelocity.y))
            {
                animator.SetFloat("RightSpeed", Mathf.Abs(aiPath.desiredVelocity.x));
                animator.SetFloat("UpSpeed", 0);
                animator.SetFloat("DownSpeed", 0);
            }
            if (aiPath.desiredVelocity.x <= -0.01 && Mathf.Abs(aiPath.desiredVelocity.x) > Mathf.Abs(aiPath.desiredVelocity.y))
            {
                animator.SetFloat("LeftSpeed", Mathf.Abs(aiPath.desiredVelocity.x));
                animator.SetFloat("UpSpeed", 0);
                animator.SetFloat("DownSpeed", 0);
            }
            if (aiPath.desiredVelocity.y >= 0.01 && Mathf.Abs(aiPath.desiredVelocity.y) > Mathf.Abs(aiPath.desiredVelocity.x))
            {
                animator.SetFloat("UpSpeed", Mathf.Abs(aiPath.desiredVelocity.y));
                animator.SetFloat("LeftSpeed", 0);
                animator.SetFloat("RightSpeed", 0);
            }
            if (aiPath.desiredVelocity.y <= -0.01 && Mathf.Abs(aiPath.desiredVelocity.y) > Mathf.Abs(aiPath.desiredVelocity.x))
            {
                animator.SetFloat("DownSpeed", Mathf.Abs(aiPath.desiredVelocity.y));
                animator.SetFloat("LeftSpeed", 0);
                animator.SetFloat("RightSpeed", 0);
            }
        }
    }
    //called when Player hits enemy. 
    public void PlayerHitEnemy()
    {
        PlayerAttacked = true;
        FreezeCooldown = Time.time + FreezeTime;
        this.gameObject.GetComponent<SpriteRenderer>().color = HitColor;
        //Debug.Log("HitEnemy");

    }
}
