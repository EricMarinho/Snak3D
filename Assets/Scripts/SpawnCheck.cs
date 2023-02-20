using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCheck : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            Debug.Log("EITAAAAAAAA");
            SpawnFood.instance.FoodSpawner();
            Destroy(gameObject);
        }
    }

}
