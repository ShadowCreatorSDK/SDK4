
using SC.XR.Unity.Module_Device;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity {

    public class API_Module_SDKConfiguration {
        public static bool HasKey (string section, string key) {
            return Module_SDKConfiguration.getInstance.HasKey(section, key);
        }

        public static int GetInt(string section, string key, int defaultVal) {
            return Module_SDKConfiguration.getInstance.GetInt(section, key, defaultVal);
        }

        public static bool GetBool(string section, string key, int defaultVal) {
            return Module_SDKConfiguration.getInstance.GetBool(section, key, defaultVal);
        }

        public static string GetString(string section, string key, string defaultVal) {
            return Module_SDKConfiguration.getInstance.GetString(section, key, defaultVal);
        }

        public static float GetFloat(string section, string key, float defaultVal) {
            return Module_SDKConfiguration.getInstance.GetFloat(section, key, defaultVal);
        }


    }


}