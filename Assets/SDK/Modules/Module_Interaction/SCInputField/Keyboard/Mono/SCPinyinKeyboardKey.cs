using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SCPinyinKeyboardKey : SCKeyboardKey
{
    protected override void OnNormalKeyClick(BaseEventData eventData)
    {
        TextMesh textMesh = this.GetComponentInChildren<TextMesh>();
        GameKey3Dboard gameKey3Dboard = this.GetComponentInParent<GameKey3Dboard>();
        gameKey3Dboard.OnPinyinKeyClick(textMesh.text);
    }
}
