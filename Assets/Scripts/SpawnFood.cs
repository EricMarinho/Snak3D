using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFood : MonoBehaviour
{
    //Singleton

    public static SpawnFood instance;
    
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public GameObject fruta;
    Vector3 LastFruitPosition;

    void Start()
    {
        FoodSpawner();
    }


    public void FoodSpawner(){

        while(true){
               
            Vector3 ActualFruitPosition = new Vector3(Mathf.Floor(Random.Range(-13.0f,14.0f)), 0f, Mathf.Floor(Random.Range(-8.0f,8.0f)));
                
            float distanciaX =  ActualFruitPosition.x - LastFruitPosition.x;
            float distanciaY =  ActualFruitPosition.z - LastFruitPosition.z;

            float Distance = Mathf.Sqrt(distanciaX*distanciaX + distanciaY*distanciaY);
                
            if(Distance>5){
                    
                Instantiate(fruta, ActualFruitPosition, Quaternion.identity);
                LastFruitPosition = ActualFruitPosition;
                break;

            }else{

                continue;
                
            }
     
        }

    }

}