using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity
{
    [RequireComponent(typeof(SCToggleBase))]
    public class SCToggle3DSounds : MonoBehaviour
    {
        [SerializeField]
        private AudioClip tapSound = null;

        private SCToggleBase toggle;
        private AudioSource tapAudioSource = null;

        private void Start()
        {
            if (tapAudioSource == null)
            {
                tapAudioSource = gameObject.AddComponent<AudioSource>();
            }

            toggle = GetComponent<SCToggleBase>();
            toggle.onValueChanged.AddListener(OnValueChange);
        }

        private void OnValueChange(bool isOn)
        {
            if (tapSound != null && tapAudioSource != null)
            {
                tapAudioSource.PlayOneShot(tapSound);
            }
        }
    }
}
