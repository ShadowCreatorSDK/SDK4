using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {
    public enum JOINT {
        One = 0,///靠近指尖的关节
        Two,
        Three,
        Four,///靠近手掌的关节（大拇指，食指，中指，无名指）
        Five,///靠近手掌的关节（小拇指）
    }
}