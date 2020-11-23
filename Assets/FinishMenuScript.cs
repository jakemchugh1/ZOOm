using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishMenuScript : MonoBehaviour
{
    public Button b1;
    public Button b2;
    public Button b3;

    public RectTransform first;
    public RectTransform second;
    public RectTransform third;
    public RectTransform fourth;

    bool started;
    bool done;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        started = false;
        done = false;
        b1.gameObject.SetActive(false);
        b2.gameObject.SetActive(false);
        b3.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (done)
        {
            b1.gameObject.SetActive(true);
            b2.gameObject.SetActive(true);
            b3.gameObject.SetActive(true);
        }
        else
        {
            if (gameObject.activeInHierarchy && !started)
            {
                Vector2 center = new Vector2(0.5f, 0.5f);
                first.anchorMin = center;
                first.anchorMax = center;
                second.anchorMin = center;
                second.anchorMax = center;
                third.anchorMin = center;
                third.anchorMax = center;
                fourth.anchorMin = center;
                fourth.anchorMax = center;
                started = true;
                first.anchoredPosition = new Vector2(0, 100);
                second.anchoredPosition = new Vector2(0, 50);
                third.anchoredPosition = new Vector2(0, 0);
                fourth.anchoredPosition = new Vector2(0, -50);
            }
            else if (started && !done)
            {
                timer += Time.deltaTime;

                if (timer > 2) done = true;
        }
        }
        
    }

}
