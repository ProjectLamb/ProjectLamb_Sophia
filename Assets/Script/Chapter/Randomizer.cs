using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randomizer : MonoBehaviour
{
    public static bool GetRandomBool()
    {
        System.Random random = new System.Random();
        int randomBool = random.Next(0, 2);
        if (randomBool == 1)
            return true;
        else
            return false;
    }

    public static bool GetThisChanceResult(float Chance)
    {
        System.Random random = new System.Random();
        if (Chance < 0.0000001f)
        {
            Chance = 0.0000001f;
        }

        bool Success = false;
        int RandAccuracy = 10000000;
        float RandHitRange = Chance * RandAccuracy;
        int Rand = random.Next(1, RandAccuracy+1);
        if (Rand <= RandHitRange)
        {
            Success = true;
        }
        return Success;
    }

    public static bool GetThisChanceResult_Percentage(float Percentage_Chance)
    {
        System.Random random = new System.Random();
        if (Percentage_Chance < 0.0000001f)
        {
            Percentage_Chance = 0.0000001f;
        }

        Percentage_Chance = Percentage_Chance / 100;

        bool Success = false;
        int RandAccuracy = 10000000;
        float RandHitRange = Percentage_Chance * RandAccuracy;
        int Rand = random.Next(1, RandAccuracy+1);
        if (Rand <= RandHitRange)
        {
            Success = true;
        }
        return Success;
    }
}
