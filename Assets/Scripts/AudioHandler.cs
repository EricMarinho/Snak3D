using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AudioHandler : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioHandler instance;
    [SerializeField] private Sprite onSound;
    [SerializeField] private Sprite offSound;
    [SerializeField] private Button soundButton;

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
        audioSource = GetComponent<AudioSource>();
        SetAudio();
    }

    private void SetAudio()
    {
        if(PlayerPrefs.GetString("SoundOnOff", "true") == "true")
        {
            audioSource.mute = false;
            soundButton.image.sprite = onSound;
        }
        else
        {
            audioSource.mute = true;
            soundButton.image.sprite = offSound;
        }
    }

    public void PlayMusic()
    {
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SpeedUpMusic()
    {
        audioSource.pitch *= 2f;
    }

    public void SpeedDownMusic()
    {
        audioSource.pitch /= 2f;
    }

    public void ToggleSound()
    {
        if(PlayerPrefs.GetString("SoundOnOff","true") == "true")
        {
            PlayerPrefs.SetString("SoundOnOff", "false");

            audioSource.mute = true;
            soundButton.image.sprite = offSound;
        }
        else
        {
            PlayerPrefs.SetString("SoundOnOff", "true");

            audioSource.mute = false;
            soundButton.image.sprite = onSound;
        }
    }
}
