using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trashbagScript : MonoBehaviour
{
    public float speed = 10f;
    public GameObject throwSound;
    public GameObject hitSound;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate<GameObject>(throwSound).transform.position = transform.position;
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
            
            other.gameObject.GetComponent<RacerBehaviorScript>().gotHit = true;
            Instantiate<GameObject>(hitSound).transform.position = transform.position;
            Destroy(gameObject);
        }
        
    }
}
