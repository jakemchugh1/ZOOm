using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class howToPlayScript : MonoBehaviour
{
    public List<Sprite> list;

    public int spriteIndex;

    Image mainImage;
    // Start is called before the first frame update
    void Start()
    {
        mainImage = GetComponent<Image>();
        spriteIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spriteIndex++;
            if(spriteIndex >= list.Count)
            {
                SceneManager.LoadScene(2);
            }
            else
            {
                mainImage.sprite = list[spriteIndex];
            }
        }
    }

}
