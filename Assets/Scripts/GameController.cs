using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Button exitBtn;
    // Start is called before the first frame update
    void Start()
    {
         Button btn = exitBtn.GetComponent<Button>();
		btn.onClick.AddListener(exitClick);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void exitClick(){
       SceneManager.LoadScene(0);

    }
}
