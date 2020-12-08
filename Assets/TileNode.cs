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
        gameObject.AddComponent<SphereCollider>().isTrigger = true;
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

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Building")
        {
            Transform p = transform.parent;
            transform.SetParent(transform.parent.parent);

            p.GetComponent<TileObject>().nodes = p.GetComponentsInChildren<TileNode>();
            Destroy(gameObject);
        }
        
       // if (other.gameObject.layer == 11) 
    }

    //oncoll




}
