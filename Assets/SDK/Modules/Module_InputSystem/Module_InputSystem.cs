using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SC.XR.Unity.Module_Device;

namespace SC.XR.Unity.Module_InputSystem {
    public class Module_InputSystem : SCModuleMono {

        public static Module_InputSystem instance { get; private set; }
        public event Action initializeCallBack;
        public bool initialize {get; private set; }
        public bool IsRunning { get; private set; }

        public event Action<InputDeviceBase, bool> InputDeviceChangeCallBack;

        /// <summary>
        /// 所有支持的InputDevice，支持然后register后则启用
        /// </summary>
        private List<InputDeviceBase> _inputDeviceSupportList;
        public List<InputDeviceBase> inputDeviceSupportList {
            get {
                if(_inputDeviceSupportList == null) {
                    _inputDeviceSupportList = new List<InputDeviceBase>();
                    InputDeviceBase[] inputDeviceList = GetComponentsInChildren<InputDeviceBase>(true);

                    if (API_Module_Device.Current != null) {

                        foreach (var inputDevice in inputDeviceList) {
                            if (API_Module_Device.Current.SupportInputDevices.Contains(inputDevice.inputDeviceType)) {
                                DebugMy.Log("Support InputDevice:" + inputDevice.inputDeviceType, this, true);
                                _inputDeviceSupportList.Add(inputDevice);
                            }
                        }

                    } else {
                        _inputDeviceSupportList = inputDeviceList.ToList();
                    }

                    if (_inputDeviceSupportList.Count == 0) {
                        DebugMy.Log("No Support InputDevice !", this, true);
                    }
                }
                return _inputDeviceSupportList;
            }

        }


        //[Header("Enable Head")]
        //[SerializeField]
        private bool activeHead = true;

        //[Header("Enable BT3Dof")]
        //[SerializeField]
        private bool activeBT3Dof = false;

        //[Header("Enable KS")]
        //[SerializeField]
        private bool activeKS = false;


        //[Header("Enable GreyHand")]
        //[SerializeField]
        private bool activeGGT26Dof = false;

        public override void OnSCAwake() {
            base.OnSCAwake();

            if (instance != null) {
                DestroyImmediate(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            instance = this;

            DebugMy.Log("Awake", this, true);

            foreach (var inputDevice in inputDeviceSupportList) {
                AddModule(inputDevice);
            }

            initialize = true;
            IsRunning = true;
            initializeCallBack?.Invoke();

            if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "ActiveHead")) {
                activeHead = API_Module_SDKConfiguration.GetBool("Module_InputSystem", "ActiveHead", 0);
            }

            if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "ActiveBT3Dof")) {
                activeBT3Dof = API_Module_SDKConfiguration.GetBool("Module_InputSystem", "ActiveBT3Dof", 0);
            }

            if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "ActiveKS")) {
                activeKS = API_Module_SDKConfiguration.GetBool("Module_InputSystem", "ActiveKS", 0);
            }

            if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "ActiveGGT26Dof")) {
                activeGGT26Dof = API_Module_SDKConfiguration.GetBool("Module_InputSystem", "ActiveGGT26Dof", 0);
            }

        }

        public override void OnSCStart() {
            base.OnSCStart();

            SetActiveInputDevice(InputDeviceType.Head, activeHead);
            SetActiveInputDevice(InputDeviceType.BT3Dof, activeBT3Dof);
            SetActiveInputDevice(InputDeviceType.KS, activeKS);
            SetActiveInputDevice(InputDeviceType.GGT26Dof, activeGGT26Dof);

        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            initialize = false;
            IsRunning = false;
        }

        public T GetInputDevice<T>(InputDeviceType type) where T:InputDeviceBase {
            foreach(var inputDevice in inputDeviceSupportList) {
                if(type == inputDevice.inputDeviceType) {
                    return (T)inputDevice;
                }
            }
            return null;
        }

        public void SetActiveInputDevice(InputDeviceType type,bool active) {
            InputDeviceBase inputDevice = GetInputDevice<InputDeviceBase>(type);
            if(inputDevice == null)
                return;

            SetFlag(type, active);
            
            if(active) {
                inputDevice.ModuleStart();
            } else {
                inputDevice.ModuleStop();
            }

            InputDeviceChangeCallBack?.Invoke(inputDevice, active);
        }

        private void SetFlag(InputDeviceType type, bool active) {
            if(type == InputDeviceType.Head) {
                activeHead = active;
            } else if(type == InputDeviceType.BT3Dof) {
                activeBT3Dof = active;
            } else if(type == InputDeviceType.KS) {
                activeKS = active;
            }else if(type == InputDeviceType.GGT26Dof) {
                activeGGT26Dof = active;
            }
        }


        InputDeviceBase _inputDevice;
        public bool GetInputDeviceStatus(InputDeviceType type) {
            _inputDevice = GetInputDevice<InputDeviceBase>(type);
            if(_inputDevice) {
                foreach(var part in _inputDevice.inputDevicePartList) {
                    if(part.inputDataBase.isVaild == true) {
                        return true;
                    }
                }
            }
            return false;
        }


        public bool IsSomeDeviceActiveWithoutHead {
            get {
                if(GetInputDeviceStatus(InputDeviceType.GGT26Dof) ||
                    GetInputDeviceStatus(InputDeviceType.KS) ||
                    GetInputDeviceStatus(InputDeviceType.BT3Dof)
                    ) {
                    return true;
                }
                return false;
            }
        }

    }



}
