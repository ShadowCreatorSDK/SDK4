using SC.XR.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace SC.XR.Unity.Module_Device {

    [CreateAssetMenu(menuName = "SCConfig/SCDeviceConfig")]
    public class DeviceList : ScriptableObject {

        public List<AndroidDevice> androidDevice;
        public IOSDevice iosDevice;
        public StandaloneDevice standaloneDevice;

        //private List<DeviceBase> mDevices;
        //public List<DeviceBase> Devices {
        //    get {
        //        if (mDevices == null) {
        //            mDevices = new List<DeviceBase>();
        //            if (androidDevice.Count > 0) {
        //                mDevices.AddRange(androidDevice);
        //            }
        //            if (iosDevice != null) {
        //                mDevices.Add(iosDevice);
        //            }
        //            if (standaloneDevice != null) {
        //                mDevices.Add(standaloneDevice);
        //            }
        //        }
        //        return mDevices;
        //    }
        //}

    }
}