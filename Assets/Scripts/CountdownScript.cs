using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownScript : MonoBehaviour
{
    public int countdownTime;
    public Text countdownDisplay;
    public AudioSource count;
    public AudioSource go;
    private void Start()
    {
        StartCoroutine(CountdownToStart());
    }

    IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            count.Play();
            yield return new WaitForSeconds(1f);
            countdownTime--;
        }

        go.Play();
        countdownDisplay.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownDisplay.gameObject.SetActive(false);
    }

}
