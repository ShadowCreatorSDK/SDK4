using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlphaTween : TweenBase
{
    private float[] endAlphas;
    private Renderer[] renderers;
    protected override void Awake()
    {
        base.Awake();
        renderers = this.GetComponentsInChildren<Renderer>();
        endAlphas = new float[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            endAlphas[i] = renderers[i].material.color.a;
        }
    }

    public override void Init()
    {
        base.Init();

        for (int i = 0; i < renderers.Length; i++)
        {
            LeanTween.alpha(renderers[i].gameObject, 0, 0);
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
            LeanTween.alpha(renderers[i].gameObject, endAlphas[i], duration).setEase(mLeanTweenType);
        }
    }
}