using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenBase : MonoBehaviour
{
    public LeanTweenType mLeanTweenType = LeanTweenType.animationCurve;
    [HideInInspector]
    public GameObject obj;
    public Transform StartPoint;
    public Transform EndPoint;
    public Color StartColor = Color.white;
    public Color EndColor = Color.white;
    public float duration = 0.5f;
    public float delaytime;

    public virtual void Init()
    {

    }

    public virtual void StartAction()
    {

    }

    protected virtual void Awake()
    {
        obj = gameObject;
        if(StartPoint == null)
        {
            StartPoint = transform;
        }
        if (EndPoint == null)
        {
            EndPoint = transform;
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
