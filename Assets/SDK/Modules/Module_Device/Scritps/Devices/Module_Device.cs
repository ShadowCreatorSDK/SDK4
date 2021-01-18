
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_Device {
    public class Module_Device {

        private static Module_Device instance;
        public static Module_Device getInstance {
            get {
                if (instance == null) {
                    instance = new Module_Device();
                }
                return instance;
            }
        }

        private DeviceList _deviceAssets;
        private DeviceList deviceAssets {
            get {
                if (_deviceAssets == null) {
                    _deviceAssets = Resources.Load<DeviceList>("DeviceList");
                }
                return _deviceAssets;
            }
        }

        private DeviceBase _current;
        public DeviceBase Current {
            get {
                if (_current == null) {

                    if (!Application.isEditor && Application.platform == RuntimePlatform.Android) {
                        foreach (var item in deviceAssets.androidDevice) {
                            if (item.modelName == item.MODEL) {
                                _current = item;
                                break;
                            }
                        }

                        if (_current == null) {
                            foreach (var item in deviceAssets.androidDevice) {
                                if (item.type == AndroidDeviceType.Other ) {
                                    _current = item;
                                    break;
                                }
                            }
                        }
                        CurrentAndroid = (AndroidDevice)_current;

                    } else if (Application.platform == RuntimePlatform.IPhonePlayer) {
                        if (deviceAssets.iosDevice != null) {
                            _current = deviceAssets.iosDevice;
                        }

                    } else {
                        if (deviceAssets.standaloneDevice != null) {
                            _current = deviceAssets.standaloneDevice;
                        }
                    }
                }
                return _current;
            }
        }

        public AndroidDevice CurrentAndroid { get; private set; }

    }


}