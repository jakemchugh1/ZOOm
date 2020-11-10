using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WelcomeScript : MonoBehaviour
{
    public Button startBtn;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = startBtn.GetComponent<Button>();
		btn.onClick.AddListener(startedClick);   
         }

    // Update is called once per frame
    void Update()
    {
        
    }

    void startedClick()
    {
       SceneManager.LoadScene(1);

    }
}
