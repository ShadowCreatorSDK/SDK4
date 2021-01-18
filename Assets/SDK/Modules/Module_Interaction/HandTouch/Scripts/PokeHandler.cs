using SC.XR.Unity.Module_InputSystem;
using SC.XR.Unity.Module_InputSystem.InputDeviceHand;
using UnityEngine;
using UnityEngine.Events;

namespace SC.XR.Unity.Module_InputSystem {

    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(NearInteractionTouchable))]
    [AddComponentMenu("SDK/PokeHandler")]
    public class PokeHandler : MonoBehaviour, IPokeHandler {


        #region Event handlers
        /// <summary>
        /// A UnityEvent callback containing a TouchEventData payload.
        /// </summary>
        [System.Serializable]
        public class TouchEvent : UnityEvent<TouchPointer,SCPointEventData> { }

        [Header("Poke Events")]
        public TouchEvent PokeDown;
        public TouchEvent PokeUp;
        public TouchEvent PokeUpdated;
        #endregion

        [Header("Poke Audio")]
        [SerializeField]
        protected SCAudiosConfig.AudioType PokeDownAudio = SCAudiosConfig.AudioType.Null;
        [SerializeField]
        protected SCAudiosConfig.AudioType PokeUpAudio = SCAudiosConfig.AudioType.Null;

        public virtual void OnPokeDown(TouchPointer touchPointer, SCPointEventData eventData) {
            AudioSystem.getInstance.PlayAudioOneShot(gameObject, PokeDownAudio);
            PokeDown.Invoke(touchPointer,eventData);
        }

        public virtual void OnPokeUpdated(TouchPointer touchPointer, SCPointEventData eventData) {
            PokeUpdated.Invoke(touchPointer,eventData);
        }

        public virtual void OnPokeUp(TouchPointer touchPointer, SCPointEventData eventData) {
            AudioSystem.getInstance.PlayAudioOneShot(gameObject, PokeUpAudio);
            PokeUp.Invoke(touchPointer,eventData);
        }
    }
}