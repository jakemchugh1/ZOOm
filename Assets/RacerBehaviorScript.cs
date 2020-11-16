using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerBehaviorScript : MonoBehaviour
{
    [SerializeField]private Transform checkpoint;
    public float delay;
    public float timer;
    bool hovering;

    public int behavior;
    public float currentSpeed;
    public int maxSpeed;
    public float acceleration;
    public float turn;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (hovering) freeze();
        else runBehavior();
        returnToTrack();
    }

    void runBehavior()
    {
        switch (behavior)
        {
            case (0):
                player();
                break;
            case (1):
                easyAI();
                break;
            case (2):
                mediumAI();
                break;
            case (3):
                hardAI();
                break;

        }
    }

    void player()
    {
        
    }

    void easyAI()
    {
        if (checkpoint)
        {
            if (checkpoint.gameObject.GetComponent<TileObject>().nextTile)
            {
                accelerate();
                turnTowards(checkpoint.gameObject.GetComponent<TileObject>().nextTile.gameObject);
            }
        }
    }

    void mediumAI()
    {

    }

    void hardAI()
    {

    }

    void accelerate()
    {
        if(currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    void brake()
    {
        currentSpeed -= acceleration * Time.deltaTime;
        if (currentSpeed < 0)
        {
            currentSpeed = 0;
        }
        else
        {
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
        }
        
    }

    void turnTowards(GameObject target)
    {
        //transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        Vector3 temp = target.transform.position - transform.position;
        temp.y = transform.position.y;
        Quaternion look = Quaternion.LookRotation(temp);
        transform.rotation = Quaternion.Lerp(transform.rotation, look, turn);
    }

    void returnToTrack()
    {

        if (transform.position.y < -10)
        {
            transform.position = new Vector3(checkpoint.position.x, 1, checkpoint.position.z);
            transform.rotation = checkpoint.rotation;
            GetComponent<Controller>().currentSpeed = 0;
            hovering = true;
        }
    }

    void freeze()
    {
        GetComponent<Controller>().currentSpeed = 0;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().velocity = new Vector3();
        GetComponent<Rigidbody>().angularVelocity = new Vector3();
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            hovering = false;
            timer = 0;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Tile")
        {
            checkpoint = collision.gameObject.transform;
        }
    }
}
