using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    float timer;
    float limit;
    GameObject power;
    // Start is called before the first frame update
    void Start()
    {
        spawnPower();
        timer = 0;
        limit = 60;
    }

    void Update()
    {
        if (!GlobalVariables.paused && !GlobalVariables.finished)
        {
            timer += Time.deltaTime;
            if (timer >= limit)
            {
                timer = 0;
                spawnPower();
            }
        }
        
    }

    void spawnPower()
    {
        if (!power)
        {
            if (Random.Range(0, 9) == 1)
            {
                power = Instantiate(FindObjectOfType<TrackSpawner>().milk, transform.position, FindObjectOfType<TrackSpawner>().milk.transform.rotation, transform);
            }
            else if (Random.Range(0, 9) == 2)
            {
                power = Instantiate(FindObjectOfType<TrackSpawner>().trashcan, transform.position, FindObjectOfType<TrackSpawner>().trashcan.transform.rotation, transform);
            }
        }
        
    }


}
