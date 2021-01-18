using AOT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {
    public class InputDeviceKS : InputDeviceGC {

        public override InputDeviceType inputDeviceType {
            get {
                return InputDeviceType.KS;
            }
        }

        bool isInvokeOnce = false;


        [Header("Enable GameController")]
        public bool LeftActive = true;
        public bool RightActive = true;
        protected override void InputDeviceStart() {
            SetActiveInputDevicePart(InputDevicePartType.KSLeft, LeftActive);
            SetActiveInputDevicePart(InputDevicePartType.KSRight, RightActive);
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();

            if(SvrPlugin.Instance != null && SvrManager.Instance.IsRunning == false)
                return;

            if(isInvokeOnce == false) {
                isInvokeOnce = true;
                if(Application.platform == RuntimePlatform.Android) {
                    try {
                        SvrPlugin.Instance.HandShank_SetKeyEventCallback(KeyEvent);
                        SvrPlugin.Instance.HandShank_SetKeyTouchEventCallback(KeyTouchEvent);
                        SvrPlugin.Instance.HandShank_SetTouchEventCallback(JoystickEvent);
                        SvrPlugin.Instance.HandShank_SetHallEventCallback(HallEvent);
                        SvrPlugin.Instance.HandShank_SetChargingEventCallback(ChargingEvent);
                        SvrPlugin.Instance.HandShank_SetBatteryEventCallback(BatteryEvent);
                        SvrPlugin.Instance.HandShank_SetConnectEventCallback(ConnectEvent);
                    } catch(Exception e) {
                        Debug.Log(e);
                    }
                }
            }
        }

        [MonoPInvokeCallback(typeof(SvrPlugin.OnKeyEvent))]
        public static void KeyEvent(int keycode, int action, int lr) {
            Debug.Log("KS -- key event: " + keycode + " " + action + " " + lr);
            InputDataGC.GCData.GCKeyList.Add(new GCKeyData() { keycode = keycode, keyevent = action, deivceID = lr });
        }


        [MonoPInvokeCallback(typeof(SvrPlugin.OnKeyTouchEvent))]
        static void KeyTouchEvent(bool key1, bool key2, bool key3, bool key4, int lr) {
            Debug.Log("KS -- KeyTouchEvent:" + key1 + " " + key2 + " " + key3 + " " + key4 + " " + lr);
        }

        [MonoPInvokeCallback(typeof(SvrPlugin.OnTouchEvent))]
        static void JoystickEvent(int touch_x, int touch_y, int lr) {
            Debug.Log("KS -- JoystickEvent:" + touch_x +" "+ touch_y + " " + lr);
            InputDataKS.TempJoystickDataList.Add(new InputDataKS.JoystickData() { JoystickX = touch_x, JoystickY = touch_y, deviceID = lr });
        }

        [MonoPInvokeCallback(typeof(SvrPlugin.OnHallEvent))]
        static void HallEvent(int hall_x, int hall_y, int lr) {
            Debug.Log("KS -- HallEvent:" + hall_x + " " + hall_y + " " + lr);
            InputDataKS.TempHallDataList.Add(new InputDataKS.HallData() { HallInside = hall_x, HallFoward = hall_y, deviceID = lr });
        }

        [MonoPInvokeCallback(typeof(SvrPlugin.OnChargingEvent))]
        static void ChargingEvent(bool isCharging, int lr) {
            Debug.Log("KS -- ChargingEvent:" + isCharging + " " + lr);
        }

        [MonoPInvokeCallback(typeof(SvrPlugin.OnBatteryEvent))]
        static void BatteryEvent(int battery, int lr) {
            Debug.Log("KS -- BatteryEvent:" + battery + " " + lr);
        }

        [MonoPInvokeCallback(typeof(SvrPlugin.OnConnectEvent))]
        static void ConnectEvent(bool isConnected, int lr) {
            Debug.Log("KS -- ConnectEvent:" + isConnected + " " + lr);
            InputDataKS.StatusDataList.Add(new InputDataKS.StatusData() { isConnected = isConnected, deviceID = lr });
        }

    }
}
