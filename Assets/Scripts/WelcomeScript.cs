using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI.Extensions;

public enum Animal
{
    Bear, Monkey, Penguin, Rabbit
}  
public enum CarColor
{
    Red, Green, Blue, Yellow
}    
public class WelcomeScript : MonoBehaviour
{
    public Button startBtn;
    public ScrollSnap hss;
    public ScrollSnap carhss;
    public Text animalLbl;
    public Text carLbl;
    public Slider volumeSlider;

    public Toggle toggleBear, toggleMonkey, togglePenguin, toggleRabbit;
    public Toggle toggleTrack1, toggleTrack2 , toggleTrack3, toggleTrackCustom, toggleTrack4;
    public Toggle  volumToggle;
    public Camera mainCam;

    public Button lapsPlus;
    public Button lapsMinus;
    public TMPro.TextMeshProUGUI lapsText;
    public Camera smallCam;
    public Button preBtn, nextBtn, preCar, nextCar;
    private int indexAnimal, indexCar;
    public Camera smallCamCar;

    

    // Start is called before the first frame update
    void Start()
    {
        volumeSlider = FindObjectOfType<Slider>();
        Button btn = startBtn.GetComponent<Button>();
		btn.onClick.AddListener(startedClick);
        Toggle t = volumToggle;
        t.onValueChanged.AddListener(delegate {
            ToggleVolume(t);
        });
        volumeSlider.value = GlobalVariables.volume;
        volumeSlider.onValueChanged.AddListener(delegate { onValueChanged(); });
        lapsPlus.onClick.AddListener(delegate { changeLaps(1); });
        lapsMinus.onClick.AddListener(delegate { changeLaps(-1); });
        Button btnp = preBtn.GetComponent<Button>();
		btnp.onClick.AddListener(preClick); 
        Button btnn = nextBtn.GetComponent<Button>();
		btnn.onClick.AddListener(nextClick);
        indexAnimal = 0;
        Button btnp1 = preCar.GetComponent<Button>();
		btnp1.onClick.AddListener(preCClick); 
        Button btnn1 = nextCar.GetComponent<Button>();
		btnn1.onClick.AddListener(nextCClick);
        indexAnimal = 0;
        indexAnimal = 0;
    }

    public void changeLaps(int change)
    {
        GlobalVariables.numLaps += change;
        if (GlobalVariables.numLaps < 1) GlobalVariables.numLaps = 1;
        else if (GlobalVariables.numLaps > 10) GlobalVariables.numLaps = 10;
    }

    void onValueChanged()
    {
        mainCam.GetComponent<AudioSource>().volume = volumeSlider.value;
        GlobalVariables.volume = volumeSlider.value;
    }

    // Update is called once per frame
    void Update()
    {
        lapsText.text = "Number of Laps: " + GlobalVariables.numLaps;
        ScrollSnap s = hss.GetComponent<ScrollSnap>();
        //Debug.Log(s.CurrentPage());
        switch (indexAnimal)
        {
            case(0):
            animalLbl.text = "Bear";
            GlobalVariables.selectedAnimal = Animal.Bear;
            break;
            case(1):
            animalLbl.text = "Monkey";
            GlobalVariables.selectedAnimal = Animal.Monkey;
            break; 
            case(2):
            animalLbl.text = "Penguin";
            GlobalVariables.selectedAnimal = Animal.Penguin;
            break; 
            case(3):
            animalLbl.text = "Rabbit";
            GlobalVariables.selectedAnimal = Animal.Rabbit;
            break;
            default:
            break;
        }
        ScrollSnap c = carhss.GetComponent<ScrollSnap>();
        switch (indexCar)
        {
            case(0):
            carLbl.text = "Red";
            carLbl.color =  Color.red;
            GlobalVariables.selectedCar = CarColor.Red;
            break;
            case(1):
            carLbl.text = "Green";
            carLbl.color =  Color.green;
            GlobalVariables.selectedCar = CarColor.Green;
            break; 
            case(2):
            carLbl.text = "Blue";
            carLbl.color =  Color.blue;
            GlobalVariables.selectedCar = CarColor.Blue;
            break; 
            case(3):
            carLbl.text = "Yellow";
            carLbl.color =  Color.yellow;
            GlobalVariables.selectedCar = CarColor.Yellow;
            break;
            default:
            break;
        }

    }

    void ToggleVolume(Toggle t)
    {
        mainCam.GetComponent<AudioSource>().mute = t.isOn;
        GlobalVariables.muted=t.isOn;

    }
    void startedClick()
    {
       // Select Animal
       Toggle a1 = toggleBear.GetComponent<Toggle>();
       Toggle a2 = toggleMonkey.GetComponent<Toggle>();
       Toggle a3 = togglePenguin.GetComponent<Toggle>();
       Toggle a4 = toggleRabbit.GetComponent<Toggle>(); 
       if(a1.isOn)
            GlobalVariables.selectedAnimal = Animal.Bear;
        else if(a2.isOn)
            GlobalVariables.selectedAnimal = Animal.Monkey;
        else if(a3.isOn)
            GlobalVariables.selectedAnimal = Animal.Penguin;
        else if(a4.isOn)
            GlobalVariables.selectedAnimal = Animal.Rabbit;

       // Select Track
       Toggle t1 = toggleTrack1.GetComponent<Toggle>();
       Toggle t2 = toggleTrack2.GetComponent<Toggle>();
       Toggle t3 = toggleTrack3.GetComponent<Toggle>();

       
       if(t1.isOn)
       GlobalVariables.selectedFile = "Track1";
       else if(t2.isOn)
       GlobalVariables.selectedFile = "Track2";
       else if(t3.isOn)
       GlobalVariables.selectedFile = "Track3";
        else if(t3.isOn)
       GlobalVariables.selectedFile = "Track4";
       SceneManager.LoadScene(3);
       



    }

    public void quit()
    {
        Application.Quit();
    }

    void ToggleValueChanged(Toggle t){
        if(t.isOn){
            GlobalVariables.selectedFile = "Track";
        }
    }
     void preClick(){
         Vector3 p = smallCam.transform.localPosition;
        p.x-=3;
         if(p.x<-100)
         p.x=-91;
         else
         if(p.x>-91)
         p.x=-100;
         smallCam.transform.localPosition = p;
         indexAnimal = (int)(p.x+100)/3;

     }
     void nextClick(){
Vector3 p = smallCam.transform.localPosition;
         p.x+=3;
          if(p.x<-100)
         p.x=-91;
         else
         if(p.x>-91)
         p.x=-100;
         smallCam.transform.localPosition = p;
                  indexAnimal = (int)(p.x+100)/3;

     }
     void preCClick(){
         Vector3 p = smallCamCar.transform.localPosition;
        p.x-=5;
         if(p.x<-100)
         p.x=-85.6f;
         else
         if(p.x>-85)
         p.x=-100.6f;
         smallCamCar.transform.localPosition = p;
         indexCar = (int)(p.x+100+0.6)/5;

     }
     void nextCClick(){
Vector3 p = smallCamCar.transform.localPosition;
         p.x+=5;
          if(p.x<-100)
         p.x=-85.6f;
         else
         if(p.x>-85)
         p.x=-100.6f;
         smallCamCar.transform.localPosition = p;
                  indexCar = (int)(p.x+100+0.6)/5;

     }
}
