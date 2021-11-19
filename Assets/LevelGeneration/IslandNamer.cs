using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IslandNamer : MonoBehaviour
{
    private const string dataFile = "charData.txt";
    private static int[] data;

    public static string NextIslandName = "";

    public static void SetNextRandomIslandName() //we can do this here but it's better, actually to do it in the progression menu, because then there's no delay. 
    {
        int length = Random.Range(4, 8);
        
        if (data == null || data.Length == 0)
        {
            string[] lines = File.ReadAllLines(MainMenuScript.appendPath + dataFile);

            data = new int[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                data[i] = int.Parse(lines[i]); //this is kinda inefficent 
            }
        }

        string result = "" + (char)(Random.Range(0, 26) + 97);
            //DecodeString(Random.Range(0, 676));

        char last = result[result.Length - 1];

        while (result.Length < length)
        {
            int startRange = 26 * (last - 97); //it can go from startRange to startRange + 26 (exclusive)

            int sum = 0;

            for (int i = 0; i < 26; i++)
            {
                sum += data[i + startRange];
            }

            int selection = Random.Range(0, sum);

            int cur = 0;

            for (int i = 0; i < 26; i++)
            {
                if (selection >= cur && selection < cur + data[i + startRange])
                {
                    last = (char)(97 + i);
                    result += last;
                    break;
                }
                cur += data[i + startRange];
            }
        }

        result = (char)((int)result[0] - 32) + result.Substring(1); //capitalize first letter. A lot of magic numbers here.

        NextIslandName = result;
    }

    private static bool IsVowel(int index)
    {
        if (index == 0 || index == 4 || index == 8 || index == 14 || index == 20 || index == 24)
        {
            return true;
        }
        return false;
    }

    private static bool IsVowel(char input)
    {
        int index = (input - 97);
        return IsVowel(index);
    }

    private static string DecodeString(int index)
    {
        int first = index / 26;
        int second = index - (first * 26);

        return (char)(first + 97) + "" + (char)(second + 97);
    }
}
