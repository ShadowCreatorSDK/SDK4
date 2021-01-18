using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SC.XR.Unity.Module_InputSystem;
using DG.Tweening;
using UnityEngine.Events;


[RequireComponent(typeof(BoxCollider))]

[AddComponentMenu("SDK/PressableButton")]
public class PressableButton : PointerHandler
{
    private const float duration = 0.1f;

    [SerializeField]
    private List<InteractionPressableEntry> m_Delegates;

    public List<InteractionPressableEntry> Triggers
    {
        get
        {
            if (m_Delegates == null)
                m_Delegates = new List<InteractionPressableEntry>();
            return m_Delegates;
        }
        set { m_Delegates = value; }
    }

    [SerializeField]
    protected SCAudiosConfig.AudioType PressAudio = SCAudiosConfig.AudioType.ButtonPress;
    [SerializeField]
    protected SCAudiosConfig.AudioType ReleaseAudio = SCAudiosConfig.AudioType.ButtonUnpress;

    public Transform VisualMove;

    public Transform VisualScale;

    protected Vector3 MoveObjInitLocalPosition;
    protected Vector3 ScaleObjInitLocalScale;

    private Tween scaleTween;
    private Tween moveTween;

    [SerializeField]
    [Range(0.2f, 0.8f)]
    [Tooltip("The minimum percentage of the original scale the compressableButtonVisuals can be compressed to.")]
    protected float minCompressPercentage = 0.25f;

    private BoxCollider boxCollider;
    protected BoxCollider BoxCollider
    {
        get
        {
            if (boxCollider == null)
            {
                boxCollider = GetComponent<BoxCollider>();
            }
            return boxCollider;
        }
    }

    void Awake()
    {
        if (VisualScale != null)
        {
            ScaleObjInitLocalScale = VisualScale.localScale;
        }

        if (VisualMove != null)
        {
            MoveObjInitLocalPosition = VisualMove.localPosition;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Execute(InteractionPressableType.PointerEnter, eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        SCPointEventData mSCPointEventData = eventData as SCPointEventData;
        Execute(InteractionPressableType.PointerExit, eventData);

        if (VisualScale != null && mSCPointEventData.rawPointerPress == null)
        {
            if (VisualScale.localScale.z < ScaleObjInitLocalScale.z)
            {
                //DOTween.KillAll();
                if (scaleTween != null && scaleTween.IsPlaying())
                {
                    scaleTween.Kill();
                }

                scaleTween = VisualScale.transform.DOScaleZ(ScaleObjInitLocalScale.z, duration);
            }
        }

        if (VisualMove != null && mSCPointEventData.rawPointerPress == null)
        {
            if (moveTween != null && moveTween.IsPlaying())
            {
                moveTween.Kill();
            }

            moveTween = VisualMove.transform.DOLocalMoveZ(MoveObjInitLocalPosition.z, duration);
        }
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        //base.OnPointerDown(eventData);
        SCPointEventData mSCPointEventData = eventData as SCPointEventData;
        Execute(InteractionPressableType.PointerDown, eventData);

        //DOTween.KillAll();

        if (VisualScale != null)
        {
            if (scaleTween != null && scaleTween.IsPlaying())
            {
                scaleTween.Kill();
            }

            if (VisualScale.localScale.z > ScaleObjInitLocalScale.z * minCompressPercentage)
            {
                scaleTween = VisualScale.transform.DOScaleZ(ScaleObjInitLocalScale.z * minCompressPercentage, duration);
            }
        }

        if (VisualMove != null)
        {
            if (moveTween != null && moveTween.IsPlaying())
            {
                moveTween.Kill();
            }

            moveTween = VisualMove.transform.DOLocalMoveZ(MoveObjInitLocalPosition.z + (BoxCollider.size.z / 2 * (1 - minCompressPercentage)), duration);
        }

        AudioSystem.getInstance.PlayAudioOneShot(gameObject, PressAudio);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        Execute(InteractionPressableType.PointerClick, eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        SCPointEventData mSCPointEventData = eventData as SCPointEventData;
        Execute(InteractionPressableType.PointerUp, eventData);

        //DOTween.KillAll();

        if (VisualScale != null)
        {
            if (scaleTween != null && scaleTween.IsPlaying())
            {
                scaleTween.Kill();
            }

            if (VisualScale.localScale.z < ScaleObjInitLocalScale.z)
            {
                scaleTween = VisualScale.transform.DOScaleZ(ScaleObjInitLocalScale.z, duration);
            }
        }

        if (VisualMove != null)
        {
            if (moveTween != null && moveTween.IsPlaying())
            {
                moveTween.Kill();
            }

            moveTween = VisualMove.transform.DOLocalMoveZ(MoveObjInitLocalPosition.z, duration);
        }

        AudioSystem.getInstance.PlayAudioOneShot(gameObject, ReleaseAudio);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Execute(InteractionPressableType.Drag, eventData);
    }

    private void Execute(InteractionPressableType id, BaseEventData eventData)
    {
        for (int i = 0; i < Triggers.Count; i++)
        {
            InteractionPressableEntry entry = Triggers[i];
            if (entry.eventID == id)
            {
                entry.callback?.Invoke(eventData);
            }
        }
    }
}
