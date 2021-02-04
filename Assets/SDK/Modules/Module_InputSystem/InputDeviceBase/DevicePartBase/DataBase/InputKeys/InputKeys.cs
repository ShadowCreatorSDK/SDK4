using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem {
    public class InputKeys : SCModule {

        public InputDataBase inputDataBase;
        public InputKeys(InputDataBase inputDataBase) {
            this.inputDataBase = inputDataBase;
        }

        /// <summary>
        /// Long按键时长
        /// </summary>
        public float LongKeyDurationTime = 3.0f;

        /// <summary>
        /// 按键的实时信息
        /// </summary>
        public Dictionary<InputKeyCode, InputKeyState> inputKeyDic;

        /// <summary>
        /// 按键的按下状态
        /// </summary>
        public Dictionary<InputKeyCode, InputKeyState> inputKeyPressDic = new Dictionary<InputKeyCode, InputKeyState>();

        /// <summary>
        /// 按键的简单处理，获取按下状态，存储于inputKeyPressDic列表
        /// </summary>
        /// 
        Dictionary<InputKeyCode, InputKeyState> inputDataPreviousKeyDic = new Dictionary<InputKeyCode, InputKeyState>();

        InputKeyState state;

        #region Module Behavior

        public override void OnSCAwake() {
            base.OnSCAwake();
            inputKeyDic = new Dictionary<InputKeyCode, InputKeyState>();
            inputKeyPressDic = new Dictionary<InputKeyCode, InputKeyState>();
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            UpdateKeyEvent();
        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            inputKeyDic.Clear();
            inputKeyPressDic.Clear();
            inputDataPreviousKeyDic.Clear();
            state = InputKeyState.Null;
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            inputKeyPressDic = null;
            inputKeyDic = null;
            inputDataPreviousKeyDic = null;
            inputDataBase = null;
        }

        #endregion

        List<InputKeyCode> listKeyCode = new List<InputKeyCode>();

        public void VaildChanged() {
            listKeyCode.Clear();
            foreach(var item in inputKeyDic) {
                if(item.Value == InputKeyState.DOWN || item.Value == InputKeyState.LONG) {
                    listKeyCode.Add(item.Key);
                }
            }
            for(int i = 0; i < listKeyCode.Count; i++) {
                InputDataAddKey(listKeyCode[i], InputKeyState.UP);
            }
            UpdateKeyEvent();
        }
        protected virtual void UpdateKeyEvent() {
            lock(inputKeyDic) {

                //for lower GC
                inputKeyPressDic.Clear();
                foreach (var key in inputKeyDic) {
                    inputKeyPressDic.Add(key.Key, key.Value);
                }

                //foreach(var item in inputKeyPressDic) {
                //    if(item.Value != InputKeyState.Null) {
                //        //DebugMy.Log("inputKeyDic: [" + item.Key + "]:" + "[" + item.Value + "]", this);
                //        DebugMy.Log(" inputKeyDic: [" + item.Key + "]:" + "[" + item.Value + "]",this);
                //    }
                //}

                foreach(var item in inputKeyDic) {
                    inputDataPreviousKeyDic.TryGetValue(item.Key, out state);
                    if(state == item.Value) {
                        inputKeyPressDic[item.Key] = InputKeyState.Null;
                    }
                }

                //for lower GC
                inputDataPreviousKeyDic.Clear();
                foreach (var key in inputKeyDic) {
                    inputDataPreviousKeyDic.Add(key.Key, key.Value);
                }

                //foreach(var item in inputKeyPressDic) {
                //    if(item.Value != InputKeyState.Null) {
                //        //DebugMy.Log("inputKeyPressDic: [" + item.Key + "]:" + "[" + item.Value + "]", this);
                //        DebugMy.Log(" inputKeyPressDic: [" + item.Key + "]:" + "[" + item.Value + "]", this);
                //    }
                //}
            }
        }



        /// <summary>
        /// 向inputKeyDic列表增加按键信息
        /// </summary>
        /// <param name="inputKeyCode"> 按键KeyCode</param>
        /// <param name="inputKeyState"> 按键状态 </param>
        public void InputDataAddKey(InputKeyCode inputKeyCode, InputKeyState inputKeyState) {

            lock(inputKeyDic) {
                DebugMy.Log("InputDataAddKey: " + inputKeyCode + "=" + inputKeyState, this);
                if(!inputKeyDic.ContainsKey(inputKeyCode)) {
                    inputKeyDic.Add(inputKeyCode, InputKeyState.Null);
                }
                inputKeyDic[inputKeyCode] = inputKeyState;

                //UpdateKeyEvent();
            }
        }

        /// <summary>
        /// 获取inputKeyPressDic列表中的某个按键状态
        /// </summary>
        /// <param name="inputKeyCode"> 按键KeyCode </param>
        /// <returns></returns>
        public bool GetKeyDown(InputKeyCode inputKeyCode) {
            InputKeyState inputKeyState;
            inputKeyPressDic.TryGetValue(inputKeyCode, out inputKeyState);
            if(inputKeyState == InputKeyState.DOWN) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取inputKeyPressDic列表中的某个按键状态
        /// </summary>
        /// <param name="inputKeyCode"> 按键KeyCode </param>
        /// <returns></returns>
        public bool GetKeyUp(InputKeyCode inputKeyCode) {
            InputKeyState inputKeyState;
            inputKeyPressDic.TryGetValue(inputKeyCode, out inputKeyState);
            if(inputKeyState == InputKeyState.UP) {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取inputKeyPressDic列表中的某个按键状态
        /// </summary>
        /// <param name="inputKeyCode"> 按键KeyCode </param>
        /// <returns></returns>
        public InputKeyState GetKeyState(InputKeyCode inputKeyCode) {
            InputKeyState inputKeyState;
            inputKeyPressDic.TryGetValue(inputKeyCode, out inputKeyState);
            return inputKeyState;
        }

        /// <summary>
        /// 获取inputKeyDic列表中某个按键状态
        /// </summary>
        /// <param name="inputKeyCode"></param>
        /// <returns></returns>
        public InputKeyState GetKeyCurrentState(InputKeyCode inputKeyCode) {
            InputKeyState inputKeyState;
            inputKeyDic.TryGetValue(inputKeyCode, out inputKeyState);
            return inputKeyState;
        }


    }

}
