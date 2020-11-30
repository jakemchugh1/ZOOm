using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashcanScript : MonoBehaviour
{
    public GameObject sound;

    private void Start()
    {
        foreach (AudioSource a in GetComponents<AudioSource>())
        {
            a.volume = GlobalVariables.volume;
        }
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            //   check to see if in  last place
            if(other.gameObject.GetComponent<RacerBehaviorScript>() == FindObjectOfType<ScoreKeeperScript>().drivers[3])
            {
                other.gameObject.GetComponent<RacerBehaviorScript>().itemHeld = "bus";
            }
            else
            {
                other.gameObject.GetComponent<RacerBehaviorScript>().itemHeld = "trash";


            }
            other.gameObject.GetComponent<RacerBehaviorScript>().trashcollect = true;
            Instantiate<GameObject>(sound).transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
