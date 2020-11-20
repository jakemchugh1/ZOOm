using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashbagScript : MonoBehaviour
{
    public float speed = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //   Debug.Log("Hit");
            //Do something
            other.gameObject.GetComponent<RacerBehaviorScript>().gotHit = true;
            Destroy(gameObject);
        }
        Destroy(gameObject);
    }
}
