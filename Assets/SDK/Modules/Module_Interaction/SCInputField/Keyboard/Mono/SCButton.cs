using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using SC;

[RequireComponent(typeof(BoxCollider))]
[AddComponentMenu("SDK/SCButton")]
public class SCButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler {

    public enum Transition {
        None,
        Scale,
        Position,
    }

    public enum ScaleType {
        Null,
        Z,
        XYZ,
    }


    [Header("Animation Settings")]
    public ScaleType scaleType = ScaleType.Null;
    public float duration = 0.3f;
    public float scaleRatio = 0.2f;

    public bool PositionAnimation = false;
    public float forwardRatio = 0.05f;

    [Header("Events Settings")]
    public UnityEvent onClick = new UnityEvent();
    public UnityEvent onEnter = new UnityEvent();
    public UnityEvent onDown = new UnityEvent();
    public UnityEvent onUp = new UnityEvent();
    public UnityEvent onExit = new UnityEvent();

    public Vector3 initScale;
    public Vector3 initPosition;


    /// <summary>
    /// 子集中需要触发的效果
    /// </summary>
    private PointEffectBase[] _effectComponents;
    public PointEffectBase[] effectComponents {
        get {
            if(_effectComponents == null) {
                _effectComponents = GetComponentsInChildren<PointEffectBase>();
            }
            return _effectComponents;
        }
    }

    void Awake() {
        initScale = transform.localScale;
        initPosition = transform.localPosition;
    }

    public virtual void Start() { }
    public virtual void Update() { }



    public virtual void OnDestroy() {
        if(onClick != null) {
            onClick.RemoveAllListeners();
        }
    }





    public virtual void OnPointerEnter( PointerEventData eventData ) {

        foreach(var item in effectComponents) {
            if(item.gameObject != gameObject)
                item.OnPointerEnter(eventData);
        }

        if(onEnter != null) {
            onEnter.Invoke();
        }
        OnEnterAnimation();
    }
    public virtual void OnPointerDown( PointerEventData eventData ) {
        foreach(var item in effectComponents) {
            if(item.gameObject != gameObject)
                item.OnPointerDown(eventData);
        }
        if(onDown != null) {
            onDown.Invoke();
        }

    }

    public virtual void OnPointerUp( PointerEventData eventData ) {
        foreach(var item in effectComponents) {
            if(item.gameObject != gameObject)
                item.OnPointerUp(eventData);
        }
        if(onUp != null) {
            onUp.Invoke();
        }
    }

    public virtual void OnPointerClick( PointerEventData eventData ) {
        foreach(var item in effectComponents) {
            if(item.gameObject != gameObject)
                item.OnPointerClick(eventData);
        }


        if(onClick != null) {
            onClick.Invoke();
        }
        OnClickAnimation();

    }

    public virtual void OnDrag( PointerEventData eventData ) {
        foreach(var item in effectComponents) {
            if(item.gameObject != gameObject)
                item.OnDrag(eventData);
        }
    }



    public virtual void OnPointerExit( PointerEventData eventData ) {
        foreach(var item in effectComponents) {
            if(item.gameObject != gameObject)
                item.OnPointerExit(eventData);
        }

        if(onExit != null) {
            onExit.Invoke();
        }

        OnExitAnimation();

    }

    public virtual void OnEnterAnimation() {
        //if(scaleType == ScaleType.Z) {
        //    transform.DOLocalMoveZ(initScale.z, 0.5f).SetEase(Ease.OutQuart).SetAutoKill(true);
        //} else if(scaleType == ScaleType.XYZ) {
        //    transform.DOScale(initScale * scalRatio, transitionTime).SetEase(Ease.InOutExpo).SetAutoKill(true);
        //}

        if(PositionAnimation) {
            transform.DOLocalMove(initPosition + new Vector3(0, 0, forwardRatio * -1), duration).SetEase(Ease.InOutExpo).SetAutoKill(true);
        }
    }
    public virtual void OnClickAnimation() {
        if(scaleType == ScaleType.Z) {
            transform.DOScaleZ(initScale.z * scaleRatio, duration / 2).SetEase(Ease.InOutExpo).SetId("OnClickAnimation").OnComplete(ClickFinish).SetAutoKill(true);

        } else if(scaleType == ScaleType.XYZ) {
            transform.DOScale(initScale * scaleRatio, duration).SetEase(Ease.InOutExpo).SetAutoKill(true);
        }
    }
    public virtual void ClickFinish() {
        foreach(var item in effectComponents) {
            if(item.gameObject != gameObject)
                item.ClickFinish();
        }
        transform.DOScaleZ(initScale.z, duration).SetEase(Ease.InOutExpo);
    }

    public virtual void OnExitAnimation() {
        DOTween.Kill("OnClickAnimation");
        if(scaleType == ScaleType.Z) {
            transform.DOScaleZ(initScale.z, duration).SetEase(Ease.InOutExpo).SetAutoKill(true);

        } else if(scaleType == ScaleType.XYZ) {
            transform.DOScale(initScale , duration).SetEase(Ease.InOutExpo).SetAutoKill(true);
        }

        if(PositionAnimation) {
            transform.DOLocalMove(initPosition, duration).SetEase(Ease.InOutExpo).SetAutoKill(true);
        }

    }

}