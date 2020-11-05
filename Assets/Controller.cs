using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public bool playerControls;

    public float mSpeed;

    public float currentSpeed;

    public float accel;

    public float turn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls)
        {
            if (Input.GetKey(KeyCode.W))
            {
                goForward();
            }else if (Input.GetKey(KeyCode.S))
            {
                goBackward();
            }

            if (Input.GetKey(KeyCode.A))
            {
                turnLeft();
            }
            if (Input.GetKey(KeyCode.D))
            {
                turnRight();
            }
        }

        transform.position += transform.forward * Time.deltaTime * currentSpeed;
    }

    void goForward()
    {
        if(currentSpeed < mSpeed)
        {
            currentSpeed += Time.deltaTime * accel;
        }
    }

    void goBackward()
    {
        if (currentSpeed > -mSpeed/4)
        {
            currentSpeed -= Time.deltaTime * accel * 4;
        }
    }

    void turnLeft()
    {
        transform.rotation *= Quaternion.Euler(0,-turn*Time.deltaTime * currentSpeed,0);
    }

    void turnRight()
    {
        transform.rotation *= Quaternion.Euler(0, turn * Time.deltaTime * currentSpeed, 0);
    }

}
