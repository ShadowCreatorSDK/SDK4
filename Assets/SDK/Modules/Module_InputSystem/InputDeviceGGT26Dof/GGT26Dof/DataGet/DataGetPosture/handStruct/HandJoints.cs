using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {

    public enum HandJoint {
        /// 手腕
        Wrist = 0,
        /// 手掌
        Palm,

        // << 姆指 >>
        /// 拇指尖
        ThumbTip,
        /// 拇指远关节，拇指的第一个（最远的）关节。
        ThumbDistalJoint,
        /// 拇指近关节，拇指的第二个关节
        ThumbProximalJoint,
        /// 拇指掌关节，拇指最低的关节（在你的手掌下面）
        ThumbMetacarpalJoint,


        // << 食指 >>
        /// 食指尖
        IndexTip,
        /// 食指远关节
        IndexDistalJoint,
        /// 食指中间关节
        IndexMiddleJoint,
        /// 食指的关节，从进到远第二节
        IndexKnuckle,
        /// 食指掌关节
        IndexMetacarpal,


        // << 中指 >>
        MiddleTip,
        MiddleDistalJoint,
        MiddleMiddleJoint,
        MiddleKnuckle,
        MiddleMetacarpal,


        // << 无名指 >>
        RingTip,
        RingDistalJoint,
        RingMiddleJoint,
        RingKnuckle,
        RingMetacarpal,


        // << 小拇指 >>
        PinkyTip,
        PinkyDistalJoint,
        PinkyMiddleJoint,
        PinkyKnuckle,
        PinkyMetacarpal
    }


}