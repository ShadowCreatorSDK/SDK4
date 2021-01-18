
using SC.XR.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SC.XR.Unity.Module_InputSystem;
using SC.XR.Unity.Module_InputSystem.InputDeviceHead;

public class SliderChangeColor : MonoBehaviour {
    [SerializeField]
    private Renderer TargetRenderer;

    public SCSlider3D RedSCSlider3D;
    public SCSlider3D GreenSCSlider3D;
    public SCSlider3D BlueSCSlider3D;

    void Awake() {
        RedSCSlider3D?.onValueChanged.AddListener(OnSliderUpdatedRed);
        GreenSCSlider3D?.onValueChanged.AddListener(OnSliderUpdatedGreen);
        BlueSCSlider3D?.onValueChanged.AddListener(OnSliderUpdateBlue);
    }

    void OnDestroy() {
        RedSCSlider3D?.onValueChanged.RemoveListener(OnSliderUpdatedRed);
        GreenSCSlider3D?.onValueChanged.RemoveListener(OnSliderUpdatedGreen);
        BlueSCSlider3D?.onValueChanged.RemoveListener(OnSliderUpdateBlue);
    }

    public void OnSliderUpdatedRed(float value) {
        TargetRenderer = GetComponentInChildren<Renderer>();
        if((TargetRenderer != null) && (TargetRenderer.material != null)) {
            TargetRenderer.material.color = new Color(value, TargetRenderer.sharedMaterial.color.g, TargetRenderer.sharedMaterial.color.b);
        }
    }

    public void OnSliderUpdatedGreen(float value) {
        TargetRenderer = GetComponentInChildren<Renderer>();
        if((TargetRenderer != null) && (TargetRenderer.material != null)) {
            TargetRenderer.material.color = new Color(TargetRenderer.sharedMaterial.color.r, value, TargetRenderer.sharedMaterial.color.b);
        }
    }

    public void OnSliderUpdateBlue(float value) {
        TargetRenderer = GetComponentInChildren<Renderer>();
        if((TargetRenderer != null) && (TargetRenderer.material != null)) {
            TargetRenderer.material.color = new Color(TargetRenderer.sharedMaterial.color.r, TargetRenderer.sharedMaterial.color.g, value);
        }
    }
}
