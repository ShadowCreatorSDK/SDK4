using SC.XR.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity {
    public class Module_SDKConfiguration : MonoBehaviour {

        private static Module_SDKConfiguration mInstance;

        public static Module_SDKConfiguration getInstance {
            get {
                if (mInstance == null) {
                    mInstance = new GameObject("SDKConfiguration").AddComponent<Module_SDKConfiguration>();
                }
                return mInstance;
            }
        }

        private SDKConfiguration SDKConfiguration;

        private ConfigFile configFile;
        private string fileConfigPath {
            get {
                return Application.persistentDataPath + "/" + "SDK"+"_Configs.txt";
            }
        }


        void Awake() {

            SDKConfiguration = Resources.Load<SDKConfiguration>("SDKConfiguration");
            if (SDKConfiguration == null) {
                DebugMy.Log("SDKConfiguration Not Exist !", this, true);
            }

            configFile = new ConfigFile(fileConfigPath,true);


            if (Application.isEditor) {
                DebugMy.Log("Platfom isEditor: Remove SDKConfigs File",this,true);
                configFile.RemoveConfigFile();
            }

            bool isExist = configFile.ParseConfig();
            if (isExist == false) {
                DebugMy.Log("ConfigFile:" + fileConfigPath + " Not Exist !", this, true);
            } else {
                DebugMy.Log("ConfigFile:" + fileConfigPath + " Exist !", this, true);
            }

            if (SDKConfiguration) {
                configFile.SetString("SDK_Information", "Version", API_Module_SDKVersion.Version);

                if (isExist == false) {
                    foreach (var section in SDKConfiguration.Configs) {
                        foreach (var keyValue in section.KEY_VALUE) {
                            configFile.SetString(section.section, keyValue.Name, keyValue.Value);
                            DebugMy.Log("Write To ConfigsFile ==> [" + section.section + "]:" + keyValue.Name + "=" + keyValue.Value, this, true);
                        }
                    }
                    configFile.SaveConfig();
                } else {
                    foreach (var dic in configFile.Configs.Keys) {
                        foreach (var keyValue in configFile.Configs[dic]) {
                            DebugMy.Log("Read From ConfigsFile ==> [" + dic + "]:" + keyValue.Key + "=" + keyValue.Value, this, true);
                        }
                    }
                }
            }

        }

        public int GetInt(string section, string key, int defaultVal) {
            return getInstance.configFile.GetInt(section,key,defaultVal);
        }

        public bool GetBool(string section, string key, int defaultVal) {
            return getInstance.configFile.GetInt(section, key, defaultVal) > 0 ? true : false;
        }

        public string GetString(string section, string key, string defaultVal) {
            return getInstance.configFile.GetString(section, key, defaultVal);
        }

        public float GetFloat(string section, string key, float defaultVal) {
            return getInstance.configFile.GetFloat(section, key, defaultVal);
        }

        public bool HasKey(string section, string key) {
            return getInstance.configFile.HasKey(section,key);
        }

    }
}
