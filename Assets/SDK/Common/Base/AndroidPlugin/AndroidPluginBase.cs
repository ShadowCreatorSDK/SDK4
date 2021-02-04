using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity {
    public abstract class AndroidPluginBase:  SCModule {

        private static AndroidJavaObject mCurrentActivity = null;
        private static AndroidJavaClass mUnityPlayerClass = null;
        private static AndroidJavaObject mContext = null;
        public static AndroidJavaObject CurrentActivity {
            get {
                if(Application.platform == RuntimePlatform.Android) {
                    if(mCurrentActivity == null && UnityPlayerClass != null) {
                        mCurrentActivity = UnityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                    }
                }
                return mCurrentActivity;
            }
        }
        public static AndroidJavaClass UnityPlayerClass {
            get {
                if(Application.platform == RuntimePlatform.Android) {
                    if(mUnityPlayerClass == null) {
                        mUnityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    }
                }
                return mUnityPlayerClass;
            }
        }
        public static AndroidJavaObject Context {
            get {
                if(Application.platform == RuntimePlatform.Android) {
                    if(mContext == null) {
                        mContext = CurrentActivityFunctionCall<AndroidJavaObject>("getApplicationContext");
                    }
                }
                return mContext;
            }
        }

        protected static List<AndroidJavaProxy> androidListernerList = new List<AndroidJavaProxy>();

        public static void AddListener(string setCallBackFunctionName, AndroidJavaProxy callBack) {
            if(Application.platform == RuntimePlatform.Android) {
                CurrentActivity.Call(setCallBackFunctionName, callBack);
                androidListernerList.Add(callBack);
            }
        }

        public static void ObjectAddListener(AndroidJavaObject androidObject,string setCallBackFunctionName, AndroidJavaProxy callBack) {
            if(Application.platform == RuntimePlatform.Android) {
                if(androidObject != null) {
                    androidObject.Call(setCallBackFunctionName, callBack);
                    androidListernerList.Add(callBack);
                }
            }
        }

        public static AndroidJavaClass GetAndroidJavaClass(string classPatch) {
            if(Application.platform == RuntimePlatform.Android) {
                try {
                    return new AndroidJavaClass(classPatch);
                } catch(Exception e) {
                    Debug.LogError(e);
                }
            }
            return null;
        }
        /// <summary>
        /// 实例方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="androidJavaObject"></param>
        /// <param name="callName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static T ObjectFunctionCall<T>(AndroidJavaObject androidJavaObject, string callName, params object[] args) {
            if(androidJavaObject != null) {
                try {
                    return androidJavaObject.Call<T>(callName, args);
                } catch(Exception e) {
                    Debug.LogError(e);
                }
            }
            return default(T);
        }
        public static void ObjectFunctionCall(AndroidJavaObject androidJavaObject, string callName, params object[] args) {
            if(androidJavaObject != null) {
                try {
                    androidJavaObject.Call(callName, args);
                } catch(Exception e) {
                    Debug.LogError(e);
                }
            }
        }
        public static T ObjectFunctionCall2<T>(AndroidJavaObject androidJavaObject, string callName, object args) {
            if(androidJavaObject != null) {
                try {
                    return androidJavaObject.Call<T>(callName, args);
                } catch(Exception e) {
                    Debug.LogError(e);
                }
            }
            return default(T);
        }
        public static void ObjectFunctionCall2(AndroidJavaObject androidJavaObject, string callName, object args) {
            if(androidJavaObject != null) {
                try {
                    androidJavaObject.Call(callName, args);
                } catch(Exception e) {
                    Debug.LogError(e);
                }
            }
        }

        public static T CurrentActivityFunctionCall<T>(string callName, params object[] args) {
            if(CurrentActivity != null) {
                try {
                    return CurrentActivity.Call<T>(callName, args);
                } catch(Exception e) {
                    Debug.LogError(e);
                }
            }
            return default(T);
        }
        public static void CurrentActivityFunctionCall(string callName, params object[] args) {
            if(CurrentActivity != null) {
                try {
                    CurrentActivity.Call(callName, args);
                } catch(Exception e) {
                    Debug.LogError(e);
                }
            }
        }

        /// <summary>
        /// 类方法
        /// </summary>
        /// <param name="androidJavaClass"></param>
        /// <param name="callName"></param>
        /// <param name="args"></param>
        public static void ClassFunctionCallStatic(AndroidJavaClass androidJavaClass, string callName, params object[] args) {
            if(androidJavaClass != null) {
                try {
                    androidJavaClass.CallStatic(callName, args);
                } catch(Exception e) {
                    Debug.LogError(e);
                }
            }
        }
        public static T ClassFunctionCallStatic<T>(AndroidJavaClass androidJavaClass, string callName, params object[] args) {
            if(androidJavaClass != null) {
                try {
                    return androidJavaClass.CallStatic<T>(callName, args);
                } catch(Exception e) {
                    Debug.LogError(e);
                }
            }
            return default(T);
        }

    }
}
