using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHead {
    public class InputDataGetPosture : InputDataGetOneBase {

        InputDataGetHead inputDataGetHead;
        public InputDataGetPosture(InputDataGetHead _inputDataGetHead) : base(_inputDataGetHead) {
            inputDataGetHead = _inputDataGetHead;
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            OnUpdatePosition();
            OnUpdateRotation();
        }

        void OnUpdateRotation() {
            if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                inputDataGetHead.inputDeviceHeadPart.inputDataHead.rotation = SvrManager.Instance.head.rotation;
            } else if(Camera.main) {
                inputDataGetHead.inputDeviceHeadPart.inputDataHead.rotation = Camera.main.transform.rotation;
            }
        }

        void OnUpdatePosition() {
            if(SvrManager.Instance != null && SvrManager.Instance.gameObject.activeSelf) {
                inputDataGetHead.inputDeviceHeadPart.inputDataHead.position = SvrManager.Instance.head.position;
            } else if(Camera.main) {
                inputDataGetHead.inputDeviceHeadPart.inputDataHead.position = Camera.main.transform.position;
            }
        }
    }
}
