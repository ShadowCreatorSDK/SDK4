using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTween : TweenBase
{
    public Vector3 StartEuler;
    public Vector3 EndEuler;

    protected override void Awake()
    {
        base.Awake();
        StartEuler = StartPoint.eulerAngles;
        EndEuler = EndPoint.eulerAngles;
    }

    public override void Init()
    {
        this.transform.eulerAngles = StartEuler;
        base.Init();
    }

    public override void StartAction()
    {
        if (!this.enabled)
        {
            return;
        }
       // CDebug.Log("rotate StartAction name = " + obj.name);
        LeanTween.rotateLocal(obj, EndEuler, duration).setEase(mLeanTweenType);
    }
}
