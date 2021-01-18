using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem {
    public class AutoClick : PointerHandler {

        public float autoClickTime = 3;
        Coroutine coroutineAddKey;
        bool isAddKeyFinish = false;



        public override void OnPointerEnter(PointerEventData eventData) {
            StartCountDown((eventData as SCPointEventData), autoClickTime);
        }

        public override void OnPointerDown(PointerEventData eventData) {
            StopCountDown((eventData as SCPointEventData));
        }

        public override void OnPointerExit(PointerEventData eventData) {
            StopCountDown((eventData as SCPointEventData));
        }

        void StartCountDown(SCPointEventData scData, float time) {
            if(scData == null)
                return;

            StopCountDown(scData);

            coroutineAddKey = StartCoroutine(AddKey(InputKeyCode.Enter, InputKeyState.DOWN, scData,time));
            scData.inputDevicePartBase.detectorBase.pointerBase.cursorBase.StartGazeAnimation(time);
        }

        void StopCountDown(SCPointEventData scData) {
            if(scData == null)
                return;

            if(coroutineAddKey != null)
                StopCoroutine(coroutineAddKey);

            scData.inputDevicePartBase.detectorBase.pointerBase.cursorBase.StopGazeAnimation();
            if(isAddKeyFinish) {
                isAddKeyFinish = false;
                StartCoroutine(AddKey(InputKeyCode.Enter, InputKeyState.UP, scData,0.5f));
            }
        }

        IEnumerator AddKey(InputKeyCode keycode, InputKeyState state, SCPointEventData scData, float time) {
            yield return new WaitForSeconds(time);
            scData.inputDevicePartBase.inputDataBase.inputKeys.InputDataAddKey(keycode, state);
            isAddKeyFinish = true;
        }

    }
}