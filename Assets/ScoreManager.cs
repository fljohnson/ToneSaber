using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static int score = 0;
    private static int counter = 0;
    private static void changeMusic()
    {
        if (counter <= 4)
        {
            //switch trigger
        }
        else if (counter > 4 && counter <= 8)
        {
            //switch trigger 2
        }
    }
    public static void increaseScore(int amount)
    {
        score += amount;
        counter++;
        changeMusic();
    }
    public static int getScore()
    {
        return score;
    }
}
