using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusScript : MonoBehaviour
{
    
    Transform racer;
    Quaternion target;
    float maxDegreesRotation;
    [SerializeField] float busSpeed;
    // Start is called before the first frame update
    void Start()
    {
        racer = transform.parent;
        maxDegreesRotation = 180;
        Physics.IgnoreCollision(racer.gameObject.GetComponent<BoxCollider>(), GetComponentInChildren<BoxCollider>(), true);
        transform.position = new Vector3(racer.position.x, 0, racer.position.z);
        GetComponent<AudioSource>().volume = GlobalVariables.volume;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<AudioSource>().volume = GlobalVariables.volume;
        if (FindObjectOfType<ScoreKeeperScript>().drivers[0] != racer.GetComponent<RacerBehaviorScript>())
        {
            transform.position = new Vector3(racer.position.x, 0, racer.position.z); //Vector3.Lerp(transform.position,new Vector3(racer.position.x, 0.5f, racer.position.z), 10*Time.deltaTime);
            if (target != null)
            {
                racer.rotation = Quaternion.RotateTowards(racer.rotation, target, Time.deltaTime * maxDegreesRotation);
                Vector3 temp = racer.position;
                racer.position += racer.forward * Time.deltaTime * busSpeed;
                racer.position = new Vector3(racer.position.x, temp.y, racer.position.z);
            }
        }
        else
        {
            Destroy(gameObject);
        }
        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
    }

    public void setTarget(Vector3 dir)
    {
        target = Quaternion.LookRotation(dir);
    }
}
