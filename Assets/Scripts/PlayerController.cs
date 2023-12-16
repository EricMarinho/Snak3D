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
    float counter = 0f;
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

    private bool wasUIPrssed = false;

    private Vector2[] startTouchPositions;
    private Vector2[] endTouchPositions;
    private bool[] touchInProgress;
    private Renderer renderer;

    private void Awake()
    {
        if (instance == null)
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
        renderer = GetComponent<Renderer>();
        snakeRb = GetComponent<Rigidbody>();

        arrayBody.Add(new Vector3(0, 0, 0));

        startTouchPositions = new Vector2[5];
        endTouchPositions = new Vector2[5];
        touchInProgress = new bool[5];
    }


    void Update()
    {
        counter += Time.deltaTime;

        HandleLight();
        changeSnakeDirection();
        SnakeRun();

        if (counter >= speedTime)
        {
            moveSnake();
            updateHistory();
            counter = 0f;
        }
    }

    private void SnakeRun()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpeedUP();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            SpeedDown();
        }
    }

    private void HandleLight()
    {
        gameLight.Rotate(Vector3.up * lightSpeed * lightSpeedModifier * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            scriptFood.FoodSpawner();
            score++;
            growBody();
            Instantiate(body);
            scoreScriptInstance.UpdateScore(score);
            StopCoroutine(OnSnakeEat());
            StartCoroutine(OnSnakeEat());
            if (score > 492)
            {
                Time.timeScale = 0f;
                winScreen.SetActive(true);
            }
        }
    }

    void updateHistory()
    {
        arrayBody[0] = snakeRb.transform.position;

        for (int i = snakeSize; i >= 0; i--)
        {
            if (i == 0)
            {
                arrayBody[0] = snakeRb.transform.position;
            }
            else
            {
                arrayBody[i] = arrayBody[i - 1];
            }
        }
    }

    void growBody()
    {
        snakeSize++;
        arrayBody.Add(new Vector3(10, 99, 10));
    }


    void moveSnake()
    {

        if (direction == 1)
        {
            snakeRb.MovePosition(new Vector3(snakeRb.position.x, snakeRb.position.y, snakeRb.position.z + speedMove));
            isForward = true;
        }
        else if (direction == 2)
        {
            snakeRb.MovePosition(new Vector3(snakeRb.position.x, snakeRb.position.y, snakeRb.position.z - speedMove));
            isForward = true;
        }
        else if (direction == 3)
        {
            snakeRb.MovePosition(new Vector3(snakeRb.position.x - speedMove, snakeRb.position.y, snakeRb.position.z));
            isForward = false;
        }
        else
        {
            snakeRb.MovePosition(new Vector3(snakeRb.position.x + speedMove, snakeRb.position.y, snakeRb.position.z));
            isForward = false;
        }

    }

    public void SpeedUP()
    {
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



    void changeSnakeDirection()
    {

#if !UNITY_ANDROID
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
#else


        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                startTouchPositions[touch.fingerId] = touch.position;
                touchInProgress[touch.fingerId] = true;

                Debug.Log("Touch ID " + touch.fingerId + " started at: " + startTouchPositions[touch.fingerId]);
            }

            if (touchInProgress[touch.fingerId] && touch.phase == TouchPhase.Ended)
            {
                endTouchPositions[touch.fingerId] = touch.position;
                touchInProgress[touch.fingerId] = false;

                HandleSwipe(startTouchPositions[touch.fingerId], endTouchPositions[touch.fingerId]);
            }
        }

#endif
    }

    private IEnumerator OnSnakeEat()
    {
        SpeedUP();
        yield return new WaitForSecondsRealtime(2f);
        SpeedDown();
    }

    void HandleSwipe(Vector2 start, Vector2 end)
    {
        float x = end.x - start.x;
        float y = end.y - start.y;

        if (isForward == true)
        {
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0)
                {
                    Debug.Log("Turn right");
                    direction = 4;
                    renderer.material.color = new Color32(255, 69, 0, 90);
                }
                else
                {
                    Debug.Log("Turn left");
                    direction = 3;
                    renderer.material.color = new Color32(255, 140, 0, 60);
                }
            }
        }
        else
        {
            if (Mathf.Abs(x) < Mathf.Abs(y))
            {
                if (y > 0)
                {
                    Debug.Log("Turn up");
                    direction = 1;
                    renderer.material.color = new Color32(0, 40, 0, 50);
                }
                else
                {
                    Debug.Log("Turn down");
                    direction = 2;
                    renderer.material.color = new Color32(40, 255, 165, 0);

                    Debug.Log("Swipe detected: " + direction);

                }
            }
        }
    }
}