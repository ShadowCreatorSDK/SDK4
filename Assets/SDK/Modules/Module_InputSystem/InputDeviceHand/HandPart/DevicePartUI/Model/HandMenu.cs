using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SC.XR.Unity;
using SC.XR.Unity.Module_InputSystem.InputDeviceHand;

public class HandMenu : SCModuleMono {

    InputDeviceHandPartUI _inputDeviceHandPartUI;
    public InputDeviceHandPartUI inputDeviceHandPartUI {
        get {
            if(_inputDeviceHandPartUI == null) {
                _inputDeviceHandPartUI = GetComponentInParent<InputDeviceHandPartUI>();
            }
            return _inputDeviceHandPartUI;
        }
    }

    protected InputDataHand inputDataHand {
        get {
            return inputDeviceHandPartUI?.inputDeviceHandPart.inputDataHand;
        }
    }

    public override void OnSCLateUpdate() {
        base.OnSCLateUpdate();
        UpdateTransform();
    }

    protected virtual void UpdateTransform() {
        transform.position = inputDataHand.handInfo.right*0.08f+ inputDataHand.handInfo.centerPosition;
        transform.rotation = Quaternion.LookRotation(inputDeviceHandPartUI.modelHand.ActiveHandModel.GetJointTransform(FINGER.small, JOINT.Four).transform.up,
            inputDeviceHandPartUI.modelHand.ActiveHandModel.GetJointTransform(FINGER.small, JOINT.Four).transform.forward);
        //transform.rotation = inputDeviceHandPartUI.modelHand.fingerUI[(int)FINGER.small].jointGameObject[(int)JOINT.Four].transform.rotation;
    }

}
