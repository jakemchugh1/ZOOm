using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class milkScript : MonoBehaviour
{
    public GameObject sound;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //   Debug.Log("Hit");
            other.gameObject.GetComponent<RacerBehaviorScript>().currentSpeed *= 1.5f;
            Instantiate<GameObject>(sound).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
