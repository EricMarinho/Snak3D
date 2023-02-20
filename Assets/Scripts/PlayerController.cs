using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public ArrayList arrayBody = new ArrayList();
    public GameObject body;
    public int snakeSize = 0;
    float contador = 0f;
    float backupSpeed = -1;
    float speedMove = 1.0f;
    float speedTime = 0.3f;
    int direction = 1;
    public int score = 0;
    public SpawnFood scriptFood;
    Rigidbody snakeRb;
    bool isForward = true;
    [SerializeField] float speed = 8.0f;
    [SerializeField] private GameObject winScreen;

    // Start is called before the first frame update
    void Start()
    {
        snakeRb = GetComponent<Rigidbody>(); 
        arrayBody.Add(new Vector3(0,0,0));

      
    }

    // Update is called once per frame

   
    void Update()
    {
        contador += Time.deltaTime;

        changeSnakeDirection();
        SpeedUP();

        if(contador >= speedTime){
            moveSnake();
            updateHistory();
            contador = 0f;
        }
        

    }

    void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.CompareTag("Food")){
            Destroy(other.gameObject);
            scriptFood.FoodSpawner();
            score++;
            growBody();
            Instantiate(body);
            if(score > 199)
            {
                Time.timeScale = 0f;
                winScreen.SetActive(true);
            }
            
        }

    }

    void updateHistory(){
        arrayBody[0] = snakeRb.transform.position;

        for (int i = snakeSize; i>=0 ; i--)
        {
            if(i==0){
                arrayBody[0] = snakeRb.transform.position;
            }else{
                arrayBody[i] = arrayBody[i-1];
            }
        }
        
        Debug.Log(arrayBody);
    }

    void growBody(){
        snakeSize++;
        arrayBody.Add(new Vector3(10,99,10));
    }


    void  moveSnake(){

       if(direction==1){
            snakeRb.MovePosition(new Vector3(snakeRb.position.x,snakeRb.position.y,snakeRb.position.z+speedMove));
            isForward = true;
       }else if(direction==2){
            snakeRb.MovePosition(new Vector3(snakeRb.position.x,snakeRb.position.y,snakeRb.position.z-speedMove));
            isForward = true;
       }else if(direction==3){
            snakeRb.MovePosition(new Vector3(snakeRb.position.x-speedMove,snakeRb.position.y,snakeRb.position.z));
            isForward = false;
       }else{
            snakeRb.MovePosition(new Vector3(snakeRb.position.x+speedMove,snakeRb.position.y,snakeRb.position.z));
            isForward = false;
       }

     
    
    }

    void SpeedUP(){
        if(Input.GetKeyDown(KeyCode.Space) && backupSpeed==-1 ){
            backupSpeed = speedTime;
            speedTime = speedTime * 0.3f;
        }else if( Input.GetKeyUp(KeyCode.Space) && backupSpeed!=-1 ) {
            speedTime = backupSpeed;
            backupSpeed=-1;

        }
    }


    void changeSnakeDirection(){


        if(isForward == false){    

            if(Input.GetKeyDown(KeyCode.W)){
                direction = 1;
                GetComponent<Renderer>().material.color = new Color32(0,40,0,50);
            
            }
 
            else if(Input.GetKeyDown(KeyCode.S)){
                direction = 2;
                GetComponent<Renderer>().material.color = new Color32(40,255,165,0);
            }
        }
        if(isForward == true){
            if(Input.GetKeyDown(KeyCode.A)){
                direction = 3;
                GetComponent<Renderer>().material.color = new Color32(255,140,0,60);
            }
            else if(Input.GetKeyDown(KeyCode.D)){
                direction = 4;
                GetComponent<Renderer>().material.color = new Color32(255,69,0,90);
            }
        }
           
    }

}
