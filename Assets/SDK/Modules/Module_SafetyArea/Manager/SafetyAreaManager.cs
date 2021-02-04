using SC.XR.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyAreaManager : SingletonMono<SafetyAreaManager>
{
    public Action OnBeginSetSafeArea;
    public Action OnFinishSetSafeArea;

    private Dictionary<SafetyAreaStepEnum, AbstractSafetyAreaStep> areaStepDic;

    private GameObject safetyAreaGameObject;
    private SafetyAreaMono safetyAreaMono;
    private AbstractSafetyAreaStep currentStep;

    //for Test
    //private void Start()
    //{
    //    Init();
    //}

    //for Test
    //private void OnDestroy()
    //{
    //    Release();
    //}

    private void InitStep()
    {
        if (areaStepDic == null)
        {
            areaStepDic = new Dictionary<SafetyAreaStepEnum, AbstractSafetyAreaStep>();
            areaStepDic.Add(SafetyAreaStepEnum.GroundHeight, new GroundHeightStep());
            areaStepDic.Add(SafetyAreaStepEnum.PlayArea, new PlayAreaStep());
            areaStepDic.Add(SafetyAreaStepEnum.StationaryArea, new StationaryAreaStep());
            areaStepDic.Add(SafetyAreaStepEnum.ConfirmPlayArea, new ConfirmPlayAreaStep());
        }
    }

    private void InitSafetyAreaMono()
    {
        if (safetyAreaMono == null)
        {
            GameObject safetyAreaMonoResource = Resources.Load<GameObject>("SafetyAreaMono");
            safetyAreaGameObject = GameObject.Instantiate(safetyAreaMonoResource);
            safetyAreaMono = safetyAreaGameObject.GetComponent<SafetyAreaMono>();
        }
        safetyAreaMono.Init();
    }

    public void StartSetSafetyArea()
    {
        if (safetyAreaMono != null)
        {
            Debug.LogError("last set safety area process not complete");
            return;
        }
        Init();
        OnBeginSetSafeArea?.Invoke();
    }


    public T GetStep<T>(SafetyAreaStepEnum safetyAreaStepEnum) where T : AbstractSafetyAreaStep
    {
        if (!areaStepDic.ContainsKey(safetyAreaStepEnum))
        {
            return null;
        }
        return areaStepDic[safetyAreaStepEnum] as T;
    }

    public void ChangeStep(SafetyAreaStepEnum safetyAreaStep)
    {
        if (currentStep != null)
        {
            currentStep.OnExitStep();
        }
        AbstractSafetyAreaStep nextStep = areaStepDic[safetyAreaStep];
        nextStep.OnEnterStep();
        currentStep = nextStep;
    }

    public void ExitSafeAreaStep()
    {
        if (currentStep != null)
        {
            currentStep.OnExitStep();
        }
        currentStep = null;
        Release();
    }

    private void Init()
    {
        InitStep();
        InitSafetyAreaMono();
    }

    private void Release()
    {
        OnFinishSetSafeArea?.Invoke();
        if (safetyAreaMono != null)
        {
            safetyAreaMono.Release();
            GameObject.Destroy(safetyAreaGameObject);
            safetyAreaMono = null;
        }
    }

}
