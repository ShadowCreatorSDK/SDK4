using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {

    public class GCStaticData {
        public List<GCKeyData> GCKeyList = new List<GCKeyData>();
    }
    public class GCKeyData {
        public int keycode = -1;
        public int keyevent = -1;
        public int deivceID = -1;
    }

    public abstract class InputDataGC : InputDataBase {

        public InputDeviceGCPart inputDeviceGCPart;
        public InputDataGC(InputDeviceGCPart inputDeviceGCPart) : base(inputDeviceGCPart) {
            this.inputDeviceGCPart = inputDeviceGCPart;
        }

        public int SoftVesion;

        /// <summary>
        /// 静态全局信息
        /// </summary>
        public static GCStaticData GCData = new GCStaticData();

        public bool isConnected = false;

        /// <summary>
        /// 手柄类型 见 InputDeviceGCType
        /// </summary>
        public GCType GCType = GCType.Null;

        /// <summary>
        /// 手柄标识名 “K02” "K07"
        /// </summary>
        public string GCName = "";

        /// <summary>
        /// TouchPanel Data
        /// </summary>
        public Vector2 tpPosition { get; private set; }

        public bool isTpTouch {
            get {
                if(tpPosition[0] == 0 && tpPosition[1] == 0) {
                    return false;
                }
                return true;
            }
        }
        public void InputDataAddTouch( Vector2 _tpPosition) {
            tpPosition = _tpPosition;
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            GCData = null;
            GCType = GCType.Null;
            GCName = "";
            tpPosition = Vector2.zero;
        }
    }
}
