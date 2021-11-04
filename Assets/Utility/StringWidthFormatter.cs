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

        int charsSinceLine = 0;

        for (int i = 0; i < input.Length; i++)
        {
            result += input[i];
            
            if (input[i] == '\n')
            {
                charsSinceLine = 0;
                searching = false;
            }

            if (charsSinceLine >= width) //so, if we've reached a multiple of the standard width, then we start looking to insert a new line 
            {
                searching = true;
            }

            charsSinceLine++;

            if (searching)
            {
                if (input[i] == ' ')
                {
                    charsSinceLine = 0;
                    result += '\n';
                    searching = false;
                }
            } 
        }

        return result; 
    }
}
