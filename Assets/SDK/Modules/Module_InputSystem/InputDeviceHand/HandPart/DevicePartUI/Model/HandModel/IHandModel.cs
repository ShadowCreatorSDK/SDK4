using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand
{
    public interface IHandModel {

        /// <summary>
        /// Strcut for store joint data
        /// </summary>
        FingerUI[] fingerUI { get; }

        HandModelType handModelType { get; }

        /// <summary>
        /// Tranform for Entire Model Operation
        /// </summary>
        Transform handJointContainer { get; }

        void UpdateTransform();

        Transform GetJointTransform(FINGER finger, JOINT joint);
    }

}