using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IslandNamer : MonoBehaviour
{
    private const string dataFile = "charData.txt";
    private const string startDataFile = "startCharData.txt";
    private static int[] data;
    private static int[] startData;

    public static string NextIslandName = "";

    private static int[] LoadData(string path)
    {
        string[] lines = File.ReadAllLines(path);

        int[] result = new int[lines.Length];

        for (int i = 0; i < lines.Length; i++)
        {
            result[i] = int.Parse(lines[i]); //this is kinda inefficent 
        }

        return result;
    }

    public static void SetNextRandomIslandName() //we can do this here but it's better, actually to do it in the progression menu, because then there's no delay. 
    {
        int length = Random.Range(4, 10);
        
        if (data == null || data.Length == 0)
        {
            data = LoadData(MainMenuScript.appendPath + dataFile);
            startData = LoadData(MainMenuScript.appendPath + startDataFile);
        }

        string result = "" + DecodeString(ChooseWeightedNumber(0, 676, startData));

        char last = result[result.Length - 1];

        while (result.Length < length)
        {
            int startRange = 26 * (last - 97); //it can go from startRange to startRange + 26 (exclusive)
            int chosen = ChooseWeightedNumber(startRange, startRange + 26, data);

            char next = (char)((chosen - startRange) + 97);
            result += next;
            last = next; 
        }

        result = (char)((int)result[0] - 32) + result.Substring(1); //capitalize first letter. A lot of magic numbers here.

        NextIslandName = result;
    }

    private static int ChooseWeightedNumber(int start, int end, int[] d)
    {
        int sum = 0;
        for (int i = 0; i < (end - start); i++)
        {
            sum += d[i + start];
        }

        int selection = Random.Range(0, sum);
        int cur = 0;

        for (int i = 0; i < (end - start); i++)
        {
            if (selection >= cur && selection < cur + d[i + start])
            {
                return i + start;
            }
            cur += d[i + start];
        }

        throw new System.Exception("Could not choose weighted number!");
    }

    private static string DecodeString(int index)
    {
        int first = index / 26;
        int second = index - (first * 26);

        return (char)(first + 97) + "" + (char)(second + 97);
    }
}
