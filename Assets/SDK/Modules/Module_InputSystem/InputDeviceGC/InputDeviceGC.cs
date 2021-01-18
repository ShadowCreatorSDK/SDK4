using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public abstract class InputDeviceGC : InputDeviceBase {

        /// <summary>
        /// 编辑器模式是否打开HandShank,对于非编辑器模式不影响
        /// </summary>
        [Header("Is Simulate In Editor Mode")]
        public bool SimulateInEditorMode = false;

    }
}
