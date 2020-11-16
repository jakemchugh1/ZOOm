﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Animal
{
    Bear, Monkey, Penguin, Rabbit
}    
public class WelcomeScript : MonoBehaviour
{
    public Button startBtn;
    public Toggle toggleBear, toggleMonkey, togglePenguin, toggleRabbit;
    public Toggle toggleTrack1, toggleTrack2 , toggleTrack3, toggleTrackCustom;

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
    }

    // Update is called once per frame
    void Update()
    {
        
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
       Toggle tc = toggleTrackCustom.GetComponent<Toggle>();

       if(tc.isOn){
       GlobalVariables.selectedFile = "Track";
       
       SceneManager.LoadScene(2);
       }else{
       if(t1.isOn)
       GlobalVariables.selectedFile = "Track1";
       else if(t2.isOn)
       GlobalVariables.selectedFile = "Track2";
       else if(t3.isOn)
       GlobalVariables.selectedFile = "Track3";

       SceneManager.LoadScene(1);
       }



    }

    void ToggleValueChanged(Toggle t){
        if(t.isOn){
            GlobalVariables.selectedFile = "Track";
        }
    }
}
