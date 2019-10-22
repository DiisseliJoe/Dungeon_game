using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthSystem : MonoBehaviour
{
    private SpriteRenderer SR;
    private int LayerOrder;
    // Start is called before the first frame update
    void Start()
    {
        SR = this.gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    //Changes sorting order of pieces based on their y coordinates
    //making lower pieces seem to be closer to camera
    void Update()
    {
       
            LayerOrder = Mathf.RoundToInt(this.gameObject.transform.position.y * 100);
            LayerOrder = -LayerOrder;
            SR.sortingOrder = LayerOrder;
        
    }
}
