using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class InputDeviceHandPartLeftRightEvent : InputDeviceHandPartEventBase {

        public InputDeviceHandPartLeftRightEvent(InputDevicePartDispatchEventHand inputDevicePartDispatchEventHand) : base(inputDevicePartDispatchEventHand) {
        }

        enum XDirection {
            Left = -1,
            Right = 1,
        }

        List<Vector3> HandTrendList = new List<Vector3>(new Vector3[30]);
        int DirectionResult = 0;
        //int DirectionResultL = 0;
        //int DirectionResultR = 0;

        //int DirectionResultY = 0;
        //int DirectionResultZ = 0;

        int currentNum = 0;
        float timer = 0;

        float timeTriggerEvent;

        protected override void OnUpdateEvent() {
            currentEvent = HandEventType.Null;
            // timer += Time.deltaTime;
            if(timer == 0) {
                timer = Time.frameCount;
            }

            if((Time.frameCount - timer) >= samplingTime && (Time.time - timeTriggerEvent) > 0.5f) {
                //timer = 0;
                timer = Time.frameCount;

                if(handInfo.isLost ==false) {
                    HandTrendList[currentNum] = handInfo.finger[(int)(FINGER.forefinger)].joint[(int)JOINT.Four].localPosition;
                } else {
                    HandTrendList[currentNum] = Vector3.zero;
                }

                //int j = 0;
                //DebugMy.Log("trendList * 1000 ===========  " + HandTrendList[currentNum] * 1000 + "  ==========", this);
                //foreach(var trend in HandTrendList) {
                //    if(currentNum == j) {
                //        DebugMy.Log("trendList * 1000:[" + j++ + "]:" + trend * 1000 + "<===", this);
                //    } else {
                //        DebugMy.Log("trendList * 1000:[" + j++ + "]:" + trend * 1000, this);
                //    }
                //}


                DirectionResult = 0;
                for(int i = HandTrendList.Count - 1; i >= 0; i--) {

                    if(HandTrendList[i].x > (HandTrendList[((i - 1) < 0) ? HandTrendList.Count - 1 : i - 1].x + noise)) {
                        DirectionResult += (int)XDirection.Right;
                        //DebugMy.Log("   " + XDirection.Right + "  " + DirectionResult, this);
                    } else if((HandTrendList[i].x + noise) < HandTrendList[((i - 1) < 0) ? HandTrendList.Count - 1 : i - 1].x) {
                        DirectionResult += (int)XDirection.Left;
                        //DebugMy.Log("   " + XDirection.Left + "  " + DirectionResult, this);
                    }
                    //Debug.Log("xxxx:: "+ (HandTrendList[i].x - (HandTrendList[((i - 1) < 0) ? HandTrendList.Count - 1 : i - 1].x)));
                }

                //if(DirectionResultR > (HandTrendList.Count / 3) && DirectionResultL < (HandTrendList.Count / 6)) {
                //    currentEvent = Event.Rigth;
                //}
                //if(DirectionResultL > (HandTrendList.Count / 3) && DirectionResultR < (HandTrendList.Count / 6)) {
                //    currentEvent = Event.Left;
                //}

                ///此log可用于查看noise大小，正常手不动DirectionResult为0，当不为0时，调大 noise
               // Debug.Log("   Noise ----- ----- X: " + DirectionResult+":"+ DirectionResultR+":"+DirectionResultL+":"+ DirectionResultY+":"+ DirectionResultZ);



                if(DirectionResult >= effect) {
                    //DebugMy.Log("   Event ----- ----- Rigth:"+ inputDevice26Dof.PartType, this);
                    currentEvent = HandEventType.Rigth;

                } else if(DirectionResult <= -effect) {
                    // DebugMy.Log("   Event ----- ----- Left:" + inputDevice26Dof.PartType, this);
                    currentEvent = HandEventType.Left;
                }

                if(currentEvent == HandEventType.Null) {
                    currentNum++;
                    if(currentNum == HandTrendList.Count) {
                        currentNum = 0;
                    }
                } else {
                    currentNum = 0;
                    for(int i = 0; i < HandTrendList.Count; i++) {
                        HandTrendList[i] = Vector3.zero;
                    }
                    timeTriggerEvent = Time.time;
                }


            }

        }
    }
}
