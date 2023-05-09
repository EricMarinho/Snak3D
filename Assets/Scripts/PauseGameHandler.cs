using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGameHandler : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject audioButton;

    public void PauseGame()
    {
        if (isPaused)
        {
            Time.timeScale = 1;
            audioSource.UnPause();
            isPaused = false;
            audioButton.gameObject.SetActive(false);
            PlayerController.instance.enabled = true;
        }
        else
        {
            Time.timeScale = 0;
            audioSource.Pause();
            isPaused = true;
            audioButton.gameObject.SetActive(true);
            PlayerController.instance.enabled = false;
        }
    }

}
