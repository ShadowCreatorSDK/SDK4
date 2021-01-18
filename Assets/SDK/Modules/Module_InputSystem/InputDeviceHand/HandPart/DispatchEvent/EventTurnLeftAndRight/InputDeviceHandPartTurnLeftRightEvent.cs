using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class InputDeviceHandPartTurnLeftRightEvent : InputDeviceHandPartEventBase {

        public InputDeviceHandPartTurnLeftRightEvent(InputDevicePartDispatchEventHand inputDevicePartDispatchEventHand) : base(inputDevicePartDispatchEventHand) {
        }

        enum TurnAround {
            Left = -1,
            Right = 1,
        }


        List<Vector3> DeltaTrendList = new List<Vector3>(new Vector3[6]);
        int DirectionResult = 0;

        protected int currentNum = 0;
        protected float timer = 0;

        protected override void OnUpdateEvent() {
            currentEvent = HandEventType.Null;
            timer += Time.deltaTime;

            if(timer >= samplingTime) {
                timer = 0;

                if(handInfo.isLost == false) {
                    DeltaTrendList[currentNum] = handInfo.finger[(int)FINGER.ring].joint[(int)JOINT.Four].localPosition - handInfo.finger[(int)FINGER.forefinger].joint[(int)JOINT.Four].localPosition;
                } else {
                    DeltaTrendList[currentNum] = Vector3.zero;
                }


                //int j = 0;
                //DebugMy.Log("trendList * 1000 ===========  " + DeltaTrendList[currentNum] * 1000 + "  ==========", this);
                //foreach(var trend in DeltaTrendList) {
                //    if(currentNum == j) {
                //        DebugMy.Log("trendList * 1000:[" + j++ + "]:" + trend * 1000 + "<===", this);
                //    } else {
                //        DebugMy.Log("trendList * 1000:[" + j++ + "]:" + trend * 1000, this);
                //    }
                //}


                DirectionResult = 0;
                for(int i = DeltaTrendList.Count - 1; i >= 0; i--) {


                    //XDirectionResult
                    if(DeltaTrendList[i].x > (DeltaTrendList[((i - 1) < 0) ? DeltaTrendList.Count - 1 : i - 1].x + noise)) {
                        DirectionResult += (int)TurnAround.Right;
                        DebugMy.Log("trendList :" + TurnAround.Right + "  " + DirectionResult, this);
                    } else if((DeltaTrendList[i].x + noise) < DeltaTrendList[((i - 1) < 0) ? DeltaTrendList.Count - 1 : i - 1].x) {
                        DirectionResult += (int)TurnAround.Left;
                        DebugMy.Log("trendList :" + TurnAround.Left + "    " + DirectionResult, this);
                    }
                    //Debug.Log("xxxx:: "+ i+"::"+(((i - 1) < 0) ? HandTrendList.Count - 1 : i - 1));
                }


                ///此log可用于查看noise大小，正常手不动DirectionResult为0，当不为0时，调大 noise
                //DebugMy.Log("Noise ----- ----- Turn---" + DirectionResult, this);


                if(DirectionResult >= effect) {
                    //DebugMy.Log("Event ----- ----- TurnAround Rigth---", this);
                    for(int i = 0; i < DeltaTrendList.Count; i++) {
                        DeltaTrendList[i] = Vector3.zero;
                    }
                    currentEvent = HandEventType.TurnLeft;

                } else if(DirectionResult <= -effect) {
                    //DebugMy.Log("Event ----- ----- TurnAround Left ---", this);
                    for(int i = 0; i < DeltaTrendList.Count; i++) {
                        DeltaTrendList[i] = Vector3.zero;
                    }
                    currentEvent = HandEventType.TurnRight;
                }

                currentNum++;
                if(currentNum == DeltaTrendList.Count) {
                    currentNum = 0;
                }
            }
        }

    }
}
