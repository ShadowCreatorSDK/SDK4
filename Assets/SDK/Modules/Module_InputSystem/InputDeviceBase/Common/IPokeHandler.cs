using SC.XR.Unity.Module_InputSystem.InputDeviceHand;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {

    /// <summary>
    /// PokeHandler
    /// </summary>
    public interface IPokeHandler: IPokeDownHandler, IPokeUpdatedHandler, IPokeUpHandler {
    }

}
