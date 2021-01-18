using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public class ResetLoading : SCModuleMono {

        InputDeviceGCPartUI _inputDeviceGCPartUI;
        public InputDeviceGCPartUI inputDeviceGCPartUI {
            get {
                if(_inputDeviceGCPartUI) {
                    _inputDeviceGCPartUI = GetComponentInParent<InputDeviceGCPartUI>();
                }
                return _inputDeviceGCPartUI;
            }
        }

        Animator resetLoadingAnimator;
        float resetLoadingClipLength = 0;
        float timer = 0;

        Vector3 loadingPosition = new Vector3(0, 0, 1);
        MeshRenderer[] meshRenderer;


        public override void OnSCStart() {
            base.OnSCStart();
            timer = 0;
            DebugMy.Log("OnEnable:" + timer, this);
            if(meshRenderer == null) {
                meshRenderer = GetComponentsInChildren<MeshRenderer>(true);
            }
            foreach(var item in meshRenderer) {
                item.enabled = false;
            }


            resetLoadingAnimator = GetComponent<Animator>();
            //resetLoadingAnimator.enabled = true;
            AnimationClip[] clips = resetLoadingAnimator.runtimeAnimatorController.animationClips;
            if(clips.Length > 0) {
                resetLoadingClipLength = clips[0].length;
            }
        }
        public override void OnSCDisable() {
            base.OnSCDisable();
            DebugMy.Log("OnDisable", this);
            if(meshRenderer == null) {
                meshRenderer = GetComponentsInChildren<MeshRenderer>(true);
            }
            foreach(var item in meshRenderer) {
                item.enabled = false;
            }
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            if(SvrManager.Instance && SvrManager.Instance.gameObject.activeSelf) {
                transform.position = SvrManager.Instance.head.TransformPoint(loadingPosition);
                //transform.LookAt(SvrManager.Instance.head);
                transform.eulerAngles = SvrManager.Instance.head.eulerAngles + new Vector3(0, 0, -7);
            }
            timer += Time.deltaTime;
            if(timer > resetLoadingClipLength) {
                timer = 0;
                gameObject.SetActive(false);
            }
        }
    }
}
