using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
    RacerBehaviorScript racer;
    // Start is called before the first frame update
    void Start()
    {
        racer = transform.parent.gameObject.GetComponent<RacerBehaviorScript>();
        Debug.Log(transform.parent.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            
            if (racer.trashcollect)
            {
                
                racer.throwTrash();
            }
        }
        
    }
}
