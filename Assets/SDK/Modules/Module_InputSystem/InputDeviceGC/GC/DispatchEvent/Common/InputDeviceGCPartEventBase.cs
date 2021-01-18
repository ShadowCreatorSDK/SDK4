using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {

    public abstract class InputDeviceGCPartEventBase : InputDevicePartEventBase {

        InputDevicePartDispatchEventGC inputDevicePartDispatchEventGC;
        public InputDeviceGCPartEventBase(InputDevicePartDispatchEventGC inputDevicePartDispatchEventGC) : base(inputDevicePartDispatchEventGC) {
            this.inputDevicePartDispatchEventGC = inputDevicePartDispatchEventGC;
        }

        /// <summary>
        /// 超过noise算一次有效运动
        /// </summary>
        protected float noise = 0.016f;

        /// <summary>
        /// 几次有效运动算一个Event
        /// </summary>
        protected int effect = 3;

        /// <summary>
        /// 多久采样一次是否有超过noise的有效运动,0.1f表示0.1s
        /// </summary>
        protected float samplingTime = 0.05f;

        /// <summary>
        /// 当前的Event状态
        /// </summary>
        public static EventDelegate eventDelegate;

        /// <summary>
        /// 当前的Event状态
        /// </summary>
        public GCEventType currentEvent = GCEventType.Null;
        protected GCEventType previousEvent = GCEventType.Null;

        protected override void DispatchEventDelegate() {
            if(eventDelegate == null || currentEvent == GCEventType.Null) {
                return;
            }

            //DebugMy.Log(handShankPart.PartType + "   DispatchEventDelegate -----> " + currentEvent, this);
            eventDelegate(currentEvent, inputDevicePartDispatchEventGC.inputDeviceGCPart);
        }

        protected override void DispatchEventTarget() {

        }
    }
}
