using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAreaOKState : AbstractPlayAreaState<SafetyAreaMono>
{
    public override void OnStateEnter(object data)
    {
        ShowPlayAreaOKUI();
        reference.playAreaOKUI.OnRedrawAreaClick += ChangePrepareDrawPlayAreaState;
        reference.playAreaOKUI.OnBackClick += ChangePrepareDrawPlayAreaState;
        reference.playAreaOKUI.OnContinueClick += SwitchToConfirmPlayAreaStep;
    }

    public override void OnStateExit(object data)
    {
        reference.playAreaOKUI.OnRedrawAreaClick -= ChangePrepareDrawPlayAreaState;
        reference.playAreaOKUI.OnBackClick -= ChangePrepareDrawPlayAreaState;
        reference.playAreaOKUI.OnContinueClick -= SwitchToConfirmPlayAreaStep;
        HidePlayAreaOKUI();
    }

    private void ChangePrepareDrawPlayAreaState()
    {
        reference.ClearPlaneColor();
        reference.ChangePlayAreaState(PlayAreaStateEnum.WaitingDraw);
    }

    private void SwitchToConfirmPlayAreaStep()
    {
        SafetyAreaManager.Instance.ChangeStep(SafetyAreaStepEnum.ConfirmPlayArea);
    }

    public void ShowPlayAreaOKUI()
    {
        reference.playAreaOKUI.gameObject.SetActive(true);
        reference.playAreaOKUI.Init();
    }

    public void HidePlayAreaOKUI()
    {
        reference.playAreaOKUI.Release();
        reference.playAreaOKUI.gameObject.SetActive(false);
    }
}
