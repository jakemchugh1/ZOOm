using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacerBehaviorScript : MonoBehaviour
{
    public int place;
    public int playerNumber;
    public Transform checkpoint;
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

    public Camera cam;

    public int lap;

    public float camOffset;

    bool started = false;

    public bool trashcollect = false;
    public GameObject trashbag;
    public bool gotHit = false;

    DirectionIndicator directionIndicator;

    AudioSource[] audio;

    public float startingPitch;

    int selectAnimal;
    // Start is called before the first frame update
    void Start()
    {
        place = 0;
        checkpoint = FindObjectOfType<TileObject>().transform;
        getNextTarget();
        audio = GetComponents<AudioSource>();
        directionIndicator = FindObjectOfType<DirectionIndicator>();
       // cam = FindObjectOfType<Camera>();
         GameObject car = transform.Find("Kart").gameObject;
        if (car){
             var carRenderer = car.GetComponent<Renderer>();
            //  carRenderer.material.SetColor("_Color", Color.blue);
            int colorIndex;
            if(behavior == 0)
            {
                colorIndex = (int)GlobalVariables.selectedCar;
            }
            else
            {
                colorIndex = Random.Range(0, 4);
            }
            switch (colorIndex)
            {
                case(0):
                carRenderer.material.SetColor("_BaseColor", Color.red);
                break;
                case(1):
                carRenderer.material.SetColor("_BaseColor", Color.green);
                break;
                case(2):
                carRenderer.material.SetColor("_BaseColor", Color.blue);
                break;
                case(3):
                carRenderer.material.SetColor("_BaseColor", Color.yellow);
                break;
                default:
                carRenderer.material.SetColor("_BaseColor", Color.white);
                break;
            }
            setCamera();

        }
        StartCoroutine(Countdown());
        //maxSpeed = 2;
        
        if(behavior == 0)
        {
            selectAnimal = (int)GlobalVariables.selectedAnimal;
            setStats(GlobalVariables.selectedAnimal);
        }
        else
        {
            behavior = GlobalVariables.aiDifficulty;
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
        setAudioPitch();
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
    void setStats(int i)
    {

        switch (i)
        {
            case (0):
                maxSpeed = 2.5f;
                acceleration = 0.5f;
                turn = 0.2f;
                break;
            case (1):
                maxSpeed = 2f;
                acceleration = 0.5f;
                turn = 0.25f;
                break;
            case (2):
                maxSpeed = 2f;
                acceleration = 1f;
                turn = 0.2f;
                break;
            case (3):
                maxSpeed = 3f;
                acceleration = 0.75f;
                turn = 0.18f;
                break;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (behavior == 0)
        {
            setCamera();
            throwTrash();
        }
        if (!started) return;
        if (hovering) freeze();
        else runBehavior();
        returnToTrack();
        if (gotHit)
        {
            transform.position = new Vector3(checkpoint.position.x, 1, checkpoint.position.z);
            transform.LookAt(checkpoint.GetComponent<TileObject>().nextTile.transform.position);
            currentSpeed = 0;
            timer = 0;
            hovering = true;
            gotHit = false;
        }
        setAudioPitch();
        if(behavior != 0)dynamicAI();
    }

    void dynamicAI()
    {
        if (place == 1)
        {
            behavior = 2;
        }
        else if (place == 2)
        {
            behavior = 2;
        }
        else if (place == 3)
        {
            behavior = 3;
        }
        else if (place == 4)
        {
            behavior = 3;
        }
    }

    void setAudioPitch()
    {
        for(int i = 0; i < audio.Length; i++)
        {
            audio[i].pitch = startingPitch + Mathf.Abs(currentSpeed / maxSpeed);
        }
        
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
        if (Input.GetKey(KeyCode.S))
        {
            reverse();
        }
        if(!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
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

    void setCamera()
    {
        cam.transform.position = transform.position - (new Vector3(transform.forward.x, -0.25f, transform.forward.z) * camOffset);
        cam.transform.LookAt(transform.position + new Vector3(0, camOffset / 3, 0));
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
        setStats(selectAnimal);
        maxSpeed *= 1.1f;
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
        setStats(selectAnimal);
        maxSpeed *= 1.3f;
        if (getNextTile())
        {
            
            steer(hardSteerTolerance);
            accelerate();
            moveForward();
            //turnTowards(driveTarget);
        }
    }

    void accelerate()
    {
        if(currentSpeed < maxSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
        }else if(currentSpeed > maxSpeed * 1.5f)
        {
            currentSpeed = maxSpeed * 1.5f;
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
        if(currentSpeed > 0)
        {
            currentSpeed -= acceleration * Time.deltaTime;
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        
        if (currentSpeed < 0.15 && currentSpeed >-0.15)
        {
            currentSpeed = 0;
        }
        
    }

    void reverse()
    {
        currentSpeed -= acceleration * Time.deltaTime;
        if (currentSpeed < -maxSpeed/3)
        {
            currentSpeed = -maxSpeed / 3;
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

        if (transform.position.y < -10 || Vector3.Dot(transform.up, Vector3.up) <0)
        {
            transform.position = new Vector3(checkpoint.position.x, 1, checkpoint.position.z);

            transform.LookAt(checkpoint.GetComponent<TileObject>().nextTile.transform.position);
            currentSpeed = 0;
            timer = 0;
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
                if (!trashcollect) driveTarget = getNextTile().getNearestTrashCan(transform.position);
                else driveTarget = getNextTile().getNearestTileNode(transform.position);
                break;
            case (3):
                if(!trashcollect) driveTarget = getNextTile().getNearestTrashCan(transform.position);
                else driveTarget = getNextTile().getNearestMilk(transform.position);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Tile")
        {
            ///set direction indicator
            if(behavior == 0)
            {
                directionIndicator.setTarget(collision.gameObject.GetComponent<TileObject>().nextTile.transform.position - collision.gameObject.transform.position);
                //Debug.Log(collision.gameObject.GetComponent<TileObject>().nextTile.transform.position - collision.gameObject.transform.position);
            }


            //other behavior
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
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(4f);
        started = true;
        cam.GetComponent<AudioSource>().Play();
    }

    void throwTrash()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (trashcollect)
            {
                GameObject temp = Instantiate(trashbag);
                temp.transform.position = transform.position + (transform.forward);
                temp.transform.rotation = transform.rotation;
                trashcollect = false;
            }
        }
    }
}
