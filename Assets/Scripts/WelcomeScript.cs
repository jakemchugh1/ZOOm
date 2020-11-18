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

    public Toggle toggleBear, toggleMonkey, togglePenguin, toggleRabbit;
    public Toggle toggleTrack1, toggleTrack2 , toggleTrack3, toggleTrackCustom;
    public Dropdown difficulty;

    

    // Start is called before the first frame update
    void Start()
    {
        Button btn = startBtn.GetComponent<Button>();
		btn.onClick.AddListener(startedClick);   

        Toggle m_Toggle = toggleTrackCustom.GetComponent<Toggle>();
        //Add listener for when the state of the Toggle changes, to take action
        m_Toggle.onValueChanged.AddListener(delegate {
            ToggleValueChanged(m_Toggle);
        });
        difficulty = FindObjectOfType<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        ScrollSnap s = hss.GetComponent<ScrollSnap>();
        //Debug.Log(s.CurrentPage());
        switch (s.CurrentPage())
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
        switch (c.CurrentPage())
        {
            case(0):
            carLbl.text = "Red";
            GlobalVariables.selectedCar = CarColor.Red;
            break;
            case(1):
            carLbl.text = "Green";
            GlobalVariables.selectedCar = CarColor.Green;
            break; 
            case(2):
            carLbl.text = "Blue";
            GlobalVariables.selectedCar = CarColor.Blue;
            break; 
            case(3):
            carLbl.text = "Yellow";
            GlobalVariables.selectedCar = CarColor.Yellow;
            break;
            default:
            break;
        }

    }

    void startedClick()
    {
       GlobalVariables.aiDifficulty = difficulty.value + 1;
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
       Toggle tc = toggleTrackCustom.GetComponent<Toggle>();

       if(tc.isOn){
       GlobalVariables.selectedFile = "https://jakemchugh1.github.io/ZOOm-firstDemo/Track";
       
       SceneManager.LoadScene(1);
       }else{
       if(t1.isOn)
       GlobalVariables.selectedFile = "https://jakemchugh1.github.io/ZOOm-firstDemo/Track1";
       else if(t2.isOn)
       GlobalVariables.selectedFile = "https://jakemchugh1.github.io/ZOOm-firstDemo/Track2";
       else if(t3.isOn)
       GlobalVariables.selectedFile = "https://jakemchugh1.github.io/ZOOm-firstDemo/Track3";

       SceneManager.LoadScene(2);
       }



    }

    void ToggleValueChanged(Toggle t){
        if(t.isOn){
            GlobalVariables.selectedFile = "Resources/Track";
        }
    }
}
