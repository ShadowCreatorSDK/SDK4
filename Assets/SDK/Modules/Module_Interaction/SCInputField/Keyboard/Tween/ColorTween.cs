using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorTween : TweenBase
{
    public override void Init()
    {
        LeanTween.color(obj, StartColor, 0);
        base.Init();
    }

    public override void StartAction()
    {
        if(!this.enabled)
        {
            return;
        }
        Debug.Log("Color StartAction name = " + obj.name);
        LeanTween.color(obj, EndColor, duration).setEase(mLeanTweenType);
    }
}