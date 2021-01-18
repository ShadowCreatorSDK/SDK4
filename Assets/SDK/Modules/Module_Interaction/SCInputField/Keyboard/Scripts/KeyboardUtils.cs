using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardUtils
{
    public static int CaculateTextLength(string message, Text tex)
    {
        int totalLength = 0;
        Font myFont = tex.font;  //chatText is my Text component
        myFont.RequestCharactersInTexture(message, tex.fontSize, tex.fontStyle);
        CharacterInfo characterInfo = new CharacterInfo();

        char[] arr = message.ToCharArray();

        foreach (char c in arr)
        {
            myFont.GetCharacterInfo(c, out characterInfo, tex.fontSize);

            totalLength += characterInfo.advance;
        }

        return totalLength;
    }
}
