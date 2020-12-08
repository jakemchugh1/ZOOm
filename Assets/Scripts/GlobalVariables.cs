using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalVariables 
{
    public static string selectedFile = "Track4";
    public static Animal selectedAnimal = Animal.Bear;
    public static CarColor selectedCar = CarColor.Red;
    public static int aiDifficulty = 1;
    public static bool finished = false;
    public static bool paused;
    public static float volume = 0.5f;
    public static bool muted = false;

    public static int numLaps = 1;

    public static string trackLink = "https://jakemchugh1.github.io/ZOOm-firstDemo/Track";
    public static string track1Link = "https://jakemchugh1.github.io/ZOOm-firstDemo/Track1";
    public static string track2Link = "https://jakemchugh1.github.io/ZOOm-firstDemo/Track2";
    public static string track3Link = "https://jakemchugh1.github.io/ZOOm-firstDemo/Track3";
}
