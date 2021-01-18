using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceGC {

    public class InputDeviceGCPartEventSliderLeftRight : InputDeviceGCPartEventBase {

        InputDevicePartDispatchEventGC inputDevicePartDispatchEventGC;
        public InputDeviceGCPartEventSliderLeftRight(InputDevicePartDispatchEventGC inputDevicePartDispatchEventGC) : base(inputDevicePartDispatchEventGC) {
            this.inputDevicePartDispatchEventGC = inputDevicePartDispatchEventGC;
            noise = 12;
            effect = 3;
            samplingTime = 0.03f;
        }

        enum XDirection {
            Left = -1,
            Right = 1,
        }

        List<Vector2> TouchTrendList = new List<Vector2>(new Vector2[6]);
        int DirectionResult = 0;

        int currentNum = 0;
        float timer = 0;


        protected override void DispatchEventTarget() {

        }
        protected override void OnUpdateEvent() {

            currentEvent = GCEventType.Null;
            timer += Time.deltaTime;

            if(timer >= samplingTime) {
                timer = 0;

                if(inputDevicePartDispatchEventGC.inputDeviceGCPart.inputDataGC.isTpTouch == true) {
                    TouchTrendList[currentNum] = inputDevicePartDispatchEventGC.inputDeviceGCPart.inputDataGC.tpPosition;
                } else {
                    TouchTrendList[currentNum] = Vector2.zero;
                }

                //int j = 0;
                //DebugMy.Log("trendList * 1000 ===========  " + TouchTrendList[currentNum] * 1000 + "  ==========", this);
                //foreach(var trend in TouchTrendList) {
                //    if(currentNum == j) {
                //        DebugMy.Log("trendList * 1000:[" + j++ + "]:" + trend * 1000 + "<===", this);
                //    } else {
                //        DebugMy.Log("trendList * 1000:[" + j++ + "]:" + trend * 1000, this);
                //    }
                //}


                DirectionResult = 0;
                for(int i = TouchTrendList.Count - 1; i >= 0; i--) {

                    if(TouchTrendList[i].x > (TouchTrendList[((i - 1) < 0) ? TouchTrendList.Count - 1 : i - 1].x + noise)) {
                        DirectionResult += (int)XDirection.Right;
                        //DebugMy.Log("   " + XDirection.Right + "  " + DirectionResult, this);
                    } else if((TouchTrendList[i].x + noise) < TouchTrendList[((i - 1) < 0) ? TouchTrendList.Count - 1 : i - 1].x) {
                        DirectionResult += (int)XDirection.Left;
                        //DebugMy.Log("   " + XDirection.Left + "  " + DirectionResult, this);
                    }
                    //Debug.Log("xxxx:: "+ i+"::"+(((i - 1) < 0) ? HandTrendList.Count - 1 : i - 1));
                }


                ///此log可用于查看noise大小，正常手不动DirectionResult为0，当不为0时，调大 noise
                //DebugMy.Log("Noise ----- ----- X---:" + DirectionResult, this);


                if(DirectionResult >= effect) {
                    //DebugMy.Log("Event ----- ----- TouchSildeRight---", this);
                    for(int i = 0; i < TouchTrendList.Count; i++) {
                        TouchTrendList[i] = Vector3.zero;
                    }
                    currentEvent = GCEventType.TouchSlideRight;

                } else if(DirectionResult <= -effect) {
                    //DebugMy.Log("Event ----- ----- TouchSildeLeft ---", this);
                    for(int i = 0; i < TouchTrendList.Count; i++) {
                        TouchTrendList[i] = Vector3.zero;
                    }
                    currentEvent = GCEventType.TouchSlideLeft;
                }

                currentNum++;
                if(currentNum == TouchTrendList.Count) {
                    currentNum = 0;
                }

            }
        }

    }
}
