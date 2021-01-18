using SC.XR.Unity.Module_InputSystem;
using SC.XR.Unity.Module_InputSystem.InputDeviceHand;
using UnityEngine;
using UnityEngine.Serialization;

namespace SC.XR.Unity.Module_InputSystem
{
    public class HandInteractionTouchRotate : PokeHandler
    {
        [SerializeField]
        [FormerlySerializedAs("TargetObjectTransform")]
        private Transform targetObjectTransform = null;

        [SerializeField]
        private float rotateSpeed = 300.0f;



        public override void OnPokeUpdated(TouchPointer touchPointer, SCPointEventData eventData) {
            base.OnPokeUpdated(touchPointer, eventData);
            if(targetObjectTransform != null) {
                targetObjectTransform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
            }
        }

    }
}