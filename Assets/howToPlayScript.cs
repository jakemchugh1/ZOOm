using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class howToPlayScript : MonoBehaviour
{
    public Sprite first;
    public Sprite second;

    bool pressed;

    Image mainImage;
    // Start is called before the first frame update
    void Start()
    {
        mainImage = GetComponent<Image>();
        mainImage.sprite = first;
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!pressed)
            {
                pressed = true;
                mainImage.sprite = second;
            }
            else
            {
                SceneManager.LoadScene(2);
            }
        }
    }

}
