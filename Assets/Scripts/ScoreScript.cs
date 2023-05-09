using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class ScoreScript : MonoBehaviour
{

    public PlayerController player;
    TMP_Text scoreText;

    public static ScoreScript instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        scoreText = gameObject.GetComponent<TMP_Text>();
    }

    public void UpdateScore(int score)
    {
        scoreText.SetText("Score: " + score.ToString());
    }

}
