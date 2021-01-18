using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace SC.XR.Unity.Module_InputSystem.InputDeviceHand {

    public class handInfo {


        public handInfo(string configPath) {
            if(Application.platform == RuntimePlatform.Android) {
                try {
                    CommonConfig config;

                    config = new CommonConfig(configPath);

                    float x = 0, y = 0, z = 0;
                    string xx = config.GetLineValue(1);
                    x = float.Parse(xx != null ? xx : "0");
                    xx = config.GetLineValue(2);
                    y = float.Parse(xx != null ? xx : "0");
                    xx = config.GetLineValue(3);
                    z = float.Parse(xx != null ? xx : "0");

                    if(x > -10 && x < 10 && y > -10 && y < 10 && z > -10 && z < 10) {
                        positionOffest = new Vector3(x, y, z);
                        DebugMy.Log("Read "+ configPath + "" + x + "  " + y + "  " + z,this,true);
                    }
                    config = null;
                } catch(Exception e) {
                    Debug.Log(e);
                }

                Vector3 data = Vector3.zero;
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "GreyHandOffsetX")) {
                    data.x = API_Module_SDKConfiguration.GetFloat("Module_InputSystem", "GreyHandOffsetX", 0);
                    data.x = (data.x > -10 && data.x < 10) ? data.x : 0;
                }
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "GreyHandOffsetY")) {
                    data.y = API_Module_SDKConfiguration.GetFloat("Module_InputSystem", "GreyHandOffsetY", 0);
                    data.y = (data.y > -10 && data.y < 10) ? data.y : 0;
                }
                if (API_Module_SDKConfiguration.HasKey("Module_InputSystem", "GreyHandOffsetZ")) {
                    data.z = API_Module_SDKConfiguration.GetFloat("Module_InputSystem", "GreyHandOffsetZ", 0);
                    data.z = (data.z > -10 && data.z < 10) ? data.z : 0;
                }

                positionOffest = data;
                DebugMy.Log("ReadFromConfig positionOffest: " + "X:" + data.x + "  Y:" + data.y + "  Z:" + data.z, this, true);



            }
        }

        public Quaternion rotation =Quaternion.identity;
        public Vector3 position = Vector3.zero;

        public Vector3 normal = Vector3.zero;//只能自算，手掌中法向量
        public Vector3 right = Vector3.zero;//只能自算，手掌right向量

        public Vector3 centerLocalPosition = Vector3.zero;//手掌中心点局部坐标
        public Vector3 centerPosition = Vector3.zero;//手掌中心点全局坐标 


        public float zdeep = 0;
        public int findFrameCount = 0;//识别开始后共多少帧，丢失即复位
        public int frameCountValid = 10;//丢失后重新识别到frameCountValid帧连续有效才算识别

        protected Vector3 deltaOffset=Vector3.zero;
        public virtual Vector3 positionOffest {
            get {
                return deltaOffset;//new Vector3(0.05f, 0.00f, 0.14f) + new Vector3(-0.025f, 0.00f, -0.14f) + Vector3.forward * zdeep + deltaOffset;//摄像头向下15度  //new Vector3(0, 0, 0.25f);z正常机器// new Vector3(0.280f,-0.10f,0f);
            }
            set {
                deltaOffset = value;
            }
        }


        private Vector3 _eulerAnglesOffset = new Vector3(-11,0,0);
        public Vector3 eulerAnglesOffset {
            get {
                //if(ShadowSystem.Instant && ShadowSystem.Instant.Device) {
                //    return ShadowSystem.Instant.Device.CurrentDevice.RGBRotationOffset;
                //}
                if(Application.platform == RuntimePlatform.Android) {
                    return _eulerAnglesOffset;
                }
                return Vector3.zero;
            }
        }

        public Vector3 localPosition = Vector3.zero;
        public bool isLost = false;//是否丢失手
        public float lostPercent = 0;//丢失的比例  0 - 1 范围
        public float lostTimer = 0.8f;//多久没识别到算丢失

        public Vector3 trend = Vector3.zero;//手势的趋势  食指指尖

        //[MarshalAs(UnmanagedType.ByValArray, SizeConst = 21, ArraySubType = UnmanagedType.Struct)]
        //public Vector3[] fingerPos = new Vector3[21];
        public fingerInfo[] finger = new fingerInfo[5] {
            new fingerInfo(){
                joint = new jointInfo[4]{
                    new jointInfo(),new jointInfo(),new jointInfo(),new jointInfo(),
                }
            },
            new fingerInfo(){
                joint = new jointInfo[4]{
                    new jointInfo(),new jointInfo(),new jointInfo(),new jointInfo(),
                }
            },
            new fingerInfo(){
                joint = new jointInfo[4]{
                    new jointInfo(),new jointInfo(),new jointInfo(),new jointInfo(),
                }
            },
            new fingerInfo(){
                joint = new jointInfo[4]{
                    new jointInfo(),new jointInfo(),new jointInfo(),new jointInfo(),
                }
            },
            new fingerInfo(){
                joint = new jointInfo[5]{
                    new jointInfo(),new jointInfo(),new jointInfo(),new jointInfo(),new jointInfo(),
                }
            },

            };

    }



}







