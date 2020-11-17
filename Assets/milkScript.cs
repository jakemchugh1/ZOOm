using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class milkScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //   Debug.Log("Hit");
            other.gameObject.GetComponent<RacerBehaviorScript>().currentSpeed *= 1.5f; 
            Destroy(gameObject);
        }
    }
}
