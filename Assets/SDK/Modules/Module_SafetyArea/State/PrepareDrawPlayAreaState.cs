using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PrepareDrawPlayAreaState : AbstractPlayAreaState<SafetyAreaMono>
{
    public override void OnStateEnter(object data)
    {
        ShowPlayAreaWaitingDrawUI();
        reference.playAreaWaitingDrawUI.OnSwitchToStationaryAreaClick += SwitchToStationaryAreaStep;
        reference.playAreaWaitingDrawUI.OnBackClick += SwitchToGroundHeightStep;
    }

    public override void OnStateExit(object data)
    {
        reference.playAreaWaitingDrawUI.OnSwitchToStationaryAreaClick -= SwitchToStationaryAreaStep;
        reference.playAreaWaitingDrawUI.OnBackClick -= SwitchToGroundHeightStep;
        HidePlayAreaWaitingDrawUI();
    }

    private void SwitchToStationaryAreaStep()
    {
        SafetyAreaManager.Instance.ChangeStep(SafetyAreaStepEnum.StationaryArea);
    }

    private void SwitchToGroundHeightStep()
    {
        SafetyAreaManager.Instance.ChangeStep(SafetyAreaStepEnum.GroundHeight);
    }

    public void ShowPlayAreaWaitingDrawUI()
    {
        reference.playAreaWaitingDrawUI.gameObject.SetActive(true);
        reference.playAreaWaitingDrawUI.Init();
        reference.safetyPlaneMono.RegistPointerUpEvent(ChangePlayAreaOKStep);
    }

    public void HidePlayAreaWaitingDrawUI()
    {
        reference.safetyPlaneMono.UnRegistPointerUpEvent(ChangePlayAreaOKStep);
        reference.playAreaWaitingDrawUI.Release();
        reference.playAreaWaitingDrawUI.gameObject.SetActive(false);
    }

    private void ChangePlayAreaOKStep(PointerEventData pointerEventData)
    {
        reference.ChangePlayAreaState(PlayAreaStateEnum.OK);
    }
}
