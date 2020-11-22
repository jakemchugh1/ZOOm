using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
     if(Random.Range(0,9) == 1)
        {
            Instantiate(FindObjectOfType<TrackSpawner>().milk, transform.position, FindObjectOfType<TrackSpawner>().milk.transform.rotation, transform);
        }   
     else if(Random.Range(0,9) == 2)
        {
            Instantiate(FindObjectOfType<TrackSpawner>().trashcan, transform.position, FindObjectOfType<TrackSpawner>().trashcan.transform.rotation, transform);
        }
    }

    
}
