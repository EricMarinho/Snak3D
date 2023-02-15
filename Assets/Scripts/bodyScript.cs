using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bodyScript : MonoBehaviour
{   
    int arrayIndex;
    // Start is called before the first frame update
    void Start()
    {
        arrayIndex = GameObject.Find("SnakeHead").GetComponent<PlayerController>().snakeSize;
        
    }
    //
    // Update is called once per frame
    void Update(){
        Vector3 newPosition = (Vector3)GameObject.Find("SnakeHead").GetComponent<PlayerController>().arrayBody[arrayIndex];
        if(gameObject.transform.position!=newPosition){
           gameObject.transform.position=newPosition;
        }
    }
}
