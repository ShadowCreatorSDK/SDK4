using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem {

    public abstract class DispatcherBase : SCModule {

        public InputDevicePartDispatchEventBase inputDevicePartDispatchEventBase;
        public DispatcherBase(InputDevicePartDispatchEventBase inputDevicePartDispatchEventBase) {
            this.inputDevicePartDispatchEventBase = inputDevicePartDispatchEventBase;
        }


        /// <summary>
        /// When Any part Device of Any key Down will invoke this delegate
        /// </summary>
        /// <param name="keyCode">which key</param>
        /// <param name="part">which part,part.PartType</param>
        static event AnyKeyEventDelegate AnyKeyDownDelegate;

        /// <summary>
        /// When Any part Device of Any key Up will invoke this delegate
        /// </summary>
        /// <param name="keyCode">which key</param>
        /// <param name="part">which part,part.PartType</param>
        static event AnyKeyEventDelegate AnyKeyUpDelegate;

        /// <summary>
        /// When Any part Device of Any key Long will invoke this delegate
        /// </summary>
        /// <param name="keyCode">which key</param>
        /// <param name="part">which part,part.PartType</param>
        static event AnyKeyEventDelegate AnyKeyLongDelegate;

        public static void KeyDownDelegateInvoke(InputKeyCode keyCode,InputDevicePartBase inputDevicePart) {
            AnyKeyDownDelegate?.Invoke(keyCode, inputDevicePart);
        }
        public static void KeyUpDelegateInvoke(InputKeyCode keyCode, InputDevicePartBase inputDevicePart) {
            AnyKeyUpDelegate?.Invoke(keyCode, inputDevicePart);
        }
        public static void KeyLongDelegateInvoke(InputKeyCode keyCode, InputDevicePartBase inputDevicePart) {
            AnyKeyLongDelegate?.Invoke(keyCode, inputDevicePart);
        }

        public static void KeyDownDelegateRegister(AnyKeyEventDelegate keyDownEventDelegate) { AnyKeyDownDelegate += keyDownEventDelegate; }
        public static void KeyDownDelegateUnRegister(AnyKeyEventDelegate keyDownEventDelegate) { AnyKeyDownDelegate -= keyDownEventDelegate; }

        public static void KeyUpDelegateRegister(AnyKeyEventDelegate keyUpEventDelegate) { AnyKeyUpDelegate += keyUpEventDelegate; }
        public static void KeyUpDelegateUnRegister(AnyKeyEventDelegate keyUpEventDelegate) { AnyKeyUpDelegate -= keyUpEventDelegate; }

        public static void KeyLongDelegateRegister(AnyKeyEventDelegate keyLongEventDelegate) { AnyKeyLongDelegate += keyLongEventDelegate; }
        public static void KeyLongDelegateUnRegister(AnyKeyEventDelegate keyLongEventDelegate) { AnyKeyLongDelegate -= keyLongEventDelegate; }

    }
}
