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
    public float maxSpeed;
    public float acceleration;
    public float turn;
    public GameObject driveTarget;
    private Rigidbody rb;

    public float easySteerTolerance;
    public float mediumSteerTolerance;
    public float hardSteerTolerance;

    public List<GameObject> animals;

    public int lap;
    // Start is called before the first frame update
    void Start()
    {
        //maxSpeed = 2;
        int selectAnimal;
        if(behavior == 0)
        {
            selectAnimal = (int)GlobalVariables.selectedAnimal;
            setStats(GlobalVariables.selectedAnimal);
        }
        else
        {
            selectAnimal = Random.Range(0, 3);
            switch (selectAnimal)
            {
                case (0):
                    setStats(Animal.Bear);
                    break;
                case (1):
                    setStats(Animal.Monkey);
                    break;
                case (2):
                    setStats(Animal.Penguin);
                    break;
                case (3):
                    setStats(Animal.Rabbit);
                    break;
            }
        }
        Vector3 riderPos = new Vector3(0, 0, 0);
        GameObject animal = Instantiate( animals[selectAnimal], riderPos,  Quaternion.identity,transform);
        animal.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        animal.transform.parent = transform;
        animal.transform.localPosition = new Vector3(0, 0.6f, 0);

        rb = GetComponent<Rigidbody>();
        Direction startingDirection = FindObjectOfType<TileObject>().exitDirection;
        switch (startingDirection)
        {
            case (Direction.Left):
                transform.rotation = Quaternion.Euler(0, -90, 0);
                break;
            case (Direction.Right):
                transform.rotation = Quaternion.Euler(0, 90, 0);
                break;
            case (Direction.Up):
                transform.rotation = Quaternion.Euler(0, 0, 0);
                break;
            case (Direction.Down):
                transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
        }
    }

    void setStats(Animal animal)
    {
        
        switch (animal)
        {
            case (Animal.Bear):
                maxSpeed = 2.5f;
                acceleration = 0.5f;
                turn = 0.2f;
                break;
            case (Animal.Monkey):
                maxSpeed = 2f;
                acceleration = 0.5f;
                turn = 0.25f;
                break;
            case (Animal.Penguin):
                maxSpeed = 2f;
                acceleration = 1f;
                turn = 0.2f;
                break;
            case (Animal.Rabbit):
                maxSpeed = 3f;
                acceleration = 0.75f;
                turn = 0.18f;
                break;
        }
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
        if (Input.GetKey(KeyCode.W))
        {
            accelerate();
        }
        else
        {
            brake();
        }
        if (Input.GetKey(KeyCode.S))
        {
            brake();
        }
        if (Input.GetKey(KeyCode.A))
        {
            turnLeft();
        }
        if (Input.GetKey(KeyCode.D))
        {
            turnRight();
        }
        moveForward();
        

    }

    void easyAI()
    {
        if (getNextTile())
        {

            steer(easySteerTolerance);
            accelerate();
            moveForward();
            //turnTowards(driveTarget);
        }
    }

    void mediumAI()
    {
        if (getNextTile())
        {

            steer(mediumSteerTolerance);
            accelerate();
            moveForward();
            //turnTowards(driveTarget);
        }
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
        else
        {
            brake();
        }
        
    }

    void moveForward()
    {
        transform.position += transform.forward * currentSpeed * Time.deltaTime;
    }

    void brake()
    {
        currentSpeed -= acceleration * Time.deltaTime;
        if (currentSpeed < 0)
        {
            currentSpeed = 0;
        }
        
    }

    void steer(float tolerance)
    {
        if (getDotToTargetForward(driveTarget.transform, transform) < 0)
        {
            if (getDotToTargetRight(driveTarget.transform, transform) > 0)
            {
                turnRight();
            }
            else
            {
                turnLeft();
            }
        }
        else
        {
            if (getDotToTargetRight(driveTarget.transform, transform) > tolerance)
            {
                turnRight();
            }
            else if (getDotToTargetRight(driveTarget.transform, transform) < -tolerance)
            {
                turnLeft();
            }

        }
    }

    float getDotToTargetRight(Transform target, Transform origin)
    {
        Vector3 temp = target.transform.position - origin.position;
        temp.y = transform.position.y;
        return Vector3.Dot(origin.right, temp.normalized);
        
    }
    float getDotToTargetForward(Transform target, Transform origin)
    {
        Vector3 temp = target.transform.position - origin.position;
        temp.y = transform.position.y;
        return Vector3.Dot(origin.forward, temp.normalized);

    }

    void turnTowards(GameObject target)
    {
        //transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
        Vector3 temp = target.transform.position - transform.position;
        temp.y = transform.position.y;
        Quaternion look = Quaternion.LookRotation(temp);
        transform.rotation = Quaternion.Lerp(transform.rotation, look, turn);
    }

    void turnLeft()
    {
        transform.rotation *= Quaternion.Euler(0, -turn * 360 * Time.deltaTime, 0);
    }

    void turnRight()
    {
        transform.rotation *= Quaternion.Euler(0, turn * 360 * Time.deltaTime, 0);
    }

    void returnToTrack()
    {

        if (transform.position.y < -10)
        {
            transform.position = new Vector3(checkpoint.position.x, 1, checkpoint.position.z);
            transform.rotation = checkpoint.rotation;
            currentSpeed = 0;
            hovering = true;
        }
    }

    void freeze()
    {
        currentSpeed = 0;
        rb.useGravity = false;
        rb.velocity = new Vector3();
        rb.angularVelocity = new Vector3();
        timer += Time.deltaTime;
        if (timer >= delay)
        {
            hovering = false;
            timer = 0;
            rb.useGravity = true;
        }
    }

    TileObject getNextTile()
    {
        if (checkpoint)
        {
            return checkpoint.gameObject.GetComponent<TileObject>().nextTile;
        }
        else return null;
    }

    void getNextTarget()
    {
        switch (behavior)
        {
            case (0):
                driveTarget = getNextTile().getRandomTileNode();
                break;
            case (1):
                driveTarget = getNextTile().getRandomTileNode();
                break;
            case (2):
                driveTarget = getNextTile().getNearestTileNode(transform.position);
                break;
            case (3):
                driveTarget = getNextTile().getRandomTileNode();
                break;
        }
    }
    void wrongWay()
    {
        //Debug.Log("Wrong Way!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Tile")
        {
            if(checkpoint == null)
            {
                checkpoint = collision.gameObject.transform;
                getNextTarget();
            }
            else if (collision.gameObject.GetComponent<TileObject>() == getNextTile())
            {
                checkpoint = collision.gameObject.transform;
                getNextTarget();
                if (collision.gameObject.GetComponent<TileObject>().tileIndex == 1) lap++;
            }
            else
            {
                wrongWay();
            }
        }
    }
}
