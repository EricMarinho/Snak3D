using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] private GameObject gameOverScreen;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Damage"))
        {
            Time.timeScale = 0;
            gameOverScreen.SetActive(true);
        }
    }

}
