using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToTrackScript : MonoBehaviour
{
    [SerializeField]private Transform checkpoint;
    public float delay;
    public float timer;
    bool hovering;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hovering)
        {
            GetComponent<Controller>().currentSpeed = 0;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = new Vector3();
            GetComponent<Rigidbody>().angularVelocity = new Vector3();
            timer += Time.deltaTime;
            if(timer >= delay)
            {
                hovering = false;
                timer = 0;
            }
        }
        else
        {
            GetComponent<Rigidbody>().useGravity = true;
        }
        if(transform.position.y < -10)
        {
            transform.position = new Vector3(checkpoint.position.x, 1, checkpoint.position.z);
            transform.rotation = checkpoint.rotation;
            GetComponent<Controller>().currentSpeed = 0;
            hovering = true;
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
