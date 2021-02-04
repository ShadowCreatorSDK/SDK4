using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.KS {


    public class InputDataKS : InputDataGC {

        public InputDeviceKSPart inputDeviceKSPart;
        public InputDataKS(InputDeviceKSPart inputDeviceKSPart) : base(inputDeviceKSPart) {
            this.inputDeviceKSPart = inputDeviceKSPart;
        }

        public KSIndex ksIndex {
            get {
                if(inputDeviceKSPart.PartType == InputDevicePartType.KSLeft) {
                    return KSIndex.Left;
                } else if(inputDeviceKSPart.PartType == InputDevicePartType.KSRight) {
                    return KSIndex.Right;
                }
                return KSIndex.UnKnow;
            }
        }

        public class HallData {
            public int deviceID;
            public int HallFoward;
            public int HallInside;
        }

        public static List<HallData> TempHallDataList = new List<HallData>();
        public int HallFoward = 10;
        public int HallInside = 10;


        public class BatteryPowerData {
            public int deviceID;
            public int BatteryPower;
        }

        public static List<BatteryPowerData> TempBatteryPowerDataList = new List<BatteryPowerData>();
        public int BatteryPower;

        public class JoystickData {
            public int deviceID;
            public int JoystickX;
            public int JoystickY;
        }

        public static List<JoystickData> TempJoystickDataList = new List<JoystickData>();
        public int JoystickX = 8;
        public int JoystickY = 8;

        public class ChargingData {
            public int deviceID;
            public bool isCharging;
        }

        public static List<ChargingData> TempChargingDataList = new List<ChargingData>(); 
        public bool isCharging;

        public class StatusData {
            public int deviceID;
            public bool isConnected;
        }
        public static List<StatusData> StatusDataList = new List<StatusData>();


    }
}
