﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInstance : MonoBehaviour
{
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        audio.volume = audio.volume * GlobalVariables.volume;
        audio.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audio.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
