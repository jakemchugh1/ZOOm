using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusScript : MonoBehaviour
{
    RacerBehaviorScript racer;
    Quaternion target;
    float maxDegreesRotation;
    // Start is called before the first frame update
    void Start()
    {
        racer = transform.parent.GetComponent<RacerBehaviorScript>();
        maxDegreesRotation = 360;
        setTarget(racer.checkpoint.GetComponent<TileObject>().nextTile.transform.position - racer.checkpoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        
        //transform.position = new Vector3(racer.position.x, 0.5f, racer.position.z); //Vector3.Lerp(transform.position,new Vector3(racer.position.x, 0.5f, racer.position.z), 10*Time.deltaTime);
        if (target != null)
        {
            racer.transform.rotation = Quaternion.RotateTowards(racer.transform.rotation, target, Time.deltaTime * maxDegreesRotation);
            
        }
        setTarget(racer.checkpoint.GetComponent<TileObject>().nextTile.transform.position - racer.checkpoint.position);
    }

    public void setTarget(Vector3 dir)
    {
        target = Quaternion.LookRotation(dir);
    }
}
