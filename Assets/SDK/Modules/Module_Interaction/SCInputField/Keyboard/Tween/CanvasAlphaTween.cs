using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UGUI的Canvas的渐入效果
/// </summary>
public class CanvasAlphaTween : TweenBase
{
    //修改Canvas的Alpha值实现
    private CanvasGroup[] renderers;
    protected override void Awake()
    {
        base.Awake();
        renderers = this.GetComponentsInChildren<CanvasGroup>();
    }

    public override void Init()
    {
        base.Init();
        for (int i = 0; i < renderers.Length; i++)
        {
            //LeanTween.alpha(renderers[i].gameObject, 0, 0);
            LeanTween.alphaCanvas(renderers[i], 0,0);
        }
    }

    public override void StartAction()
    {
        if(!this.enabled)
        {
            return;
        }
        for (int i = 0; i < renderers.Length; i++)
        {
            //LeanTween.alpha(renderers[i].gameObject, endAlphas[i], duration).setEase(mLeanTweenType);
            LeanTween.alphaCanvas(renderers[i], 1, duration).setEase(mLeanTweenType);
        }
    }
}