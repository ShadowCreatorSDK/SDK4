using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {
    public class BT3DofConnectListener : AndroidJavaProxy {

        InputDeviceBT3Dof inputDeviceBT3Dof;

        public event Action ConnectionStateChangeCallBack;

        public BT3DofConnectListener(InputDeviceBT3Dof inputDeviceBT3Dof) : base("com.invision.handshank.callback.HandShankConnStateCallback") {
            this.inputDeviceBT3Dof = inputDeviceBT3Dof;
        }

        /// <summary>
        /// connect status change callback
        /// </summary>
        /// <param name="index">handshank index:0 for  handshankOne,1 for handshankTwo</param>
        /// <param name="state">handshank connect status:0 for disconnect,1 for connect</param>
        public void onConnectionStateChange(int index, int state) {
            bool isConnected = state == 1 ? true : false;
            DebugMy.Log("BT3DofConnectListener onConnectionStateChange: " + index + " isConnected:" + isConnected, this);
            InputDataBT3Dof.StatusDataList.Add(new InputDataBT3Dof.StatusData() { isConnected = isConnected, deviceID = index });
        }
    }
}
