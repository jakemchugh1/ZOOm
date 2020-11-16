using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreKeeperScript : MonoBehaviour
{
    public RacerBehaviorScript[] drivers;

    public TextMeshProUGUI lapText;
    // Start is called before the first frame update
    void Start()
    {
        drivers = FindObjectsOfType<RacerBehaviorScript>();
    }

    // Update is called once per frame
    void Update()
    {
        lapText.text = ("Lap: "+drivers[0].lap);
    }
}
