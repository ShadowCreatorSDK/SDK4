using SC.XR.Unity;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace SC.XR.Unity
{
    public abstract class SCKeyboardBase
    {
        public SCKeyboardMono keyboardMono;
        //private TextMeshProUGUI keyboardText;

        public SCKeyboardBase(Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            GameObject keyboardPrefab = Resources.Load<GameObject>(PrefabResourceName);
            if (keyboardPrefab == null)
            {
                Debug.LogError("This prefab resource do not exist");
                return;
            }

            GameObject keyboardGameObject = GameObject.Instantiate(keyboardPrefab);
            this.KeyboardGameObject = keyboardGameObject;
            SetKeyboardTransform(parent, position, rotation, scale);

            keyboardMono = keyboardGameObject.GetComponent<SCKeyboardMono>();
            if (keyboardMono == null)
            {
                Debug.LogError("This keyboard prefab do not have scripts inhert from SCKeyboardMono!");
                return;
            }
            keyboardMono.Initialize();
            //keyboardText = keyboardMono.keyboardText;
            keyboardMono.OnDoneButtonClick += OnDoneButtonClick;
            keyboardMono.OnTextChange += OnTextChange;

            ResetKeyboard();
        }

        //TODO
        public static bool HideInput { get; set; }

        public abstract string PrefabResourceName { get; }

        public bool Done { get; set; }

        public bool WasCanceled { get; set; }

        private string text = string.Empty;
        public string Text { 
            get
            {
                return text;
            }
            set
            {
                keyboardMono.keyboardPrompt.SetEnteredText(value);
                text = value;
            }
        }

        public bool Active { 
            get
            {
                return KeyboardGameObject.activeSelf;
            }
            set
            {
                KeyboardGameObject.SetActive(value);
            } 
        }

        protected GameObject KeyboardGameObject { get; set; }

        private static Dictionary<SCKeyboardEnum, Type> keyboardTypeDic = new Dictionary<SCKeyboardEnum, Type>()
        {
            { SCKeyboardEnum.SCKeyboard2D, typeof(SCKeyboard2D) },
            { SCKeyboardEnum.SCKeyboard3D, typeof(SCKeyboard3D) }
        };

        private static Dictionary<Transform, Dictionary<SCKeyboardEnum, SCKeyboardBase>> keyboardCacheDic = new Dictionary<Transform, Dictionary<SCKeyboardEnum, SCKeyboardBase>>();

        public static SCKeyboardBase Open(SCKeyboardEnum sckeyboardEnum, string text, TouchScreenKeyboardType touchScreenKeyboardType, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (keyboardCacheDic.ContainsKey(parent))
            {
                Dictionary<SCKeyboardEnum, SCKeyboardBase> keyboardEnumDic = keyboardCacheDic[parent];
                if (keyboardEnumDic != null && keyboardEnumDic.ContainsKey(sckeyboardEnum))
                {
                    SCKeyboardBase keyboardCache = keyboardEnumDic[sckeyboardEnum];
                    keyboardCache.ResetKeyboard();
                    keyboardCache.SetKeyboardTransform(parent, position, rotation, scale);
                    return keyboardCache;
                }
            }

            if (!keyboardTypeDic.ContainsKey(sckeyboardEnum))
            {
                DebugMy.LogError("This SCKeyboardEnum do not exist", sckeyboardEnum);
                return null;
            }

            SCKeyboardBase keyboard = Activator.CreateInstance(keyboardTypeDic[sckeyboardEnum], parent, position, rotation, scale) as SCKeyboardBase;
            if (!keyboardCacheDic.ContainsKey(parent))
            {
                keyboardCacheDic[parent] = new Dictionary<SCKeyboardEnum, SCKeyboardBase>();
            }
            keyboardCacheDic[parent][sckeyboardEnum] = keyboard;
            return keyboard;
        }

        private void OnDoneButtonClick()
        {
            Debug.Log("OnDoneButtonClick");
            this.Done = true;
        }

        private void OnTextChange(string text)
        {
            this.Text = text;
        }

        private void ResetKeyboard()
        {
            this.Done = false;
            this.WasCanceled = false;
            //this.Text = string.Empty;
        }

        private void SetKeyboardTransform(Transform parent, Vector3 positon, Quaternion rotation, Vector3 scale)
        {
            if (KeyboardGameObject == null)
            {
                Debug.LogError("KeyboardGameObject is NULL");
                return;
            }

            this.KeyboardGameObject.transform.SetParent(null);
            this.KeyboardGameObject.transform.position = positon;
            this.KeyboardGameObject.transform.rotation = rotation;
            this.KeyboardGameObject.transform.localScale = scale;
            this.KeyboardGameObject.transform.SetParent(parent);
        }

        public void SetKeyboardTransform(Vector3 positon, Quaternion rotation, Vector3 localScale)
        {
            this.KeyboardGameObject.transform.position = positon;
            this.KeyboardGameObject.transform.rotation = rotation;
            this.KeyboardGameObject.transform.localScale = localScale;
        }
    }
}
