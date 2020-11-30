using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashbagScript : MonoBehaviour
{
    public float speed;
    public GameObject throwSound;
    public GameObject hitSound;
    Rigidbody rb;
    Vector3 vel;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
        rb = GetComponent<Rigidbody>();
        Instantiate<GameObject>(throwSound).transform.position = transform.position;
        //rb.AddForce(transform.forward * speed);
        vel = rb.velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariables.paused || GlobalVariables.finished)
        {
            

        }
        else
        {
            transform.position += transform.forward * Time.deltaTime * speed;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            other.gameObject.GetComponent<RacerBehaviorScript>().gotHit = true;
            Instantiate<GameObject>(hitSound).transform.position = transform.position;
            Destroy(gameObject);
        }
        else
        {
            Instantiate<GameObject>(hitSound).transform.position = transform.position;
            Destroy(gameObject);
        }


    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            collision.gameObject.GetComponent<RacerBehaviorScript>().gotHit = true;

            
        }
        Instantiate<GameObject>(hitSound).transform.position = transform.position;
        Destroy(gameObject);
    }
}

