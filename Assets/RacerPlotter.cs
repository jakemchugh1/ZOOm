﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RacerPlotter : MonoBehaviour
{
    Camera cam;
    RacerBehaviorScript[] racers;
    public GameObject plot;
    RectTransform[] plots;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        racers = FindObjectsOfType<RacerBehaviorScript>();
        plots = new RectTransform[racers.Length];
        for(int i = 0; i < racers.Length; i++)
        {
            plots[i] = Instantiate<GameObject>(plot, FindObjectOfType<Canvas>().transform).GetComponent<RectTransform>();
            plots[i].gameObject.GetComponentInChildren<Text>().text = "P" + (i + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < racers.Length; i++)
        {
            Vector3 screenPos = cam.WorldToScreenPoint(racers[i].transform.position);
            plots[i].position = screenPos;
        }
        
    }
}