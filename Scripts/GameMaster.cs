using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject Player;
    public Camera Cam;
    private Vector3 camMovement;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        camMovement = new Vector3( Player.transform.position.x, Player.transform.position.y, -10);
        Cam.transform.position = camMovement;   
    }
}
