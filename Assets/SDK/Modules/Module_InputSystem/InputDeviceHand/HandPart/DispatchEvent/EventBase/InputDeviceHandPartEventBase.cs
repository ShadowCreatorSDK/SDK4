using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public abstract class InputDeviceHandPartEventBase : InputDevicePartEventBase {

        public InputDevicePartDispatchEventHand inputDevicePartDispatchEventHand;
        
        public InputDeviceHandPartEventBase(InputDevicePartDispatchEventHand inputDevicePartDispatchEventHand) : base(inputDevicePartDispatchEventHand) {
            this.inputDevicePartDispatchEventHand = inputDevicePartDispatchEventHand;
            inputDeviceHandPart = this.inputDevicePartDispatchEventHand.inputDeviceHandPart;
            handInfo = this.inputDevicePartDispatchEventHand.inputDeviceHandPart.inputDataHand.handInfo;
        }

        protected handInfo handInfo;
        protected InputDeviceHandPart inputDeviceHandPart;

        ///-----------------------------算法使用的参数-----start
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
        ///-----------------------------算法使用的参数-----End


        /// <summary>
        /// 当前的Event状态
        /// </summary>
        public static HandEventDelegate eventDelegate;

        public HandEventType currentEvent = HandEventType.Null;
        protected HandEventType previousEvent = HandEventType.Null;

        /// <summary>
        /// 模板模式
        /// </summary>
        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
           
            OnUpdateEvent();
            
            DispatchEventDelegate();

            DispatchEventTarget();
        }


        /// <summary>
        /// 派发Event到委托
        /// </summary>
        protected override void DispatchEventDelegate() {
            if(eventDelegate == null || currentEvent == HandEventType.Null) {
                return;
            }

            //DebugMy.Log(inputDeviceHandPart.PartType+"   DispatchEventDelegate -----> " + currentEvent , this);
            eventDelegate(inputDeviceHandPart, currentEvent);
        }

        protected override void DispatchEventTarget() {
           
        }

    }
}