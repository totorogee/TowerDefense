using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static int DirectionToDegree(int dir)
    {
        switch (dir)
        {
            case 1:
                return 180;
            case 2:
                return 270;
            case 4:
                return 90;
            case 8:
                return 0;

            default:
                Debug.LogError("Wrong Case for direction");
                return 0;
        }
    }




}
