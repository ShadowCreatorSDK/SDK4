using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
using UnityEngine.EventSystems;
using DG.Tweening;

/// <summary>
/// 3D的 按钮
/// </summary>
public class Game3DButton : SCButton
{  
    [SerializeField] 
    private Ease mTweenType = Ease.OutCubic;//缓动的参数
 //   [SerializeField]
  //  private float duration = 0.2f;//缓动的持续时间
    [SerializeField]
    private float wScale = 1.03f;//宽缩放倍数
    [SerializeField]
    private float hScale = 1.05f;//高缩放倍数

    private AudioSource _audioSource;
    private Quaternion initRotation;//初始的角度
    private float transitionTime = 0.1f;//按键反应时间
    private float forwardNum = 0.008f;//按键的键程
    [SerializeField]
    private Renderer mRenderer;//材质球 点击变色 可以没有
    private Color DefaultColor;//记录按钮的初始颜色
    private Color ClickColor;//点击的变得颜色
    private Color FocusColor;//暂时没有用
                             //  public UnityEvent onClick;//点击事件

    public void Awake()
    {
        initPosition = transform.localPosition;
        initRotation = transform.localRotation;
        initScale = transform.localScale;
       // init();
    }
    public override void Start()
    {
        _audioSource = (AudioSource)gameObject.GetComponent<AudioSource>();
        if (_audioSource != null)
        {
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
        }
    }

    private void OnEnable()
    {
       // transform.DOPause();
        transform.localPosition = initPosition;
        transform.localRotation = initRotation;
      //  ChangeColor(DefaultColor);
    }

    protected virtual  void ChangeColor(Color co)
    {
        if (mRenderer != null)
        {
           // mRenderer.material.SetColor("_Color", co);
        }
    }

    protected virtual void ChangeMetallic(float value)
    {
        if (mRenderer != null)
        {
           // mRenderer.material.SetFloat("_Metallic", value);
        }
    }

    protected virtual void init()
    {
        if (mRenderer != null)
        {
          //  DefaultColor = mRenderer.material.GetColor("_Color");
        }
       // ClickColor = new Color(0.9f, 0.9f, 0.9f);
    }

    public override void OnPointerDown(PointerEventData data)
    {
        Debug.Log(GetType().Name + "OnPointerDown");
       // ChangeColor(ClickColor);
    }

    public override void OnPointerUp(PointerEventData data)
    {
        Debug.Log(GetType().Name + "OnPointerUp");
    }

    public override void OnPointerEnter(PointerEventData data)
    {
        OnEnterAnimation();
        ChangeMetallic(0.65f);
    }

    public override void OnPointerExit(PointerEventData data)
    {
        OnExitAnimation();
        ChangeMetallic(0);
    }

    public override void OnPointerClick(PointerEventData data)
    {
        Debug.Log(GetType().Name + "OnPointerClick");
        OnClickAnimation();
    }

    private void ClickFun()
    {
        ChangeColor(DefaultColor);
        if (onClick != null)
        {
            onClick.Invoke();
        }
    }

    public override void OnClickAnimation()
    {
        transform.DOLocalMove(initPosition + new Vector3(0, 0, forwardNum * 1), transitionTime).SetEase(Ease.InOutExpo).OnComplete(ClickFinish).SetAutoKill(true);
        //transform.DOScaleZ(initScale.z * tweenScale, duration / 2).SetEase(Ease.InOutExpo).SetId("OnClickAnimation").OnComplete(ClickFinish).SetAutoKill(true);
    }

    public override void ClickFinish()
    {
        //transform.DOScaleZ(initScale.z, duration).SetEase(mLeanTweenType);
        transform.DOLocalMove(initPosition + new Vector3(0, 0, forwardNum * -1), transitionTime).SetEase(Ease.InOutExpo).OnComplete(ClickFun).SetAutoKill(true);
    }

    public override void OnEnterAnimation()
    {
        transform.DOScale(new Vector3(initScale.x * wScale,initScale.y * hScale, initScale.z), duration).SetEase(mTweenType);
    }

    public override void OnExitAnimation()
    {
        transform.DOScale(initScale, duration).SetEase(mTweenType);
    }
}
