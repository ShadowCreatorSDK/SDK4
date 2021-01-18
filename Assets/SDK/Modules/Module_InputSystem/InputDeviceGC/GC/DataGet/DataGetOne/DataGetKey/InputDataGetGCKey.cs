using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {
    public class InputDataGetGCKey : InputDataGetKey {

        public InputDataGetGC inputDataGetGC;


        /// <summary>
        /// Enter别名，默认Trigger触发Enter键
        /// </summary>
        public InputKeyCode EnterKeyAlias = InputKeyCode.Trigger;

        public InputKeyCode CancelKeyAlias = InputKeyCode.OTHER;

        public InputDataGetGCKey(InputDataGetGC _inputDataGetGC) : base(_inputDataGetGC) {
            inputDataGetGC = _inputDataGetGC;
        }

        public override void OnUpdateKey() {
            if(Application.platform == RuntimePlatform.WindowsEditor) {
                base.OnUpdateKey();
            }
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            inputDataGetGC = null;
        }


    }
}
