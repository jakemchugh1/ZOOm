using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashcanScript : MonoBehaviour
{
    public GameObject trashbag;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //   Debug.Log("Hit");
            other.gameObject.GetComponent<RacerBehaviorScript>().trashcollect = true;
            other.gameObject.GetComponent<RacerBehaviorScript>().trashbag = trashbag;
            Destroy(gameObject);
        }
    }
}
