
using SC.XR.Unity.Module_InputSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_Device {

    public abstract class DeviceBase {

        [SerializeField]
        private XRType m_XRType = XRType.Other;
        public XRType XRType { get => m_XRType; }

        public List<InputDeviceType> SupportInputDevices;

        public string modelName;

        public int GreyAmount;
        /// <summary>
        /// One GreyCamera GreyCameraResolution
        /// (Weight,Height)
        /// </summary>
        public Vector2 GreyCameraResolution;

        public bool HasRGB;
        public Vector3 RGBRotationOffset;
        public Vector3 RGBPositionOffset;


        public abstract string MODEL {
            get;
        }

        /// <summary>
        /// SN
        /// </summary>
        public abstract string SN {
            get;
        }

        /// <summary>
        /// Release_Vesion
        /// </summary>
        public abstract string RELEASE_VERSION {
            get;
        }

        /// <summary>
        /// BatteryLevel
        /// </summary>
        public abstract int BatteryLevel {
            get;
        }

        public virtual void ShowInfo() {
            DebugMy.Log(" *** Device Info *** "
                        + "  XRType:" + XRType
                        + "  MODEL:" + MODEL
                        + "  SN:" + SN
                        + "  RELEASE_VERSION:" + RELEASE_VERSION
                        + "  BatteryLevel:" + BatteryLevel
                        + "  modelName:" + modelName
                        + "  GreyAmount:" + GreyAmount
                        + "  GreyCameraResolution:" + GreyCameraResolution
                        + "  HasRGB:" + HasRGB
                        + "  RGBRotationOffset:" + RGBRotationOffset
                        + "  RGBRotationOffset:" + RGBRotationOffset
                        , this, true);

        }

    }


}