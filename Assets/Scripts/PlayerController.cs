using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public ArrayList arrayBody = new ArrayList();
    public GameObject body;
    public int snakeSize = 0;
    float contador = 0f;
    float backupSpeed = -1;
    float speedMove = 1.0f;
    [SerializeField] float speedTime = 0.3f;
    [SerializeField] private float lightSpeed = 10f;
    private float lightSpeedModifier = 1f;
    [SerializeField] private float modifierMultiplier = 10f;
    int direction = 1;
    public int score = 0;
    public SpawnFood scriptFood;
    Rigidbody snakeRb;
    bool isForward = true;
    [SerializeField] private GameObject winScreen;
    private ScoreScript scoreScriptInstance;
    [SerializeField] private Transform gameLight;
    private AudioHandler audioHandlerInstance;
    [SerializeField] private Button runButton;  

    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    public static PlayerController instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }

        scoreScriptInstance = ScoreScript.instance;
        audioHandlerInstance = AudioHandler.instance;
    }

    void Start()
    {
        snakeRb = GetComponent<Rigidbody>(); 
        arrayBody.Add(new Vector3(0,0,0));
    }

   
    void Update()
    {
        contador += Time.deltaTime;

        HandleLight();
        changeSnakeDirection();
        SnakeRun();

        if(contador >= speedTime){
            moveSnake();
            updateHistory();
            contador = 0f;
        }
    }

    private void SnakeRun()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpeedUP();
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            SpeedDown();
        }
    }

    private void HandleLight()
    {
        gameLight.Rotate(Vector3.up * lightSpeed * lightSpeedModifier * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.CompareTag("Food")){
            Destroy(other.gameObject);
            scriptFood.FoodSpawner();
            score++;
            growBody();
            Instantiate(body);
            scoreScriptInstance.UpdateScore(score);
            StopCoroutine(OnSnakeEat());
            StartCoroutine(OnSnakeEat());
            if(score > 492)
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

    public void SpeedUP(){   
            Time.timeScale = Time.timeScale * 2f;
            lightSpeedModifier += modifierMultiplier;
            audioHandlerInstance.SpeedUpMusic();
    }

    public void SpeedDown()
    {
        audioHandlerInstance.SpeedDownMusic();
        Time.timeScale = Time.timeScale / 2f;
        lightSpeedModifier -= modifierMultiplier;
    }
    


    void changeSnakeDirection(){

        if (isForward == false)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                direction = 1;
                GetComponent<Renderer>().material.color = new Color32(0, 40, 0, 50);

            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                direction = 2;
                GetComponent<Renderer>().material.color = new Color32(40, 255, 165, 0);
            }
        }
        else
        {
            if(Input.GetKeyDown(KeyCode.A)){
                    direction = 3;
                    GetComponent<Renderer>().material.color = new Color32(255,140,0,60);
            }
                else if(Input.GetKeyDown(KeyCode.D)){
                    direction = 4;
                    GetComponent<Renderer>().material.color = new Color32(255,69,0,90);
                }

        }

        //var validTouches = Input.touches.Where(touch => !EventSystem.current.IsPointerOverGameObject(touch.fingerId)).ToArray();

        //if (validTouches.Length > 0)
        //{

        //    if (validTouches[0].phase == TouchPhase.Began)
        //    {
        //        startTouchPosition = validTouches[0].position;
        //    }

        //    if (validTouches[0].phase == TouchPhase.Ended)
        //    {
        //        endTouchPosition = validTouches[0].position;

        //        if (endTouchPosition.x > Screen.width * 0.8f && endTouchPosition.y < Screen.height * 0.2f)
        //            return;

        //        float x = endTouchPosition.x - startTouchPosition.x;
        //        float y = endTouchPosition.y - startTouchPosition.y;

        //        if (isForward == true)
        //        {
        //            if (Mathf.Abs(x) > Mathf.Abs(y))
        //            {
        //                if (x > 0)
        //                {
        //                    if (endTouchPosition.x > Screen.width * 0.70f && endTouchPosition.y < Screen.height * 0.2f)
        //                        return;

        //                    direction = 4;
        //                    GetComponent<Renderer>().material.color = new Color32(255, 69, 0, 90);
        //                }
        //                else
        //                {
        //                    direction = 3;
        //                    GetComponent<Renderer>().material.color = new Color32(255, 140, 0, 60);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (Mathf.Abs(x) < Mathf.Abs(y))
        //            {
        //                if (y > 0)
        //                {
        //                    direction = 1;
        //                    GetComponent<Renderer>().material.color = new Color32(0, 40, 0, 50);
        //                }
        //                else
        //                {
        //                    direction = 2;
        //                    GetComponent<Renderer>().material.color = new Color32(40, 255, 165, 0);
        //                }
        //            }
        //        }
        //    }
        //}
    }

    private IEnumerator OnSnakeEat()
    {
        SpeedUP();
        yield return new WaitForSecondsRealtime(2f);
        SpeedDown();
    }

}
