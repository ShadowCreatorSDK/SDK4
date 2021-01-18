
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;

namespace SC.XR.Unity {
    public class ConfigFile {

        private static readonly char[] TrimStart = new char[] { ' ', '\t' };
        private static readonly char[] TrimEnd = new char[] { ' ', '\t', '\r', '\n' };

        private const string DELEMITER = "=";

        private string strFilePath = null;

        private bool IsCaseSensitive = false;
        public Dictionary<string, Dictionary<string, string>> Configs {
            get {
                return ConfigDic;
            }
        }
        private Dictionary<string, Dictionary<string, string>> ConfigDic = new Dictionary<string, Dictionary<string, string>>();

        public ConfigFile(string path, bool isCaseSensitive = false) {
            strFilePath = path;
            IsCaseSensitive = isCaseSensitive;
        }
        public bool ParseConfig() {
            if (!File.Exists(strFilePath)) {
                Debug.LogWarning("the Config file's path is error：" + strFilePath);
                return false;
            }
            using (StreamReader reader = new StreamReader(strFilePath)) {
                string section = null;
                string key = null;
                string val = null;
                Dictionary<string, string> config = null;

                string strLine = null;
                while ((strLine = reader.ReadLine()) != null) {
                    strLine = strLine.TrimStart(TrimStart);
                    strLine = strLine.TrimEnd(TrimEnd);

                    if (strLine.StartsWith("#") || strLine.StartsWith("//")) {
                        continue;
                    }
                    if (strLine.Length <= 2) {
                        continue;
                    }

                    if (TryParseSection(strLine, out section)) {
                        if (!ConfigDic.ContainsKey(section)) {
                            ConfigDic.Add(section, new Dictionary<string, string>());
                        }
                        config = ConfigDic[section];
                        Debug.Log("[Section] : " + "[" + section + "]");
                    } else {
                        if (config != null) {
                            if (TryParseConfig(strLine, out key, out val)) {
                                if (config.ContainsKey(key)) {
                                    config[key] = val;
                                    Debug.LogError("the Key[" + key + "] is appear repeat");
                                } else {
                                    config.Add(key, val);
                                    Debug.Log("     "+key + ":" + val);
                                }
                            }
                        } else {
                            Debug.LogWarning("the Config file's format is error，lost [Section]'s information");
                        }
                    }
                }
                return true;
            }
        }

        public void SaveConfig() {
            if (string.IsNullOrEmpty(strFilePath)) {
                Debug.LogWarning("Empty file name for SaveConfig.");
                return;
            }

            string dirName = Path.GetDirectoryName(strFilePath);
            if (string.IsNullOrEmpty(dirName)) {
                Debug.LogWarning(string.Format("Empty directory for SaveConfig:{0}.", strFilePath));
                return;
            }
            if (!Directory.Exists(dirName)) {
                Directory.CreateDirectory(dirName);
            }

            using (StreamWriter sw = new StreamWriter(strFilePath)) {
                foreach (KeyValuePair<string, Dictionary<string, string>> pair in ConfigDic) {
                    sw.WriteLine("[" + pair.Key + "]");
                    foreach (KeyValuePair<string, string> cfg in pair.Value) {
                        sw.WriteLine(cfg.Key + DELEMITER + cfg.Value);
                    }
                }
            }
        }

        public bool HasKey(string section, string key) {
            if (!IsCaseSensitive) {
                section = section.ToUpper();
                key = key.ToUpper();
            }
            Dictionary<string, string> config = null;
            if (ConfigDic.TryGetValue(section, out config)) {
                return config.ContainsKey(key);
            }
            return false;
        }

        public string GetString(string section, string key, string defaultVal) {
            if (!IsCaseSensitive) {
                section = section.ToUpper();
                key = key.ToUpper();
            }
            Dictionary<string, string> config = null;
            if (ConfigDic.TryGetValue(section, out config)) {
                string ret = null;
                if (config.TryGetValue(key, out ret)) {
                    return ret;
                }
            }
            return defaultVal;
        }
        public float GetFloat(string section, string key, float defaultVal) {

            Dictionary<string, string> config = null;
            if (ConfigDic.TryGetValue(section, out config)) {
                string ret = null;
                if (config.TryGetValue(key, out ret)) {
                    try {
                        return float.Parse(ret);
                    } catch (Exception e) {
                        Debug.Log(e);
                    }
                }
            }
            return defaultVal;
        }

        public int GetInt(string section, string key, int defaultVal) {
            string val = GetString(section, key, null);
            if (val != null) {
                try {
                    return int.Parse(val);
                } catch (Exception e) {
                    Debug.Log(e);
                }
            }
            return defaultVal;
        }

        public void SetString(string section, string key, string val) {
            if (!string.IsNullOrEmpty(section) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(val)) {
                if (!IsCaseSensitive) {
                    section = section.ToUpper();
                    key = key.ToUpper();
                }
                Dictionary<string, string> config = null;
                if (!ConfigDic.TryGetValue(section, out config)) {
                    config = new Dictionary<string, string>();
                    ConfigDic[section] = config;
                }
                config[key] = val;
            }
        }

        public void SetInt(string section, string key, int val) {
            SetString(section, key, val.ToString());
        }

        public void AddString(string section, string key, string val) {
            if (!string.IsNullOrEmpty(section) && !string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(val)) {
                if (!IsCaseSensitive) {
                    section = section.ToUpper();
                    key = key.ToUpper();
                }
                Dictionary<string, string> config = null;
                if (!ConfigDic.TryGetValue(section, out config)) {
                    config = new Dictionary<string, string>();
                    ConfigDic[section] = config;
                }
                if (!config.ContainsKey(key)) {
                    config.Add(key, val);
                }
            }
        }

        public void AddInt(string section, string key, int val) {
            AddString(section, key, val.ToString());
        }

        public bool RemoveSection(string section) {
            if (ConfigDic.ContainsKey(section)) {
                ConfigDic.Remove(section);
                return true;
            }
            return false;
        }

        public bool RemoveConfig(string section, string key) {
            if (!IsCaseSensitive) {
                section = section.ToUpper();
                key = key.ToUpper();
            }
            Dictionary<string, string> config = null;
            if (ConfigDic.TryGetValue(section, out config)) {
                if (config.ContainsKey(key)) {
                    config.Remove(key);
                    return true;
                }
            }
            return false;
        }

        public void RemoveALLConfig() {
            ConfigDic.Clear();
        }

        public void RemoveConfigFile() {
            if (File.Exists(strFilePath)) {
                File.Delete(strFilePath);
            }
        }

        public Dictionary<string, string> GetSectionInfo(string section) {
            Dictionary<string, string> res = null;
            if (!IsCaseSensitive) {
                section = section.ToUpper();
            }
            ConfigDic.TryGetValue(section, out res);
            return res;
        }

        private bool TryParseSection(string strLine, out string section) {
            section = null;
            if (!string.IsNullOrEmpty(strLine)) {
                int len = strLine.Length;
                if (strLine[0] == '[' && strLine[len - 1] == ']') {
                    section = strLine.Substring(1, len - 2);
                    if (!IsCaseSensitive) {
                        section = section.ToUpper();
                    }
                    return true;
                }
            }
            return false;
        }

        private bool TryParseConfig(string strLine, out string key, out string val) {
            if (strLine != null && strLine.Length >= 3) {
                string[] contents = strLine.Split(DELEMITER.ToCharArray());
                if (contents.Length == 2) {
                    key = contents[0].TrimStart(TrimStart);
                    key = key.TrimEnd(TrimEnd);
                    val = contents[1].TrimStart(TrimStart);
                    val = val.TrimEnd(TrimEnd);
                    if (key.Length > 0 && val.Length > 0) {
                        if (!IsCaseSensitive) {
                            key = key.ToUpper();
                        }

                        return true;
                    }
                }
            }

            key = null;
            val = null;
            return false;
        }


    }
}
