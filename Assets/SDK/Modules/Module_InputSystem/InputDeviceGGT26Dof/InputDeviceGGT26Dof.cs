using AOT;
using SC.XR.Unity.Module_InputSystem.InputDeviceGC;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand.GGT26Dof {
    public class InputDeviceGGT26Dof : InputDeviceHand {


        public override InputDeviceType inputDeviceType {
            get {
                return InputDeviceType.GGT26Dof;
            }
        }

        [HideInInspector]
        public float LowPowerPercent = 15;
        private Coroutine lowPowerCoroutine=null;

        protected delegate void LowPowerWarningDelegate(int power);


        [DllImport("svrplugin")]
        protected static extern void ScHANDTRACK_Start(LowPowerWarningDelegate func);

        [DllImport("svrplugin")]
        public static extern void ScHANDTRACK_Stop();

        [DllImport("svrplugin")]
        private static extern void ScHANDTRACK_SetGestureCallback(GestureChangeDelegate func);
        [DllImport("svrplugin")]
        private static extern int ScHANDTRACK_GetGesture(float[] model, float[] pose);

        [DllImport("svrplugin")]
        private static extern int ScHANDTRACK_SetGestureData(float[] model, float[] pose);

        [DllImport("svrplugin")]
        private static extern int ScHANDTRACK_GetGestureWithIdx(ref UInt64 index, float[] model, float[] pose);

        internal delegate void GestureModelDataChangeDelegate();

        [DllImport("svrplugin")]
        private static extern int ScHANDTRACK_SetGestureModelDataCallback(GestureModelDataChangeDelegate func);
        bool isHandTrackStart = false;
        Coroutine startHand;

        [MonoPInvokeCallback(typeof(GestureModelDataChangeDelegate))]
        public static void GestureModelDataChangeCallback()
        {
            //Debug.Log("[04]GestureModelDataChangeCallback start");    

            //InputDeviceGGT26DofPart inputDeviceGGT26DofPart = (inputDevicePartList[0] as InputDeviceGGT26DofPart);
            //if (inputDeviceGGT26DofPart != null) {
            //    inputDeviceGGT26DofPart.inputDataGetGGT26Dof.inputDataGetHandsData.OnUpdateInputDataAndStore();
            //}
        }
        public override void OnSCAwake() {
            base.OnSCAwake();
            AndroidPermission.getInstant.GetPermission(false, true, true);
        }
        protected override void InputDeviceStart() {


            if (DeviceInfo.BatteryLevel < LowPowerPercent) {
                if (lowPowerCoroutine == null) {
                    lowPowerCoroutine = StartCoroutine(LowPowerFunction());
                }
                return;
            }

            base.InputDeviceStart();
        }
        public override void OnSCUpdate() {
            base.OnSCUpdate();

            if (Application.platform != RuntimePlatform.Android) {
                HANDTRACK_GetHand_PC(out InputDataGGT26Dof.handsInfo.originDataMode, out InputDataGGT26Dof.handsInfo.originDataPose);
            }
        }

        public override void OnSCLateUpdate() {
            base.OnSCLateUpdate();
            if(lowPowerTrigger) {
                lowPowerTrigger = false;
                if (lowPowerCoroutine == null) {
                    lowPowerCoroutine = StartCoroutine(LowPowerFunction());
                }
            }
        }

        IEnumerator LowPowerFunction(float loopTime = 3) {
            while (loopTime -- >0) {
                if (inputDeviceUI as InputDeviceHandUI) {
                    (inputDeviceUI as InputDeviceHandUI).SetActiveUI(HandUIType.LOWPOWER, true);
                }
                SetActiveInputDevicePart(InputDevicePartType.HandLeft, false);
                SetActiveInputDevicePart(InputDevicePartType.HandRight, false);
                yield return null;
                yield return new WaitForSeconds(12);
            }
            lowPowerCoroutine = null;
        }

        internal delegate void GestureChangeDelegate(int gesture);

        IEnumerator StartGreyHand() {
            yield return new WaitUntil(() => SvrManager.Instance != null);
            yield return new WaitUntil(() => SvrManager.Instance.IsRunning==true);
            if (isHandTrackStart == false) {
                isHandTrackStart = true;
                DebugMy.Log("ScHANDTRACK_Start", this, true);

                if (Application.platform == RuntimePlatform.Android) {
                    try {
                        ScHANDTRACK_Start(LowPowerWarningCallback);
                        ScHANDTRACK_SetGestureCallback(GestureChangeCallback);
                        ScHANDTRACK_SetGestureData(InputDataGGT26Dof.handsInfo.originDataMode, InputDataGGT26Dof.handsInfo.originDataPose);
                        ScHANDTRACK_SetGestureModelDataCallback(GestureModelDataChangeCallback);
                    } catch (Exception e) {
                        Debug.Log(e);
                    }
                }
            }
            startHand = null;

        }

        public override void OnSCStart() {
            base.OnSCStart();
            if (startHand == null) {
                startHand = StartCoroutine(StartGreyHand());
            }
        }

        public override void OnSCDisable() {
            base.OnSCDisable();
            if (startHand!=null) {
                StopCoroutine(startHand);
                startHand = null;
            }
            if (isHandTrackStart) {
                isHandTrackStart = false;
                DebugMy.Log("ScHANDTRACK_Stop", this, true);
                if (Application.platform == RuntimePlatform.Android) {
                    try {
                        ScHANDTRACK_Stop();
                    } catch (Exception e) {
                        Debug.Log(e);
                    }
                }
            }

        }

        //{1, "THUMB"},
        //{2, "ONE"},
        //{3, "TWO"},
        //{4, "THREE"},
        //{5, "FOUR"},
        //{6, "FIVE"},
        //{7, "OK"},
        //{8, "DIRECTION"},
        [MonoPInvokeCallback(typeof(GestureChangeDelegate))]
        public static void GestureChangeCallback(int gesture) {
            Debug.Log("GestureChangeCallback, gesture id:" + gesture);
        }

        static bool lowPowerTrigger = false;
        [MonoPInvokeCallback(typeof(LowPowerWarningDelegate))]
        public static void LowPowerWarningCallback(int power) {
            Debug.Log("HandGesture Cannot work in low power state:" + power);
            lowPowerTrigger = true;
        }

        

        protected virtual int HANDTRACK_GetHand_PC(out float[] model, out float[] pose) {

            pose = new float[3] { 0, 0, 0 };
            #region PCgesture
            if (Input.GetKey(KeyCode.Alpha1) == true && Input.GetKey(KeyCode.Alpha2) == false) {
                ///左手抓取手型数据
                model = new float[256] {
                    2f,

                    1f,
                    21f,
                    -0.05605574f  ,  -0.0273f  ,  0.314f,
                    -0.07f  ,  -0.0447f  ,  0.3f ,
                    -0.09515104f  , -0.05433898f  ,  0.2831757f,
                    -0.1463438f  ,  -0.06028023f  ,  0.2818089f,

                    -0.0519f  ,  -0.0216f  ,  0.3217f,
                    -0.06158409f  ,  -0.0002f  , 0.3183799f,
                    -0.07964747f  ,  0.0028f  ,  0.3071041f,
                    -0.09945723f  ,  -0.008320909f  ,  0.2969252f,

                    -0.06198902f  ,  -0.0038f  , 0.3519928f ,
                    -0.07581183f  ,  0.0061f  ,  0.3419357f ,
                    -0.09352214f  ,  0.0082f  ,  0.3285346f ,
                    -0.1137585f  ,  -0.002121262f  ,  0.3150989f,

                    -0.07718303f  ,  -0.0063f  ,  0.364556f,
                    -0.08849367f  ,  0.0018f  ,  0.3556355f,
                    -0.1056515f  ,  0.0004f  ,  0.3442511f,
                    -0.1236217f  ,  -0.008046876f  ,  0.331872f,

                    -0.09772705f  ,  -0.0135f  ,  0.3714658f,
                    -0.1064803f  ,  -0.0118f  ,  0.3659644f,
                    -0.1201454f  ,  -0.0166f  ,  0.3570033f ,
                    -0.1347919f  ,  -0.02225832f  ,  0.3441104f,
                    -0.1562702f  ,  -0.05975028f  ,  0.2943733f ,

                    2f,
                    21f,
                    0.01329147f  ,  -0.05185008f  ,  0.2831516f,
                    0.02629676f  ,  -0.05859572f  ,  0.2733593f ,
                    0.04144813f  ,  -0.06611566f  ,  0.2669144f,
                    0.09289996f  ,  -0.07803007f  ,  0.2608919f,

                    0.008512383f  ,  0.008053687f  ,  0.2999706f,
                    0.0184048f  ,  0.004125214f  ,  0.2896651f ,
                    0.0309000f  ,  -0.00680000f  ,  0.27900000f ,
                    0.05151144f  ,  -0.02237571f  ,  0.2674141f,

                    0.02381282f  ,  0.02699578f  ,  0.3218553f,
                    0.03562622f  ,  0.01996369f  ,  0.3106135f,
                    0.05122653f  ,  0.008665001f  ,  0.2939393f,
                    0.06585947f  ,  -0.01085604f  ,  0.2825926f,

                    0.04237749f  ,  0.01809751f  ,  0.3312246f ,
                    0.05178002f  ,  0.01224715f  ,  0.3221904f,
                    0.0659532f  ,  0.003831701f  ,  0.3083498f,
                    0.07914048f  ,  -0.01247946f  ,  0.296542f,

                    0.06218155f  ,  0.002945414f  ,  0.3410454f ,
                    0.07016665f  ,  -0.002003442f  ,  0.3339252f,
                    0.08204032f  ,  -0.01003692f  ,  0.322701f,
                    0.09209882f  ,  -0.02350484f  ,  0.3101943f ,
                    0.1055621f  ,  -0.07391939f  ,  0.2699004f ,

                    0,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,

                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0,0,0
            };
            } else if ((Input.GetKey(KeyCode.Alpha1) == false && Input.GetKey(KeyCode.Alpha2)) == true) {
                ///右手手抓取手型数据
                model = new float[256] {
                    2f,

                    1f,
                    21f,
                    -0.05605574f  ,  -0.04518472f  ,  0.2970701f,
                    -0.07650252f  ,  -0.04899639f  ,  0.2892286f ,
                    -0.09515104f  , -0.05433898f  ,  0.2831757f,
                    -0.1463438f  ,  -0.06028023f  ,  0.2818089f,

                    -0.04933745f  ,  0.01041036f  ,  0.3268114f,
                    -0.06158409f  ,  0.009036704f  ,  0.3183799f,
                    -0.07964747f  ,  0.0015f  ,  0.3071041f,
                    -0.09945723f  ,  -0.008320909f  ,  0.2969252f,

                    -0.06198902f  ,  0.02461821f  ,  0.3519928f ,
                    -0.07581183f  ,  0.02041251f  ,  0.3419357f ,
                    -0.09352214f  ,  0.01374155f  ,  0.3285346f ,
                    -0.1137585f  ,  -0.002121262f  ,  0.3150989f,

                    -0.07718303f  ,  0.01460867f  ,  0.364556f,
                    -0.08849367f  ,  0.01155128f  ,  0.3556355f,
                    -0.1056515f  ,  0.005635553f  ,  0.3442511f,
                    -0.1236217f  ,  -0.008046876f  ,  0.331872f,

                    -0.09772705f  ,  -0.003397862f  ,  0.3714658f,
                    -0.1064803f  ,  -0.005923742f  ,  0.3659644f,
                    -0.1201454f  ,  -0.01133183f  ,  0.3570033f ,
                    -0.1347919f  ,  -0.02225832f  ,  0.3441104f,
                    -0.1562702f  ,  -0.05975028f  ,  0.2943733f ,

                    2f,
                    21f,
                    0.0159f  ,  -0.0299f  ,  0.2831516f,
                    0.0348f  ,  -0.0511f  ,  0.2733593f ,
                    0.0491f  ,  -0.0602f  ,  0.2669144f,
                    0.09289996f  ,  -0.07803007f  ,  0.2608919f,

                    0.0085f  ,  -0.0223f  ,  0.2917f,
                    0.0184f  ,  -0.0117f  ,  0.28966f ,
                    0.0338265f  ,  -0.0106f  ,  0.2750752f ,
                    0.05151144f  ,  -0.02237571f  ,  0.2674141f,

                    0.02381282f  ,  -0.005f  ,  0.3218553f,
                    0.03562622f  ,  0.0052f  ,  0.3106135f,
                    0.05122653f  ,  0.0032f  ,  0.2939393f,
                    0.06585947f  ,  -0.01085604f  ,  0.2825926f,

                    0.04237749f  ,  0.002f  ,  0.3312246f ,
                    0.05178002f  ,  0.0005f  ,  0.3221904f,
                    0.0659532f  ,  -0.002f  ,  0.3083498f,
                    0.07914048f  ,  -0.01247946f  ,  0.296542f,

                    0.06218155f  ,  0.002945414f  ,  0.3410454f ,
                    0.07016665f  ,  -0.002003442f  ,  0.3339252f,
                    0.08204032f  ,  -0.01003692f  ,  0.322701f,
                    0.09209882f  ,  -0.02350484f  ,  0.3101943f ,
                    0.1055621f  ,  -0.07391939f  ,  0.2699004f ,

                    0,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,

                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0,0,0
            };
            } else if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Alpha1) == true && Input.GetKey(KeyCode.Alpha2) == true) {
                ///双手抓取手型数据
                model = new float[256] {
                    2f,

                    1f,
                    21f,
                    -0.05605574f  ,  -0.0273f  ,  0.314f,
                    -0.07f  ,  -0.0447f  ,  0.3f ,
                    -0.09515104f  , -0.05433898f  ,  0.2831757f,
                    -0.1463438f  ,  -0.06028023f  ,  0.2818089f,

                    -0.0519f  ,  -0.0216f  ,  0.3217f,
                    -0.06158409f  ,  -0.0002f  , 0.3183799f,
                    -0.07964747f  ,  0.0028f  ,  0.3071041f,
                    -0.09945723f  ,  -0.008320909f  ,  0.2969252f,

                    -0.06198902f  ,  -0.0038f  , 0.3519928f ,
                    -0.07581183f  ,  0.0061f  ,  0.3419357f ,
                    -0.09352214f  ,  0.0082f  ,  0.3285346f ,
                    -0.1137585f  ,  -0.002121262f  ,  0.3150989f,

                    -0.07718303f  ,  -0.0063f  ,  0.364556f,
                    -0.08849367f  ,  0.0018f  ,  0.3556355f,
                    -0.1056515f  ,  0.0004f  ,  0.3442511f,
                    -0.1236217f  ,  -0.008046876f  ,  0.331872f,

                    -0.09772705f  ,  -0.0135f  ,  0.3714658f,
                    -0.1064803f  ,  -0.0118f  ,  0.3659644f,
                    -0.1201454f  ,  -0.0166f  ,  0.3570033f ,
                    -0.1347919f  ,  -0.02225832f  ,  0.3441104f,
                    -0.1562702f  ,  -0.05975028f  ,  0.2943733f ,

                    2f,
                    21f,
                    0.0159f  ,  -0.0299f  ,  0.2831516f,
                    0.0348f  ,  -0.0511f  ,  0.2733593f ,
                    0.0491f  ,  -0.0602f  ,  0.2669144f,
                    0.09289996f  ,  -0.07803007f  ,  0.2608919f,

                    0.0085f  ,  -0.0223f  ,  0.2917f,
                    0.0184f  ,  -0.0117f  ,  0.28966f ,
                    0.0338265f  ,  -0.0106f  ,  0.2750752f ,
                    0.05151144f  ,  -0.02237571f  ,  0.2674141f,

                    0.02381282f  ,  -0.005f  ,  0.3218553f,
                    0.03562622f  ,  0.0052f  ,  0.3106135f,
                    0.05122653f  ,  0.0032f  ,  0.2939393f,
                    0.06585947f  ,  -0.01085604f  ,  0.2825926f,

                    0.04237749f  ,  0.002f  ,  0.3312246f ,
                    0.05178002f  ,  0.0005f  ,  0.3221904f,
                    0.0659532f  ,  -0.002f  ,  0.3083498f,
                    0.07914048f  ,  -0.01247946f  ,  0.296542f,

                    0.06218155f  ,  0.002945414f  ,  0.3410454f ,
                    0.07016665f  ,  -0.002003442f  ,  0.3339252f,
                    0.08204032f  ,  -0.01003692f  ,  0.322701f,
                    0.09209882f  ,  -0.02350484f  ,  0.3101943f ,
                    0.1055621f  ,  -0.07391939f  ,  0.2699004f ,

                    0,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,

                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0,0,0
            };
            } else {
                ///正常张开手型数据
                model = new float[256] {
                    2f,

                    1f,
                    21f,
                    -0.05605574f  ,  -0.04518472f  ,  0.2970701f,
                    -0.07650252f  ,  -0.04899639f  ,  0.2892286f ,
                    -0.09515104f  , -0.05433898f  ,  0.2831757f,
                    -0.1463438f  ,  -0.06028023f  ,  0.2818089f,

                    -0.04933745f  ,  0.01041036f  ,  0.3268114f,
                    -0.06158409f  ,  0.009036704f  ,  0.3183799f,
                    -0.07964747f  ,  0.0015f  ,  0.3071041f,
                    -0.09945723f  ,  -0.008320909f  ,  0.2969252f,

                    -0.06198902f  ,  0.02461821f  ,  0.3519928f ,
                    -0.07581183f  ,  0.02041251f  ,  0.3419357f ,
                    -0.09352214f  ,  0.01374155f  ,  0.3285346f ,
                    -0.1137585f  ,  -0.002121262f  ,  0.3150989f,

                    -0.07718303f  ,  0.01460867f  ,  0.364556f,
                    -0.08849367f  ,  0.01155128f  ,  0.3556355f,
                    -0.1056515f  ,  0.005635553f  ,  0.3442511f,
                    -0.1236217f  ,  -0.008046876f  ,  0.331872f,

                    -0.09772705f  ,  -0.003397862f  ,  0.3714658f,
                    -0.1064803f  ,  -0.005923742f  ,  0.3659644f,
                    -0.1201454f  ,  -0.01133183f  ,  0.3570033f ,
                    -0.1347919f  ,  -0.02225832f  ,  0.3441104f,
                    -0.1562702f  ,  -0.05975028f  ,  0.2943733f ,

                    2f,
                    21f,
                    0.01329147f  ,  -0.05185008f  ,  0.2831516f,
                    0.02629676f  ,  -0.05859572f  ,  0.2733593f ,
                    0.04144813f  ,  -0.06611566f  ,  0.2669144f,
                    0.09289996f  ,  -0.07803007f  ,  0.2608919f,

                    0.008512383f  ,  0.008053687f  ,  0.2999706f,
                    0.0184048f  ,  0.004125214f  ,  0.2896651f ,
                    0.0309000f  ,  -0.00680000f  ,  0.27900000f ,
                    0.05151144f  ,  -0.02237571f  ,  0.2674141f,

                    0.02381282f  ,  0.02699578f  ,  0.3218553f,
                    0.03562622f  ,  0.01996369f  ,  0.3106135f,
                    0.05122653f  ,  0.008665001f  ,  0.2939393f,
                    0.06585947f  ,  -0.01085604f  ,  0.2825926f,

                    0.04237749f  ,  0.01809751f  ,  0.3312246f ,
                    0.05178002f  ,  0.01224715f  ,  0.3221904f,
                    0.0659532f  ,  0.003831701f  ,  0.3083498f,
                    0.07914048f  ,  -0.01247946f  ,  0.296542f,

                    0.06218155f  ,  0.002945414f  ,  0.3410454f ,
                    0.07016665f  ,  -0.002003442f  ,  0.3339252f,
                    0.08204032f  ,  -0.01003692f  ,  0.322701f,
                    0.09209882f  ,  -0.02350484f  ,  0.3101943f ,
                    0.1055621f  ,  -0.07391939f  ,  0.2699004f ,

                    0,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,

                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,0 ,
                    0,0,0,0
            };

            }

            #endregion
            //if(Input.GetKey(KeyCode.W) == true) {
            //    temp +=Vector3.forward* Time.deltaTime * 0.1f;
            //}
            //if(Input.GetKey(KeyCode.A) == true) {
            //    temp += Vector3.left * Time.deltaTime * 0.1f;
            //}
            //if(Input.GetKey(KeyCode.D) == true) {
            //    temp += Vector3.right * Time.deltaTime * 0.1f;
            //}
            //if(Input.GetKey(KeyCode.S) == true) {
            //    temp += Vector3.back * Time.deltaTime * 0.1f;
            //}

            //pose[0] = temp.x;
            //pose[1] = temp.y;
            //pose[2] = temp.z;

            return 0;
        }

    }
}
