using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class HandDispatcher : DispatcherBase {
        public HandDispatcher(InputDevicePartDispatchEventHand inputDevicePartDispatchEventHand) :base(inputDevicePartDispatchEventHand) {
        }


        /// <summary>
        /// When Any part Device of Any key Down will invoke this delegate
        /// </summary>
        /// <param name="keyCode">which key</param>
        /// <param name="part">which part,part.PartType</param>
        //public static event HandEventDelegate HandCatchEventDelegate;


        //public static void OnHandTurnFaceDown(GameObject obj, InputDevice26DofGesturePart devicePart, SCPointEventData eventData = null) {
        //    ExecuteEvents.Execute<IGestureHandTurnFaceDownHandler>(obj, null, (x, y) => x.OnHandTurnFaceDown(devicePart, eventData));
        //}
        //public static void OnHandTurnFaceUp(GameObject obj, InputDevice26DofGesturePart devicePart, SCPointEventData eventData = null) {
        //    ExecuteEvents.Execute<IGestureHandTurnFaceUpHandler>(obj, null, (x, y) => x.OnHandTurnFaceUp(devicePart, eventData));
        //}
        //public static void OnHandTurnFaceDrag(GameObject obj, InputDevice26DofGesturePart devicePart, SCPointEventData eventData = null) {
        //    ExecuteEvents.Execute<IGestureHandTurnFaceDragHandler>(obj, null, (x, y) => x.OnHandTurnFaceDrag(devicePart, eventData));
        //}

        //public static void OnJointTouchEnter(GameObject obj, InputDevice26DofGesturePart devicePart, SCPointEventData eventData = null) {
        //    ExecuteEvents.Execute<IGestureJointTouchEnterHandler>(obj, null, (x, y) => x.OnJointTouchEnter(devicePart, eventData));
        //}
        //public static void OnJointTouchExit(GameObject obj, InputDevice26DofGesturePart devicePart, SCPointEventData eventData = null) {
        //    ExecuteEvents.Execute<IGestureJointTouchExitHandler>(obj, null, (x, y) => x.OnJointTouchExit(devicePart, eventData));
        //}

        public static void OnHandCatchDown(GameObject obj, InputDeviceHandPart inputDeviceHandPart, SCPointEventData sCPointEventData = null) {
            ExecuteEvents.Execute<IHandCatchDownHandler>(obj, null, (x, y) => x.OnHandCatchDown(inputDeviceHandPart, sCPointEventData));
        }
        public static void OnHandCatchUp(GameObject obj, InputDeviceHandPart inputDeviceHandPart, SCPointEventData sCPointEventData = null) {
            ExecuteEvents.Execute<IHandCatchUpHandler>(obj, null, (x, y) => x.OnHandCatchUp(inputDeviceHandPart, sCPointEventData));
        }
        public static void OnHandCatchDrag(GameObject obj, InputDeviceHandPart inputDeviceHandPart, SCPointEventData sCPointEventData = null) {
            ExecuteEvents.Execute<IHandCatchDragHandler>(obj, null, (x, y) => x.OnHandCatchDrag(inputDeviceHandPart, sCPointEventData));
        }

        //public static void OnHandPinchDown(GameObject obj, InputDevice26DofGesturePart devicePart, SCPointEventData eventData = null) {
        //    ExecuteEvents.Execute<IGestureHandPinchDownHandler>(obj, null, (x, y) => x.OnHandPinchDown(devicePart, eventData));
        //}
        //public static void OnHandPinchUp(GameObject obj, InputDevice26DofGesturePart devicePart, SCPointEventData eventData = null) {
        //    ExecuteEvents.Execute<IGestureHandPinchUpHandler>(obj, null, (x, y) => x.OnHandPinchUp(devicePart, eventData));
        //}

        public static void OnPokeDown(GameObject obj, TouchPointer touchPointer, SCPointEventData sCPointEventData = null) {
            ExecuteEvents.Execute<IPokeDownHandler>(obj, null, (x, y) => x.OnPokeDown(touchPointer, sCPointEventData));
        }
        public static void OnPokeUp(GameObject obj, TouchPointer touchPointer, SCPointEventData sCPointEventData = null) {
            ExecuteEvents.Execute<IPokeUpHandler>(obj, null, (x, y) => x.OnPokeUp(touchPointer, sCPointEventData));
        }
        public static void OnPokeUpdated(GameObject obj, TouchPointer touchPointer, SCPointEventData sCPointEventData = null) {
            ExecuteEvents.Execute<IPokeUpdatedHandler>(obj, null, (x, y) => x.OnPokeUpdated(touchPointer, sCPointEventData));
        }
    }
    public interface IPokeDownHandler : IEventSystemHandler {
        void OnPokeDown(TouchPointer touchPointer, SCPointEventData eventData);
    }
    public interface IPokeUpHandler : IEventSystemHandler {
        void OnPokeUp(TouchPointer touchPointer, SCPointEventData eventData);
    }
    public interface IPokeUpdatedHandler : IEventSystemHandler {
        void OnPokeUpdated(TouchPointer touchPointer, SCPointEventData eventData);
    }
    //public interface IGestureJointTouchEnterHandler : IEventSystemHandler {
    //    void OnJointTouchEnter(InputDevice26DofGesturePart devicePart, SCPointEventData eventData);
    //}
    //public interface IGestureJointTouchExitHandler : IEventSystemHandler {
    //    void OnJointTouchExit(InputDevice26DofGesturePart devicePart, SCPointEventData eventData);
    //}



    //public interface IGestureHandPinchDownHandler : IEventSystemHandler {
    //    void OnHandPinchDown(InputDevice26DofGesturePart devicePart, SCPointEventData eventData);
    //}
    //public interface IGestureHandPinchUpHandler : IEventSystemHandler {
    //    void OnHandPinchUp(InputDevice26DofGesturePart devicePart, SCPointEventData eventData);
    //}

}
