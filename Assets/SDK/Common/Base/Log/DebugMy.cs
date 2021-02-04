using System;
using UnityEngine;

namespace SC.XR.Unity {
    public class DebugMy {

        static string Tag ;
        public static bool isShowNormalLog = false;
        public static bool isShowErrorLog = true;

        private static string sdkVersion = "";
        private static string SdkVersion {
            get {
                if (sdkVersion == "") {
                    sdkVersion = API_Module_SDKVersion.Version;
                }
                return sdkVersion;
            }
        }

        public static void Log(string msg, object o, bool current = false,bool all = false) {
            if(all == true) {
                isShowNormalLog = true;
            }

            if(isShowNormalLog == false && current == false)
                return;
            
            Tag = "[ SDK:"+ SdkVersion + " ][ FrameCount:" + Time.frameCount + " ]";

            if(o == null) {
                if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) {
                    Debug.Log(Tag + msg);
                } else {
                    Console.WriteLine(Tag + msg);
                }
            } else {
                if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) {
                    Debug.Log(Tag + "[" + o.GetType().ToString() + "]: " + msg);
                } else {
                    Console.WriteLine(Tag + "[" + o.GetType().ToString() + "]: " + msg);
                }
            }

        }

        public static void LogError(string msg, object o) {
            if(isShowErrorLog == false)
                return;
            Tag = "[SDK: "+ SdkVersion + " ][ FrameCount:" + Time.frameCount + " ]";

            if(o == null) {
                if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) {
                    Debug.Log(Tag + msg);
                } else {
                    Console.WriteLine(Tag + msg);
                }
            } else {
                if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor) {
                    Debug.Log(Tag + "[" + o.GetType().ToString() + "]: " + msg);
                } else {
                    Console.WriteLine(Tag + "[" + o.GetType().ToString() + "]: " + msg);
                }
            }
        }
    }
}

