using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SC.XR.Unity;

public class Gazeloading : SCModuleMono
{
    public float timer = 3;

    private Animator autoClickLoading;
    public Animator AutoClickLoading
    {
        get
        {
            if (null == autoClickLoading)
            {
                autoClickLoading = GetComponent<Animator>();
            }
            return autoClickLoading;
        }
    }
    private MeshRenderer _meshRenderer;

    public MeshRenderer _MeshRenderer
    {
        get
        {
            if (null == _meshRenderer)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }
            return _meshRenderer;
        }
    }

    float autoClickLoadingClipLength = 0;

    public override void OnSCAwake()
    {
        base.OnSCAwake();
        AutoClickLoading?.gameObject.SetActive(false);

        AutoClickLoading.speed = 0;
        AnimationClip[] clips = AutoClickLoading?.runtimeAnimatorController.animationClips;
        if (clips.Length > 0)
        {
            autoClickLoadingClipLength = clips[0].length;
        }
      
    }

    public override void OnSCStart()
    {
        base.OnSCStart();
        _MeshRenderer.enabled = false;
        AutoClickAnimationStart(timer);
      
    }


    public void AutoClickAnimationStart(float timer)
    {
        AutoClickLoading.speed = autoClickLoadingClipLength / timer;
    }
 
}
