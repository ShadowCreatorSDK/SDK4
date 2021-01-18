using SC.XR.Unity.Module_InputSystem;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SC.XR.Unity
{
    [AddComponentMenu("UI/SCSlider", 33)]
    public class SCSlider3D : MonoBehaviour, IDragHandler, IInitializePotentialDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public enum Direction
        {
            LeftToRight,
            RightToLeft,
            BottomToTop,
            TopToBottom,
        }

        [Serializable]
        public class SliderEvent : UnityEvent<float> { }

        [Space]

        [SerializeField]
        private float m_MinValue = 0;
        public float minValue { get { return m_MinValue; } set { if (SCSetPropertyUtility.SetStruct(ref m_MinValue, value)) { Set(m_Value); UpdateVisuals(); } } }

        [SerializeField]
        private float m_MaxValue = 1;
        public float maxValue { get { return m_MaxValue; } set { if (SCSetPropertyUtility.SetStruct(ref m_MaxValue, value)) { Set(m_Value); UpdateVisuals(); } } }

        [SerializeField]
        private bool m_WholeNumbers = false;
        public bool wholeNumbers { get { return m_WholeNumbers; } set { if (SCSetPropertyUtility.SetStruct(ref m_WholeNumbers, value)) { Set(m_Value); UpdateVisuals(); } } }

        [SerializeField]
        protected float m_Value;
        public virtual float value
        {
            get
            {
                if (wholeNumbers)
                    return Mathf.Round(m_Value);
                return m_Value;
            }
            set
            {
                Set(value);
            }
        }

        public float normalizedValue
        {
            get
            {
                if (Mathf.Approximately(minValue, maxValue))
                    return 0;
                return Mathf.InverseLerp(minValue, maxValue, value);
            }
            set
            {
                this.value = Mathf.Lerp(minValue, maxValue, value);
            }
        }

        [Space]
        // Allow for delegate-based subscriptions for faster events than 'eventReceiver', and allowing for multiple receivers.
        [SerializeField]
        private SliderEvent m_OnValueChanged = new SliderEvent();
        public SliderEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }

        [SerializeField]
        private UnityEvent m_OnPointerDown = new UnityEvent();
        public UnityEvent onPointerDown { get { return m_OnPointerDown; } set { m_OnPointerDown = value; } }

        [SerializeField]
        private UnityEvent m_OnPointerUp = new UnityEvent();
        public UnityEvent onPointerUp { get { return m_OnPointerUp; } set { m_OnPointerUp = value; } }

        private DrivenRectTransformTracker m_Tracker;

        public BoxCollider handler;
        public BoxCollider handlerContainer;

        public Vector3 SliderStartWorldPosition
        {
            get
            {
                if (handlerContainer != null)
                {
                    return handlerContainer.transform.position - handlerContainer.transform.TransformVector(Vector3.right * handlerContainer.size[0] / 2f);
                }
                return Vector3.zero;
            }
        }

        public Vector3 SliderEndWorldPosition
        {
            get
            {
                if (handlerContainer != null)
                {
                    return handlerContainer.transform.position + handlerContainer.transform.TransformVector(Vector3.right * handlerContainer.size[0] / 2f);
                }
                return Vector3.zero;
            }
        }

        protected SCSlider3D()
        { }

        public virtual void Rebuild(CanvasUpdate executing)
        {
#if UNITY_EDITOR
            if (executing == CanvasUpdate.Prelayout)
                onValueChanged.Invoke(value);
#endif
        }

        public virtual void LayoutComplete()
        { }

        public virtual void GraphicUpdateComplete()
        { }

        protected virtual void OnEnable()
        {
            Set(m_Value, false);
            // Update rects since they need to be initialized correctly.
            UpdateVisuals();
        }

        protected virtual void OnDisable()
        {
            m_Tracker.Clear();
        }

        float ClampValue(float input)
        {
            float newValue = Mathf.Clamp(input, minValue, maxValue);
            if (wholeNumbers)
                newValue = Mathf.Round(newValue);
            return newValue;
        }

        // Set the valueUpdate the visible Image.
        void Set(float input)
        {
            Set(input, true);
        }

        protected virtual void Set(float input, bool sendCallback)
        {
            // Clamp the input
            float newValue = ClampValue(input);

            // If the stepped value doesn't match the last one, it's time to update
            if (m_Value == newValue)
                return;

            m_Value = newValue;
            UpdateVisuals();
            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("Slider.value", this);
                m_OnValueChanged.Invoke(newValue);
            }
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            if (wholeNumbers)
            {
                m_MinValue = Mathf.Round(m_MinValue);
                m_MaxValue = Mathf.Round(m_MaxValue);
            }

            //Onvalidate is called before OnEnabled. We need to make sure not to touch any other objects before OnEnable is run.

            Set(m_Value, false);
            // Update rects since other things might affect them even if value didn't change.
            UpdateVisuals();
        }

#endif // if UNITY_EDITOR

        // Force-update the slider. Useful if you've changed the properties and want it to update visually.
        private void UpdateVisuals()
        {
            Vector3 handlerPosition = SliderStartWorldPosition + (SliderEndWorldPosition - SliderStartWorldPosition) * normalizedValue;
            if (handler != null)
            {
                handler.transform.position = handlerPosition;
            }
        }

        // Update the slider's position based on the mouse.
        void UpdateDrag(PointerEventData eventData, Camera cam, bool isDownClick = false)
        {
            Transform clickTransform = handlerContainer.transform;
            Vector2 localPositionInPlane;
            if (!SCTransformUtility.ScreenPointToLocalPointInPlane(clickTransform, eventData.position, cam, out localPositionInPlane))
                return;
            Vector2 colliderPosition = new Vector2(handlerContainer.size.x / 2f, handlerContainer.size.y / 2f) ;
            localPositionInPlane += colliderPosition;

            float val = Mathf.Clamp01(localPositionInPlane.x / handlerContainer.size.x);


            Debug.Log("localPositionInPlane.x:" + localPositionInPlane.x + " handlerContainer.bounds.size.x:" + handlerContainer.size.x);

            normalizedValue = val;
            return;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            m_OnPointerDown?.Invoke();
            //SCPointEventData scPointEventData = eventData as SCPointEventData;
            Camera eventCamera = eventData.pressEventCamera;
            //pointerDownWorldPosition = scPointEventData.dragAuchorPosition;
            //startSliderValue = normalizedValue;
            //worldToCameraMatrixCache = eventCamera.worldToCameraMatrix;
            //projectionMatrixCache = eventCamera.projectionMatrix;
            if (CheckIsClickCollider(handler, eventData))
            {
                //do nothing
            }
            else
            {               
                UpdateDrag(eventData, eventCamera, true);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_OnPointerUp?.Invoke();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            UpdateDrag(eventData, eventData.pressEventCamera);
        }

        public virtual void OnInitializePotentialDrag(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        private bool CheckIsClickCollider(Collider collider, PointerEventData eventData)
        {
            Ray ray = new Ray();
            float distance = 0f;
            ComputeRayAndDistance(eventData, ref ray, ref distance);
            RaycastHit hitResult;
            if (collider.Raycast(ray, out hitResult, distance))
            {
                return true;
            }
            return false;
        }

        private void ComputeRayAndDistance(PointerEventData eventData, ref Ray ray, ref float distanceToClipPlane)
        {
            if (eventData.pressEventCamera == null)
            {
                Debug.LogError("pressEventCamera do not exist");
                return;
            }

            Camera eventCamera = eventData.pressEventCamera;
            Vector2 screenPosition = eventData.position;

            ray = eventCamera.ScreenPointToRay(screenPosition);
            // compensate far plane distance - see MouseEvents.cs
            float projectionDirection = ray.direction.z;
            distanceToClipPlane = Mathf.Approximately(0.0f, projectionDirection)
                ? Mathf.Infinity
                : Mathf.Abs((eventCamera.farClipPlane - eventCamera.nearClipPlane) / projectionDirection);
        }
    }
}
