using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SC.XR.Unity
{
    public class NoticeEffect : MonoBehaviour
    {

        Coroutine effect;

        [HideInInspector]
        public float effectDurtion = 3;
        [HideInInspector]
        public bool isEnable = false;   
        private float tempTime = 0;
        private List<Image> ImageList;
        private List<TextMeshProUGUI> TextMeshProUGUIList;

        private void OnEnable()
        {
            Init();
            isEnable = true;
            effect = StartCoroutine(EffectFunction(effectDurtion));
        }

        private void OnDisable()
        {
            if (effect != null)
            {
                isEnable = false;
                foreach (var item in ImageList)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
                }
                foreach (var item in TextMeshProUGUIList)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
                }
                StopCoroutine(effect);
                ImageList = null;
                TextMeshProUGUIList = null;
            }
        }

        private void Init()
        {
            ImageList = new List<Image>(GetComponentsInChildren<Image>());
            TextMeshProUGUIList = new List<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>());
            foreach (var item in ImageList)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
            }
            foreach (var item in TextMeshProUGUIList)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
            }

        }

        IEnumerator EffectFunction(float time)
        {
            yield return new WaitForSeconds(0.2f);
            AudioSystem.getInstance.PlayAudioOneShot(gameObject, SCAudiosConfig.AudioType.Notification);
            tempTime = 0;
            while ((tempTime += Time.deltaTime * 1.5f) < time)
            {
                float flag = Mathf.Clamp01((tempTime) / time);
                foreach (var item in ImageList)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, flag);
                }
                foreach (var item in TextMeshProUGUIList)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, flag);
                }
                yield return null;
            }

            yield return new WaitForSeconds(2);

            tempTime = 0;
            while ((tempTime += Time.deltaTime) < time)
            {
                float flag = Mathf.Clamp01((time - tempTime) / time);
                foreach (var item in ImageList)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, flag);
                }
                foreach (var item in TextMeshProUGUIList)
                {
                    item.color = new Color(item.color.r, item.color.g, item.color.b, flag);
                }
                yield return null;
            }
            this.enabled = false;
       
        }
    }
}
