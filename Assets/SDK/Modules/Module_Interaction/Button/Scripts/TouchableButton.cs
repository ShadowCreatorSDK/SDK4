using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SC.XR.Unity.Module_InputSystem;
using DG.Tweening;
using UnityEngine.Events;
using SC.XR.Unity.Module_InputSystem.InputDeviceHand;


[RequireComponent(typeof(BoxCollider))]

[AddComponentMenu("SDK/TouchableButton")]
public class TouchableButton : PokeHandler {
    [SerializeField]
    private List<InteractionTouchableEntry> m_Delegates;

    public List<InteractionTouchableEntry> Triggers {
        get {
            if(m_Delegates == null)
                m_Delegates = new List<InteractionTouchableEntry>();
            return m_Delegates;
        }
        set { m_Delegates = value; }
    }

    [SerializeField]
    protected SCAudiosConfig.AudioType PressAudio = SCAudiosConfig.AudioType.ButtonPress;
    [SerializeField]
    protected SCAudiosConfig.AudioType ReleaseAudio = SCAudiosConfig.AudioType.ButtonUnpress;

    public Transform VisualMove;
    public bool useCustomMovePosition = false;
    public Vector3 visualMoveStartLocalPosition;
    public Vector3 visualMoveEndLocalPosition;

    public Transform VisualScale;
    public bool useCustomScale = false;
    public Vector3 visualStartLocalScale = Vector3.one;
    public Vector3 visualEndLocalScale = new Vector3(1, 1, 0);

    protected Vector3 MoveObjInitLocalPosition;
    protected Vector3 ScaleObjInitLocalScale;

    [SerializeField]
    [Tooltip("Ensures that the button can only be pushed from the front. Touching the button from the back or side is prevented.")]
    private bool enforceFrontPush = true;

    [SerializeField]
    [Range(0.2f, 0.8f)]
    [Tooltip("The minimum percentage of the original scale the compressableButtonVisuals can be compressed to.")]
    protected float minCompressPercentage = 0.25f;

    NearInteractionTouchable nearInterationTouchable;
    public NearInteractionTouchable NearInterationTouchable {
        get {
            if(nearInterationTouchable == null) {
                nearInterationTouchable = GetComponent<NearInteractionTouchable>();
                if(nearInterationTouchable == null) {
                    nearInterationTouchable = gameObject.AddComponent<NearInteractionTouchable>();
                }
            }
            return nearInterationTouchable;
        }
    }

    protected BoxCollider BoxCollider {
        get {
            return NearInterationTouchable.BoxCollider;
        }
    }

    public Transform Center {
        get {
            return NearInterationTouchable.Center;
        }
    }

    /// <summary>
    /// The press direction of the button as defined by a NearInteractionTouchableSurface.
    /// </summary>
    private Vector3 Normal {
        get {
            return NearInterationTouchable.Normal;
        }
    }

    private float StartPushDistance;
    protected float startPushDistance {
        get {
            if(NearInterationTouchable.NormalType == NormalType.NZ) {
                return StartPushDistance = BoxCollider.size.z * BoxCollider.transform.lossyScale.z / 2.0f * -1;
            } else if(NearInterationTouchable.NormalType == NormalType.Z) {
                return StartPushDistance = BoxCollider.size.z * BoxCollider.transform.lossyScale.z / 2.0f * 1;
            } else if(NearInterationTouchable.NormalType == NormalType.NY) {
                return StartPushDistance = BoxCollider.size.y * BoxCollider.transform.lossyScale.y / 2.0f * -1;
            } else if(NearInterationTouchable.NormalType == NormalType.Y) {
                return StartPushDistance = BoxCollider.size.y * BoxCollider.transform.lossyScale.y / 2.0f * 1;
            } else if(NearInterationTouchable.NormalType == NormalType.NX) {
                return StartPushDistance = BoxCollider.size.x * BoxCollider.transform.lossyScale.x / 2.0f * -1;
            } else {
                return StartPushDistance = BoxCollider.size.x * BoxCollider.transform.lossyScale.x / 2.0f * 1;
            }
        }
    }

    private float MaxPushDistance;
    private float maxPushDistance {
        get {
            return MaxPushDistance = -startPushDistance;
        }
    }

    private float PressDistance;
    private float pressDistance {
        get {
            return PressDistance = maxPushDistance * (1.0f - minCompressPercentage);
        }
    }

    private float ReleaseDistance;
    private float releaseDistance {
        get {
            return ReleaseDistance = -pressDistance;
        }
    }

    private float currentPushDistance = 0.0f;

    /// <summary>
    /// Represents the state of whether the button is currently being pressed.
    /// </summary>
    public bool IsPressing { get; private set; }

    private bool isTouching = false;
    ///<summary>
    /// Represents the state of whether or not a finger is currently touching this button.
    ///</summary>
    public bool IsTouching {
        get => isTouching;
        set {
            if(value != isTouching) {
                isTouching = value;
            }
        }
    }

    public virtual void Awake() {
        if(VisualScale != null) {
            ScaleObjInitLocalScale = VisualScale.localScale;
        }
        if(VisualMove != null) {
            MoveObjInitLocalPosition = VisualMove.localPosition;
        }
    }

    /// <summary>
    /// Returns the local distance along the push direction for the passed in world position
    /// </summary>
    private float GetDistanceAlongPushDirection(Vector3 positionWorldSpace) {
        Vector3 worldVector = transform.TransformPoint(BoxCollider.center) - positionWorldSpace;
        return Vector3.Dot(worldVector, Normal.normalized); ;
    }

    // This function projects the current touch positions onto the 1D press direction of the button.
    // It will output the farthest pushed distance from the button's initial position.
    private float GetFarthestDistanceAlongPressDirection(Vector3 positionWorldSpace) {
        float testDistance = GetDistanceAlongPushDirection(positionWorldSpace);
        return Mathf.Clamp(testDistance, startPushDistance, maxPushDistance);
    }

    private void PulseProximityLight(TouchPointer touchPointer) {
        // Pulse each proximity light on pointer cursors' interacting with this button.
        ProximityLight[] proximityLights = touchPointer.cursorBase?.gameObject?.GetComponentsInChildren<ProximityLight>();

        if(proximityLights != null) {
            foreach(var proximityLight in proximityLights) {
                proximityLight.Pulse();
            }
        }
    }

    private void UpdatePressedState(float pushDistance, TouchPointer touchPointer) {
        // If we aren't in a press and can't start a simple one.
        if(!IsPressing) {
            // Compare to our previous push depth. Use previous push distance to handle back-presses.
            if(pushDistance >= pressDistance) {
                IsPressing = true;
                AudioSystem.getInstance.PlayAudioOneShot(gameObject, PressAudio);
                Execute(InteractionTouchableType.PokePress, null);
                PulseProximityLight(touchPointer);
            }
        }

        // If we're in a press, check if the press is released now.
        else {
            //float releaseDistance = pressDistance - releaseDistanceDelta;
            if(pushDistance <= releaseDistance) {
                IsPressing = false;
                AudioSystem.getInstance.PlayAudioOneShot(gameObject, ReleaseAudio);
                Execute(InteractionTouchableType.PokeRelease, null);
            }
        }
    }

    void UpdateVisual(float currentPushDistance) {
        if(VisualMove != null) {
            //Debug.Log("Update Visual");
            Vector3 position = Vector3.zero;
            if(useCustomMovePosition) {
                Vector3 localVisualPosition = Vector3.Lerp(visualMoveStartLocalPosition, visualMoveEndLocalPosition, (currentPushDistance - startPushDistance) / (maxPushDistance - startPushDistance));
                position = transform.TransformPoint(localVisualPosition);
            } else {
                position = transform.position + transform.TransformVector(MoveObjInitLocalPosition) - (currentPushDistance - startPushDistance) * Normal.normalized;
            }
            VisualMove.position = position;
        }

        if(VisualScale != null) {
            Vector3 scale = Vector3.one;
            float pressPercentage = 1.0f - (currentPushDistance - startPushDistance) / (maxPushDistance - startPushDistance);
            if(useCustomScale) {
                scale = Vector3.Lerp(visualEndLocalScale, visualStartLocalScale, pressPercentage); ;
            } else {
                scale = ScaleObjInitLocalScale;
                scale.z = ScaleObjInitLocalScale.z * pressPercentage;
            }
            VisualScale.transform.localScale = scale;
        }
    }

    void ResetVisual() {
        if(VisualMove) {
            VisualMove.DOMove(useCustomMovePosition ? transform.TransformPoint(visualMoveStartLocalPosition) : transform.position + transform.TransformVector(MoveObjInitLocalPosition), 0.2f);
        }
        if(VisualScale) {
            VisualScale.DOScale(useCustomScale ? visualStartLocalScale : ScaleObjInitLocalScale, 0.2f);
        }
    }

    private bool HasPassedThroughStartPlane(Vector3 positionWorldSpace) {
        float distanceAlongPushDirection = GetDistanceAlongPushDirection(positionWorldSpace);
        return distanceAlongPushDirection <= startPushDistance;
    }

    public override void OnPokeDown(TouchPointer touchPointer, SCPointEventData eventData) {
        AudioSystem.getInstance.PlayAudioOneShot(gameObject, PokeDownAudio);
        Execute(InteractionTouchableType.PokeDown, eventData);
        // Back-Press Detection:
        // Accept touch only if controller pushed from the front.
        if(enforceFrontPush && !HasPassedThroughStartPlane(touchPointer.PreviousTouchPosition)) {
            Debug.Log("Not Front Press");
            return;
        }
        IsTouching = true;
    }

    public override void OnPokeUpdated(TouchPointer touchPointer, SCPointEventData eventData) {
        Execute(InteractionTouchableType.PokeUpdate, eventData);
        if(IsTouching) {
            currentPushDistance = Mathf.Lerp(currentPushDistance, GetFarthestDistanceAlongPressDirection(touchPointer.TouchPosition), 0.5f);
            UpdatePressedState(currentPushDistance, touchPointer);
            UpdateVisual(currentPushDistance);
        }
    }

    public override void OnPokeUp(TouchPointer touchPointer, SCPointEventData eventData) {
        AudioSystem.getInstance.PlayAudioOneShot(gameObject, PokeUpAudio);
        Execute(InteractionTouchableType.PokeUp, eventData);
        ResetVisual();
        if(IsPressing) {
            IsPressing = false;
            AudioSystem.getInstance.PlayAudioOneShot(gameObject, SCAudiosConfig.AudioType.ButtonUnpress);
            Execute(InteractionTouchableType.PokeRelease, eventData);
        }
        IsTouching = false;
    }

    private void Execute(InteractionTouchableType id, BaseEventData eventData) {
        for(int i = 0; i < Triggers.Count; i++) {
            InteractionTouchableEntry entry = Triggers[i];
            if(entry.eventID == id) {
                entry.callback?.Invoke(eventData);
            }
        }
    }
}
