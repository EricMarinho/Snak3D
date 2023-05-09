using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCheck : MonoBehaviour
{

    private void FixedUpdate()
    {
        transform.Rotate(0, 60f * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            SpawnFood.instance.FoodSpawner();
            Destroy(gameObject);
        }
    }

}
