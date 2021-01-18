using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    /// <summary>
    /// 表示识别到的手的index，无左右手之分，第一个识别的hand index为0，依次类推
    /// </summary>
    public enum HandIndex {
        Left,
        Right,
    }
}