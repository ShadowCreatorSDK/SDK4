using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem {

    public class SCPointEventData : PointerEventData {

        public InputDevicePartBase inputDevicePartBase;
        public SCPointEventData(InputDevicePartBase inputDevicePartBase, EventSystem eventSystem) : base(eventSystem) {
            this.inputDevicePartBase = inputDevicePartBase;
        }

        public PointerInputModule.MouseButtonEventData MouseButtonEventData = new PointerInputModule.MouseButtonEventData();

        /// <summary>
        /// 触摸Press的百分比，当为0-1 1表示Press触发
        /// </summary>
        public float TouchPressPercent = 0;
        public Action<float,float> TouchPressPercentDelegate;



        /// <summary>
        ///碰撞物体中心相对于HitPointer的局部坐标
        /// </summary>
        public Vector3 HitPointerRelativeRayCasterCamera = Vector3.zero;

        public Vector3 HitPointerDeltaDragObjCenter = Vector3.zero;


        /// <summary>
        /// Drag实时位置,是物体中心位置而不是Focus,通过hitPointer的位置和SelfCentreRelativehitPointerPosition算出的物体因该在的位置
        /// </summary>
        public Vector3 dragObjPosition = Vector3.zero;

        public Vector3 dragAnchorPosition3D = Vector3.zero;
        public Vector3 Position3D = Vector3.zero;
        public Vector3 PressPosition3D = Vector3.zero;
        
        public Vector3 pressForward = Vector3.zero;
        public Vector3 Forward = Vector3.zero;

        public GameObject DownPressGameObject;


        public void Clear() {
            TouchPressPercent = 0;
            TouchPressPercentDelegate = null;
            HitPointerRelativeRayCasterCamera = Vector3.zero;
            HitPointerDeltaDragObjCenter = Vector3.zero;
            dragObjPosition = Vector3.zero;
            dragAnchorPosition3D = Vector3.zero;
            Position3D = Vector3.zero;
            PressPosition3D = Vector3.zero;
            pressForward = Vector3.zero;
            Forward = Vector3.zero;
            HitPointerRelativeRayCasterCamera = Vector3.zero;
            DownPressGameObject = null;
        }



    }
}
