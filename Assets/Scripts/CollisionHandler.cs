using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject inGameHighScore;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject runButton;
    [SerializeField] private TMP_Text highScoreText;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Damage"))
        {
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);
            inGameHighScore.SetActive(false);
            highScoreText.SetText("Score: " + PlayerController.instance.score.ToString());
            AudioHandler.instance.StopMusic();
            pauseButton.SetActive(false);
            runButton.SetActive(false);
            PlayerController.instance.enabled = false;
            //InterstitialAds.Instance.ShowAd();
            
        }
    }

}
