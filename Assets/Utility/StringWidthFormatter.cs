using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringWidthFormatter : MonoBehaviour
{
    public const int StandardWidth = 38; 

    public static string FormatStringToWidth(string input, int width)
    {
        string result = "";

        bool searching = false;

        for (int i = 0; i < input.Length; i++)
        {
            result += input[i];
            if (i % width == 0 && i != 0) //so, if we've reached a multiple of 38, then we start looking to insert a new line 
            {
                searching = true;
            }

            if (searching)
            {
                if (input[i] == ' ')
                {
                    result += '\n';
                    searching = false;
                }
            }
        }

        return result; 
    }
}
