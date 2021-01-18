//
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SC.XR.Unity;

public class ShowSliderValue : MonoBehaviour {
    [SerializeField]
    private SCSlider3D sCSlider3D;
    [SerializeField]
    private TextMeshPro textMesh = null;

    void Awake() {
        sCSlider3D?.onValueChanged.AddListener(OnSliderUpdated);
    }

    void OnDestroy() {
        sCSlider3D?.onValueChanged.RemoveListener(OnSliderUpdated);
    }

    public void OnSliderUpdated(float value) {
        if(textMesh == null) {
            textMesh = GetComponent<TextMeshPro>();
        }

        if(textMesh != null) {
            textMesh.text = $"{value:F2}";
        }
    }
}