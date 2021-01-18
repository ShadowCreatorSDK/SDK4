using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SC.XR.Unity.Module_InputSystem;
using UnityEngine.UI;
using TMPro;
using SC.XR.Unity.Module_InputSystem.InputDeviceHand;

public class HandLowPower : InputDeviceUIType, IHandUIType {
    public HandUIType UIType => HandUIType.LOWPOWER;

    public List<Image> ImageList;
    public List<TextMeshProUGUI> TextMeshProUGUIList;


    Coroutine effect;
    public float effectDurtion = 3;
    private float tempTime = 0;


    public void OnEnable() {
        ImageList = new List<Image>(GetComponentsInChildren<Image>());
        TextMeshProUGUIList = new List<TextMeshProUGUI>(GetComponentsInChildren<TextMeshProUGUI>());
        effect = StartCoroutine(EffectFunction(effectDurtion));
    }

    void OnDisable() {
        if (effect != null) {
            StopCoroutine(effect);
        }
    }

    IEnumerator EffectFunction(float time) {

        yield return new WaitForSeconds(0.2f);
        AudioSystem.getInstance.PlayAudioOneShot(gameObject, SCAudiosConfig.AudioType.Notification);

        tempTime = 0;
        while ((tempTime += Time.deltaTime*1.5f) < time) {
            float flag = Mathf.Clamp01((tempTime) / time);
            foreach (var item in ImageList) {
                item.color = new Color(item.color.r, item.color.g, item.color.b, flag);
            }
            foreach (var item in TextMeshProUGUIList) {
                item.color = new Color(item.color.r, item.color.g, item.color.b, flag);
            }
            yield return null;
        }

        yield return new WaitForSeconds(2);

        tempTime = 0;
        while ((tempTime += Time.deltaTime) < time) {
            float flag = Mathf.Clamp01( (time - tempTime) / time);
            foreach (var item in ImageList) {
                item.color = new Color(item.color.r, item.color.g, item.color.b, flag);
            }
            foreach (var item in TextMeshProUGUIList) {
                item.color = new Color(item.color.r, item.color.g, item.color.b, flag);
            }
            yield return null;
        }

        ModuleStop();
    }


}
