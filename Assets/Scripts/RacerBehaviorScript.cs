using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering;

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

    public Slider  volumSlider;
    private float maxEngineVolume = 0.25f;

    DirectionIndicator directionIndicator;

    AudioSource[] audio;

    public float startingPitch;

    int selectAnimal;

    public RectTransform pauseMenu;

    public RectTransform finishMenu;

    public RectTransform trashSprite;

    public bool isSpeedup;
    public float startSpeedup = 0;
    public float maxSpeedup = 30;

    public ParticleSystem particles;
    public ParticleSystem backlight;


    public float driftTimer;
    public float driftLimit;

    public bool driftingLeft;
    public bool driftingRight;

    public float generalTimer;

    public Vector3 nextTileDir;

    public GameObject bus;
    public GameObject busPrefab;

    public string itemHeld;
    // Start is called before the first frame update
    void Start()
    {
        itemHeld = "";
        generalTimer = 0;
        GlobalVariables.finished = false;
        GlobalVariables.paused = false;
        
        place = 0;
        checkpoint = FindObjectOfType<TileObject>().transform;
        getNextTarget();
        audio = GetComponents<AudioSource>();
        maxEngineVolume = audio[0].volume;
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
                acceleration = 1f;
                turn = 0.05f;
                break;
            case (Animal.Monkey):
                maxSpeed = 2.5f;
                acceleration = .8f;
                turn = 0.09f;
                break;
            case (Animal.Penguin):
                maxSpeed = 2.5f;
                acceleration = .7f;
                turn = 0.07f;
                break;
            case (Animal.Rabbit):
                maxSpeed = 2.5f;
                acceleration = 0.5f;
                turn = 0.1f;
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

    private void Update()
    {
        
        //if (Input.GetKeyDown(KeyCode.F) && behavior == 0  && !GlobalVariables.finished) finish();
        if(behavior == 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && started && !GlobalVariables.finished) pause();


            if (trashcollect) trashSprite.gameObject.SetActive(true);
            else trashSprite.gameObject.SetActive(false);
        }
        if(this.isSpeedup)
        {
            if(this.startSpeedup <= this.maxSpeedup)
            {
                this.startSpeedup++;
            }
            else
            {
                 if(backlight)
                backlight.Stop();
                this.startSpeedup = 0;
                this.isSpeedup = false;
                 var volume = gameObject.GetComponent<Volume>();
                if(volume)
                volume.enabled = false;
               

            }
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (!GlobalVariables.paused && !GlobalVariables.finished)
        {
            if (behavior == 0)
            {
                pauseMenu.gameObject.SetActive(false);
                setCamera();
            }
            if (!started) return;
            if (hovering) freeze();
            else runBehavior();
            returnToTrack();
            if (gotHit)
            {
                delayRacer();
            }
            setAudioPitch();
            if (behavior != 0) dynamicAI();
        }
        else if(behavior == 0)
        {
            
            if (!GlobalVariables.finished) 
            {
                pauseMenu.gameObject.SetActive(true);
                volumSlider.value = GlobalVariables.volume;
                volumSlider.onValueChanged.AddListener (delegate {onChangeValue();});

            }
            else finishMenu.gameObject.SetActive(true);
        }
        
    }

    void setChildrenLocalRotation(Quaternion rot)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localRotation = rot;
        }
    }

    void addChildrenLocalRotation(Quaternion rot)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).localRotation *= rot;
        }
    }

    private void delayRacer()
    {
        if(currentSpeed > 0)
        {
            currentSpeed -= Time.deltaTime * 3f;
            if (currentSpeed < 0) currentSpeed = 0;
        }
        addChildrenLocalRotation(Quaternion.Euler(0, 360 * Time.deltaTime, 0));
        generalTimer += Time.deltaTime;
        if(generalTimer > 2)
        {
            setChildrenLocalRotation(Quaternion.Euler(0, 0, 0));
            //transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
            //transform.GetChild(2).localRotation = Quaternion.Euler(0, 0, 0);
            generalTimer = 0;
            gotHit = false;
        }
    }

    void onChangeValue()
    {
        cam.GetComponent<AudioSource>().volume = volumSlider.value;
        GlobalVariables.volume = volumSlider.value;
        GameObject[] riders = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject rider in riders)
        {
            RacerBehaviorScript rs = rider.GetComponent<RacerBehaviorScript>();
            rs.setEngineVolume(volumSlider.value);
        }
        if (behavior == 0) audio[2].volume = GlobalVariables.volume;
    }

    void finish()
    {
        GlobalVariables.finished = true;
    }
    public void quit()
    {
        Application.Quit();
    }
    public void pause()
    {
        GlobalVariables.paused = !GlobalVariables.paused;
    }
    public void returnToMain()
    {
        SceneManager.LoadScene(0);
    }
    public void resetRace()
    {
        SceneManager.LoadScene(2);
    }

    void dynamicAI()
    {
        if (place == 1)
        {
            behavior = 1;
        }
        else if (place == 2)
        {
            behavior = 2;
        }
        else if (place == 3)
        {
            behavior = 2;
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

    public void setEngineVolume(float value)
    {

        for(int i = 0; i < audio.Length; i++)
        {
            audio[i].volume = value*maxEngineVolume;
        }
    }

    void runBehavior()
    {
        
        if (!bus)
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
        
    }

    void player()
    {
        ///if the player is drifting to the left
        if (driftingLeft)
        {
            driftLeft();
        }
        //if the player is drifting to the right
        else if (driftingRight)
        {
            driftRight();
        }
        //else normal inputs
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                accelerate();
            }
            if (Input.GetKey(KeyCode.S))
            {
                reverse();
            }
            if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
            {
                brake();
            }
            if (Input.GetKey(KeyCode.A))
            {

                turnLeft();
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    addChildrenLocalRotation(Quaternion.Euler(0, -45, 0));
                    driftingLeft = true;
                    driftTimer = 0;
                    audio[2].Play();
                }
                
            }
            if (Input.GetKey(KeyCode.D))
            {
                turnRight();
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    addChildrenLocalRotation(Quaternion.Euler(0, 45, 0));
                    driftingRight = true;
                    driftTimer = 0;
                    audio[2].Play();
                }
            }
            moveForward();
            if (Input.GetKey(KeyCode.Space))
            {
                throwTrash();
            }
        }
        


    }

    void driftLeft()
    {
        moveForward();
        turnLeft();

        if (Input.GetKey(KeyCode.A))
        {
            turnLeft();
        }
        driftTimer += Time.deltaTime;
        if(driftTimer > driftLimit)
        {
            setChildrenLocalRotation(Quaternion.Euler(0, 0, 0));
            driftingLeft = false;
        }
    }

    void driftRight()
    {
        turnRight();
        if (Input.GetKey(KeyCode.D))
        {
            turnRight();
        }
        moveForward();
        driftTimer += Time.deltaTime;
        if (driftTimer > driftLimit)
        {
            setChildrenLocalRotation(Quaternion.Euler(0, 0, 0));
            driftingRight = false;
        }
    }

    void setCamera()
    {
        cam.transform.position = transform.position - (new Vector3(transform.forward.x, -0.25f, transform.forward.z) * camOffset);
        cam.transform.LookAt(transform.position + new Vector3(0, camOffset / 3, 0));
    }

    void easyAI()
    {
        setStats(selectAnimal);
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
        turn *= 1.5f;
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
        turn *= 2f;
        if (getNextTile())
        {
            
            steer(hardSteerTolerance);
            accelerate();
            moveForward();
            //turnTowards(driveTarget);
        }
        if (trashcollect && itemHeld == "bus") throwTrash();
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
        transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
        driftingLeft = false;
        driftingRight = false;
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
                driveTarget = getNextTile().getNearestMilk(transform.position);
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
            ///
            nextTileDir = (collision.gameObject.GetComponent<TileObject>().nextTile.transform.position - collision.gameObject.transform.position);

            if (bus)
            {
                bus.GetComponent<BusScript>().setTarget(nextTileDir);
            }
            if (behavior == 0)
            {
                directionIndicator.setTarget(nextTileDir);
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
                if(behavior != 0)
                {
                    checkpoint = collision.gameObject.transform;
                    getNextTarget();
                    if (collision.gameObject.GetComponent<TileObject>().tileIndex == 2) lap++;
                }
                else
                {
                    checkpoint = collision.gameObject.transform;
                    getNextTarget();
                    if(lap == GlobalVariables.numLaps + 1)
                    {
                        finish();
                    }
                    else
                    {
                        if (collision.gameObject.GetComponent<TileObject>().tileIndex == 1) lap++;
                    }
                    
                }
                
            }
        }else if(collision.gameObject.tag == "Wall"||collision.gameObject.tag == "Player")
        {
               if(particles)
                particles.Play();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Wall"||collision.gameObject.tag == "Player"){
            if(particles){
                if(particles.isPlaying)
                    particles.Stop();   
            }
            
        }
        
    }
    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(4f);
        started = true;
        cam.GetComponent<AudioSource>().Play();
    }

    public void throwTrash()
    {
        if (trashcollect)
        {
            if(itemHeld == "trash")
            {
                GameObject temp = Instantiate(trashbag);
                temp.transform.position = transform.position + (transform.forward * 0.5f);
                temp.transform.position += new Vector3(0, 0.05f, 0);
                temp.transform.rotation = transform.rotation;
                
            }else if(itemHeld == "bus" && !bus)
            {
                bus = Instantiate<GameObject>(busPrefab, transform);
            }
            trashcollect = false;
        }
        
    }

    public void setSpeedup()
    {
        this.isSpeedup =true;
        this.startSpeedup = 0;

         if(backlight)
                backlight.Play();
    }
}
