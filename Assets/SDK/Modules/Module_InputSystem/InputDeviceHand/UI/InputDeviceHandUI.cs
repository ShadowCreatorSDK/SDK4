using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class InputDeviceHandUI : InputDeviceUIBase {


        public virtual void SetActiveUI(HandUIType uitype,bool active) {
            foreach (var item in UIModuleList) {
                IHandUIType handUIType = item as IHandUIType;
                if (handUIType != null && handUIType.UIType == uitype) {
                    if (active) {
                        item.ModuleStart();
                    } else {
                        item.ModuleStop();
                    }
                }
            }
        }

    }
}
