using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    [Serializable]
    public class FingerUI {
        public Transform[] jointGameObject;
    }

    public abstract class AbstractHandModel : SCModuleMono, IHandModel {

        /// <summary>
        /// Father Module
        /// </summary>
        ModelHand _modelHand;
        public ModelHand modelHand {
            get {
                if (_modelHand == null) {
                    _modelHand = GetComponentInParent<ModelHand>();
                }
                return _modelHand;
            }
        }

        [SerializeField]
        private FingerUI[] mFingerUI;
        public FingerUI[] fingerUI => mFingerUI;


        [SerializeField]
        private Transform mhandJointContainer;
        public Transform handJointContainer => mhandJointContainer;


        public abstract HandModelType handModelType { get; }
        public abstract void UpdateTransform();
        public abstract Transform GetJointTransform(FINGER finger, JOINT joint);

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            UpdateTransform();
        }
    }
}