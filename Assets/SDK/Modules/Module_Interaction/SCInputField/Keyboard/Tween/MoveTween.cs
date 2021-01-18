using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTween : TweenBase
{
    private Vector3 StartPos;
    private Vector3 EndPos;

    protected override void Awake()
    {
        base.Awake();
        StartPos = StartPoint.position;
        EndPos = EndPoint.position;
    } 

    public override void Init()
    {
        transform.position = StartPos;
        base.Init();
    }

    public override void StartAction()
    {
        if (!this.enabled)
        {
            return;
        }
        //CDebug.Log("move StartAction name = " + obj.name);
        LeanTween.move(obj, EndPos, duration).setEase(mLeanTweenType);
    }
}
