using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    Transform racer;
    Quaternion target;
    float maxDegreesRotation;
    // Start is called before the first frame update
    void Start()
    {
        racer = FindObjectOfType<RacerBehaviorScript>().transform;
        maxDegreesRotation = 90;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(racer.position.x, 0.5f, racer.position.z);
        if (target != null)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, target, Time.deltaTime*maxDegreesRotation);

        }
    }

    public void setTarget(Vector3 dir)
    {
        target = Quaternion.LookRotation(dir);
    }
}
