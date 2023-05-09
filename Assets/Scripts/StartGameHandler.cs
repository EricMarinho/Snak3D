using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartGameHandler : MonoBehaviour
{

    [SerializeField] private GameObject startGameButton;

    [SerializeField] private SpawnFood foodSpawner;
    [SerializeField] private GameObject audioButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject runButton;
    [SerializeField] private TMP_Text startGameCounterText;

    private void Start()
    {
        startGameCounterText = GetComponent<TMP_Text>();
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        startGameButton.SetActive(false);
        for (int i = 3; i > 0; i--)
        {
            startGameCounterText.text = (i).ToString();
            yield return new WaitForSecondsRealtime(1f);
        }
        startGameCounterText.text = "GO!";
        AudioHandler.instance.PlayMusic();
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;
        foodSpawner.enabled = true;
        PlayerController.instance.enabled = true;
        audioButton.SetActive(false);
        pauseButton.SetActive(true);
        runButton.SetActive(true);
        startGameCounterText.gameObject.SetActive(false);
    }
}
