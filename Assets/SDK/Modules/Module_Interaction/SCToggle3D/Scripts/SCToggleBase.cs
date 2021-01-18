using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace SC.XR.Unity
{
    public abstract  class SCToggleBase : MonoBehaviour, IPointerClickHandler
    {
        [Serializable]
        public class ToggleEvent : UnityEvent<bool>
        {

        }

        [SerializeField]
        private SCToggleGroup3D m_Group;

        public SCToggleGroup3D group
        {
            get { return m_Group; }
            set
            {
                m_Group = value;
#if UNITY_EDITOR
                if (Application.isPlaying)
#endif
                {
                    SetToggleGroup(m_Group, true);
                    PlayEffect();
                }
            }
        }

        public ToggleEvent onValueChanged = new ToggleEvent();

        [FormerlySerializedAs("m_IsActive")]
        [Tooltip("Is the toggle currently on or off?")]
        [SerializeField]
        protected bool m_IsOn;

        protected SCToggleBase()
        { }

        protected void OnEnable()
        {
            SetToggleGroup(m_Group, false);
            PlayEffect();
        }

        protected void OnDisable()
        {
            SetToggleGroup(null, false);
        }

#if UNITY_EDITOR
        protected void OnValidate()
        {
            PlayEffect();
        }

#endif // if UNITY_EDITOR

        private void SetToggleGroup(SCToggleGroup3D newGroup, bool setMemberValue)
        {
            SCToggleGroup3D oldGroup = m_Group;

            // Sometimes IsActive returns false in OnDisable so don't check for it.
            // Rather remove the toggle too often than too little.
            if (m_Group != null)
                m_Group.UnregisterToggle(this);

            // At runtime the group variable should be set but not when calling this method from OnEnable or OnDisable.
            // That's why we use the setMemberValue parameter.
            if (setMemberValue)
                m_Group = newGroup;

            // Only register to the new group if this Toggle is active.
            if (newGroup != null)
                newGroup.RegisterToggle(this);

            // If we are in a new group, and this toggle is on, notify group.
            // Note: Don't refer to m_Group here as it's not guaranteed to have been set.
            if (newGroup != null && newGroup != oldGroup && isOn)
                newGroup.NotifyToggleOn(this);
        }

        public bool isOn
        {
            get { return m_IsOn; }
            set
            {
                Set(value);
            }
        }

        void Set(bool value)
        {
            Set(value, true);
        }

        void Set(bool value, bool sendCallback)
        {
            if (m_IsOn == value)
                return;

            // if we are in a group and set to true, do group logic
            m_IsOn = value;
            if (m_Group != null)
            {
                if (m_IsOn || (!m_Group.AnyTogglesOn() && !m_Group.allowSwitchOff))
                {
                    m_IsOn = true;
                    m_Group.NotifyToggleOn(this);
                }
            }

            // Always send event when toggle is clicked, even if value didn't change
            // due to already active toggle in a toggle group being clicked.
            // Controls like Dropdown rely on this.
            // It's up to the user to ignore a selection being set to the same value it already was, if desired.
            PlayEffect();
            if (sendCallback)
            {
                UISystemProfilerApi.AddMarker("Toggle.value", this);
                onValueChanged.Invoke(m_IsOn);
            }
        }

        protected abstract void PlayEffect();

        protected void Start()
        {
            PlayEffect();
        }

        private void InternalToggle()
        {
            isOn = !isOn;
        }

        /// <summary>
        /// React to clicks.
        /// </summary>
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            InternalToggle();
        }
    }
}
