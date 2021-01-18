using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class InputDeviceHandPartCatchEvent : InputDeviceHandPartEventBase {

        public InputDeviceHandPartCatchEvent(InputDevicePartDispatchEventHand inputDevicePartDispatchEventHand) : base(inputDevicePartDispatchEventHand) {
        }

        enum XDirection {
            Release = -1,
            Catch = 1,
        }


        List<Vector3> HandTrendList = new List<Vector3>(new Vector3[6]);

        protected int currentNum = 0;
        protected float timer = 0;


        bool isForefingerCatch = false;
        bool isDistanceNear = false;

        protected override void OnUpdateEvent() {
            float angle;
            float distance = 0;
            //angle = Vector3.Angle(
            //    (inputDevice26Dof.handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Two].localPosition - inputDevice26Dof.handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Three].localPosition),
            //    (inputDevice26Dof.handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Three].localPosition - inputDevice26Dof.handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Four].localPosition));
            //if (angle > 20) {
            //    isThumbCatch = true;
            //} else if (angle < 18) {
            //    isThumbCatch = false;
            //}
            //DebugMy.Log(FINGER.thumb + "  Angle :" + angle + ">20 ?", this,true);


            angle = Vector3.Angle(
                (handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Two].localPosition - handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Three].localPosition),
                (handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Three].localPosition - handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Four].localPosition));
            if(angle > 28) {
                isForefingerCatch = true;
            } else if(angle < 20) {
                isForefingerCatch = false;
            }
            //DebugMy.Log(inputDevice26Dof.PartType+"   "+FINGER.forefinger + "  Angle :" + angle + ">28 ?", this);

            distance = Vector3.Distance(handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.One].localPosition, handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.One].localPosition);
            if(distance < 0.035f) {
                isDistanceNear = true;
            } else if(distance < 0.050f) {
                isDistanceNear = false;
            }
            if(isDistanceNear == false) {
                distance = Vector3.Distance(handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Two].localPosition, handInfo.finger[(int)FINGER.thumb].joint[(int)JOINT.Two].localPosition);
                if(distance < 0.035f) {
                    isDistanceNear = true;
                }
            }
            //DebugMy.Log(inputDevice26Dof.PartType + "   " + "distance:" + distance, this);
            //angle = Vector3.Angle(
            //    (inputDevice26Dof.handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Two].localPosition - inputDevice26Dof.handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Three].localPosition),
            //    (inputDevice26Dof.handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Three].localPosition - inputDevice26Dof.handInfo.finger[(int)FINGER.middle].joint[(int)JOINT.Four].localPosition));
            //if (angle > 40) {
            //    isMiddleCatch = true;
            //} else if (angle < 25) {
            //    isMiddleCatch = false;
            //}
            //DebugMy.Log(FINGER.middle + "  Angle :" + angle + ">25 ?", this);



            //angle = Vector3.Angle(
            //    (inputDevice26Dof.handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Two].localPosition - inputDevice26Dof.handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Three].localPosition),
            //    (inputDevice26Dof.handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Three].localPosition - inputDevice26Dof.handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Four].localPosition));
            //if (angle > 40) {
            //    isRingCatch = true;
            //} else if (angle < 25) {
            //    isRingCatch = false;
            //}
            //DebugMy.Log(FINGER.ring + "  Angle :" + angle + ">25 ?", this);

            if(isDistanceNear && isForefingerCatch) {

                if(previousEvent == HandEventType.CatchDown || previousEvent == HandEventType.CatchDrag) {
                    currentEvent = HandEventType.CatchDrag;
                } else {
                    currentEvent = HandEventType.CatchDown;
                }

            } else {

                if(previousEvent == HandEventType.CatchDown || previousEvent == HandEventType.CatchDrag) {
                    currentEvent = HandEventType.CatchUp;
                } else {
                    currentEvent = HandEventType.Null;
                }

            }
            previousEvent = currentEvent;

            //if(currentEvent != HandEventType.Null) {
            //    DebugMy.Log(inputDeviceHandPart.PartType + "   Event -----> " + currentEvent, this);
            //}

        }


    }
}
