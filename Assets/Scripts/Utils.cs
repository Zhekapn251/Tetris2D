using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static int Wrap(int input, int min, int max) =>
        input < min 
            ? max - (min - input) % (max - min)
            : min + (input - min) % (max - min);   
}
