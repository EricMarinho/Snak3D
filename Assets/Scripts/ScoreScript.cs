using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class ScoreScript : MonoBehaviour
{
    //
    public PlayerController player;
    TMP_Text ScoreText;
    // Start is called before the first frame update
    void Start(){
       ScoreText = gameObject.GetComponent<TMP_Text>(); 
        
        Debug.Log(ScoreText);
    }



    // Update is called once per frame
    void Update()
    {
         if(Int32.Parse(ScoreText.text)!=player.score){
            ScoreText.text = "" + player.score;
         
         }
         
    }
}
