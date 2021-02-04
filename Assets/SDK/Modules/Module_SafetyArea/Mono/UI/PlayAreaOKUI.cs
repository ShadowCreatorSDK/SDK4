using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayAreaOKUI : MonoBehaviour
{
    public Action OnRedrawAreaClick
    {
        get;
        set;
    }

    public Action OnContinueClick
    {
        get;
        set;
    }

    public Action OnBackClick
    {
        get;
        set;
    }

    public Button redrawAreaButton;
    public Button continueButton;
    public Button backButton;

    public void Init()
    {
        redrawAreaButton.onClick.AddListener(() =>
        {
            OnRedrawAreaClick?.Invoke();
        });

        continueButton.onClick.AddListener(() =>
        {
            OnContinueClick?.Invoke();
        });

        backButton.onClick.AddListener(() =>
        {
            OnBackClick?.Invoke();
        });
    }

    public void Release()
    {
        redrawAreaButton.onClick.RemoveAllListeners();
        continueButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }
}
