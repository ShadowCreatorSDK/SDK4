using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC.BT3Dof {

    public class BT3DofKeyListener : AndroidJavaProxy {

        public InputDataGetBT3DofKey inputDataGetBT3DofKey;

        public BT3DofKeyListener(InputDataGetBT3DofKey inputDataGetBT3DofKey) : base("com.invision.handshank.callback.HandShankKeyEventCallback") {
            this.inputDataGetBT3DofKey = inputDataGetBT3DofKey;
        }


        //wangcq327 20181221
        //备注：此接口android有按键事件时触发，不是每次触发都有机会在Update中被派发，所以需要用keyList保存起来
        //此接口中也不能直接派发事件，原因是派发事件的方法中有委托
        void onKeyEventChanged(int keycode, int keyevent, int deviceId) {
            InputDataBT3Dof.GCData.GCKeyList.Add(new GCKeyData() { keycode = keycode, keyevent = keyevent, deivceID = deviceId });
            //foreach(var keydata in InputDataBT3Dof.GCData.GCKeyList) {
            //    DebugMy.Log("onKeyEventChanged: deviceID:" + keydata.deivceID + "   " + keydata.keycode+" "+ keydata.keyevent,this);
            //}
        }


    }
}
