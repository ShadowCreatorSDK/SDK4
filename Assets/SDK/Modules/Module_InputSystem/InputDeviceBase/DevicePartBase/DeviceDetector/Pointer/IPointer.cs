using UnityEngine;
using DG.Tweening;

namespace SC.XR.Unity.Module_InputSystem {
    public interface IPointer {

        /// <summary>
        /// Is the focus for this pointer currently locked?
        /// </summary>
        bool IsFocusLocked { get; set; }

        /// <summary>
        /// The scene query rays.
        /// </summary>
        RayStep Ray { get; }

        PointerType PointerType { get; }

        SCPointEventData ResultData { get; }
    }
}
