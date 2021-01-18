using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public enum InputKeyCode {
        /// <summary>
        /// 基本按键
        /// </summary>
        Enter,
        Cancel,

        /// <summary>
        /// 手柄按键
        /// </summary>
        Trigger,
        Function,
        Back,
        Tp,
        VolumeDown,
        VolumeUp,

        A,
        B,
        LjoystickKey,
        LFunction,
        LHallInside,
        LHallForward,
        LTrigger,

        X,
        Y,
        RjoystickKey,
        RFunction,
        RHallInside,
        RHallForward, 
        RTrigger,

        UP,
        DOWN,
        RIGHT,
        LEFT,

        OTHER = -1
    }

    public enum InputKeyState {
        Null = 0,
        UP,
        DOWN,
        LONG
    }

}
