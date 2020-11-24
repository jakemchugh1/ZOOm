using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
public class milkScript : MonoBehaviour
{
    public GameObject sound;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<RacerBehaviorScript>().currentSpeed *= 1.5f;
            other.gameObject.GetComponent<RacerBehaviorScript>().setSpeedup();
            var volume = other.GetComponent<Volume>();
            if(volume)
            volume.enabled = true;
            Instantiate<GameObject>(sound).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
