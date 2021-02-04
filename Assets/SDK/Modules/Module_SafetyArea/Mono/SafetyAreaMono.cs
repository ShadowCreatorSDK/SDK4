using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SafetyAreaMono : MonoBehaviour
{
    private const string PLAY_AREA_NAME = "PlayArea";
    private const string STATIONARY_NAME = "StationaryArea";

    public GroundHeightUI groundHeightUI;
    public PlayAreaWaitingDrawUI playAreaWaitingDrawUI;
    public PlayAreaOKUI playAreaOKUI;
    public StationaryAreaUI stationaryAreaUI;
    public ConfirmPlayAreaUI confirmPlayAreaUI;

    public SafetyPlaneMono safetyPlaneMono;
    private GameObject safetyPlaneObject;
    private StationaryAreaMono stationaryAreaMono;
    private GameObject stationaryAreaObject;

    private GroundHeightStep groundHeightStep;
    private PlayAreaStep playAreaStep;
    private StationaryAreaStep stationaryAreaStep;
    private ConfirmPlayAreaStep confirmPlayAreaStep;

    private PlayAreaStateMachine playAreaStateMachine;

    public void Init()
    {
        if (groundHeightStep == null)
        {
            groundHeightStep = SafetyAreaManager.Instance.GetStep<GroundHeightStep>(SafetyAreaStepEnum.GroundHeight);
        }

        if (playAreaStep == null)
        {
            playAreaStep = SafetyAreaManager.Instance.GetStep<PlayAreaStep>(SafetyAreaStepEnum.PlayArea);
        }

        if (stationaryAreaStep == null)
        {
            stationaryAreaStep = SafetyAreaManager.Instance.GetStep<StationaryAreaStep>(SafetyAreaStepEnum.StationaryArea);
        }

        if (confirmPlayAreaStep == null)
        {
            confirmPlayAreaStep = SafetyAreaManager.Instance.GetStep<ConfirmPlayAreaStep>(SafetyAreaStepEnum.ConfirmPlayArea);
        }

        if (playAreaStateMachine == null)
        {
            playAreaStateMachine = new PlayAreaStateMachine();
            playAreaStateMachine.InitStateMachine(this);
        }

        groundHeightStep.RegistOnEnterStepCallback(OnEnterGroundHeightStep);
        groundHeightStep.RegistOnExitStepCallback(OnExitGroundHeightStep);

        playAreaStep.RegistOnEnterStepCallback(OnEnterPlayAreaStep);
        playAreaStep.RegistOnExitStepCallback(OnExitPlayAreaStep);

        stationaryAreaStep.RegistOnEnterStepCallback(OnEnterStationAreaStep);
        stationaryAreaStep.RegistOnExitStepCallback(OnExitStationAreaStep);

        confirmPlayAreaStep.RegistOnEnterStepCallback(OnEnterConfirmAreaStep);
        confirmPlayAreaStep.RegistOnExitStepCallback(OnExitConfirmAreaStep);

        SafetyAreaManager.Instance.ChangeStep(SafetyAreaStepEnum.GroundHeight);
    }

    public void Release()
    {
        groundHeightStep.UnRegistOnEnterStepCallback(OnEnterGroundHeightStep);
        groundHeightStep.UnRegistOnExitStepCallback(OnExitGroundHeightStep);

        playAreaStep.UnRegistOnEnterStepCallback(OnEnterPlayAreaStep);
        playAreaStep.UnRegistOnExitStepCallback(OnExitPlayAreaStep);

        stationaryAreaStep.UnRegistOnEnterStepCallback(OnEnterStationAreaStep);
        stationaryAreaStep.UnRegistOnExitStepCallback(OnExitStationAreaStep);

        confirmPlayAreaStep.UnRegistOnEnterStepCallback(OnEnterConfirmAreaStep);
        confirmPlayAreaStep.UnRegistOnExitStepCallback(OnExitConfirmAreaStep);
    }

    //进入设置地面高度步骤
    private void OnEnterGroundHeightStep()
    {
        groundHeightUI.gameObject.SetActive(true);
        groundHeightUI.Init();
        CreateSafetyPlane();
        ResetSafetyPlane();
        UnFreezePlaneHeight();
        groundHeightUI.OnConfirmClick += SwitchToPlayAreaStep;
        groundHeightUI.OnResetClick += ResetSafetyPlane;
        groundHeightUI.OnCancelClick += ExitSafetyAreaProcess;
    }

    //退出设置地面高度步骤
    private void OnExitGroundHeightStep()
    {
        FreezePlaneHeight();
        groundHeightUI.OnConfirmClick -= SwitchToPlayAreaStep;
        groundHeightUI.OnResetClick -= ResetSafetyPlane;
        groundHeightUI.OnCancelClick -= ExitSafetyAreaProcess;
        groundHeightUI.Release();
        groundHeightUI.gameObject.SetActive(false);
    }

    //进入设置游戏区域步骤
    private void OnEnterPlayAreaStep()
    {
        safetyPlaneMono.RegistPointerDownFillEvent();
        safetyPlaneMono.RegistPointerUpFillEvent();
        ShowPlane();
        ClearPlaneColor();
        ChangePlayAreaState(PlayAreaStateEnum.WaitingDraw);
    }

    //退出设置游戏区域步骤
    private void OnExitPlayAreaStep()
    {
        safetyPlaneMono.UnRegistPointerDownFillEvent();
        safetyPlaneMono.UnRegistPointerUpFillEvent();
        if (playAreaStateMachine != null)
        {
            playAreaStateMachine.ExitCurrentState();
        }
    }

    //进入原地区域步骤
    private void OnEnterStationAreaStep()
    {
        stationaryAreaUI.gameObject.SetActive(true);
        stationaryAreaUI.Init();
        CreateStationarySafetyArea();
        HidePlane();
        stationaryAreaUI.OnSwitchToPlayAreaClick += OnSwitchPlayAreaClick;
        stationaryAreaUI.OnCancelClick += OnStationaryAreaCancelClick;
        stationaryAreaUI.OnConfirmClick += OnStationaryAreaConfirmClick;
    }

    //退出原地区域步骤
    private void OnExitStationAreaStep()
    {
        stationaryAreaUI.OnSwitchToPlayAreaClick -= OnSwitchPlayAreaClick;
        stationaryAreaUI.OnCancelClick -= OnStationaryAreaCancelClick;
        stationaryAreaUI.OnConfirmClick -= OnStationaryAreaConfirmClick;
        stationaryAreaUI.Release();
        stationaryAreaUI.gameObject.SetActive(false);
    }

    //进入确定游戏区域步骤
    private void OnEnterConfirmAreaStep()
    {
        safetyPlaneMono.GenerateEdgeMesh((mesh) =>
        {
            CreateSafetyArea(mesh);
        });
        confirmPlayAreaUI.gameObject.SetActive(true);
        confirmPlayAreaUI.Init();
        confirmPlayAreaUI.OnConfirmClick += ExitSafetyAreaProcess;
        confirmPlayAreaUI.OnBackClick += OnConfirmPlayAreaCancel;
    }

    //退出确定游戏区域步骤
    private void OnExitConfirmAreaStep()
    {
        confirmPlayAreaUI.OnConfirmClick -= ExitSafetyAreaProcess;
        confirmPlayAreaUI.OnBackClick -= OnConfirmPlayAreaCancel;
        confirmPlayAreaUI.Release();
        confirmPlayAreaUI.gameObject.SetActive(false);
    }

    //切换游戏区域步骤的状态
    public void ChangePlayAreaState(PlayAreaStateEnum playAreaStateEnum, object data = null)
    {
        if (playAreaStateMachine != null)
        {
            playAreaStateMachine.ChangeState(playAreaStateEnum, data);
        }
    }

    //创建平面
    public void CreateSafetyPlane()
    {
        if (safetyPlaneMono == null)
        {
            safetyPlaneObject = new GameObject("SafetyPlane");
            safetyPlaneMono = safetyPlaneObject.AddComponent<SafetyPlaneMono>();
            safetyPlaneMono.Init();
        }
    }

    //重置平面高度
    public void ResetSafetyPlane()
    {
        safetyPlaneMono.ResetPlaneHeight();
    }

    //冻结平面高度
    public void FreezePlaneHeight()
    {
        if (safetyPlaneMono == null)
        {
            Debug.LogError("safetyPlaneMono is Null FreezePlaneHeight");
            return;
        }
        safetyPlaneMono.FreezePlaneHeight();
    }

    //允许平面高度变化
    public void UnFreezePlaneHeight()
    {
        if (safetyPlaneMono == null)
        {
            Debug.LogError("safetyPlaneMono is Null UnFreezePlaneHeight");
            return;
        }
        safetyPlaneMono.UnFreezePlaneHeight();
    }

    //暂时隐藏平面
    public void HidePlane()
    {
        safetyPlaneMono.gameObject.SetActive(false);
    }

    //显示平面
    public void ShowPlane()
    {
        safetyPlaneMono.gameObject.SetActive(true);
    }

    //清空平面颜色
    public void ClearPlaneColor()
    {
        safetyPlaneMono.ClearMeshColor();
    }

    //销毁平面
    private void DestroySafetyPlane()
    {
        if (safetyPlaneObject != null)
        {
            GameObject.Destroy(safetyPlaneObject);
        }
        safetyPlaneObject = null;
        safetyPlaneMono = null;
    }

    //创建原地区域
    private void CreateStationarySafetyArea()
    {
        if (stationaryAreaMono == null)
        {
            stationaryAreaObject = new GameObject(STATIONARY_NAME);
            stationaryAreaMono = stationaryAreaObject.AddComponent<StationaryAreaMono>();
        }
    }
    
    //销毁原地区域
    private void DestroyStationaryArea()
    {
        if (stationaryAreaObject != null)
        {
            GameObject.Destroy(stationaryAreaObject);
        }
        stationaryAreaObject = null;
        stationaryAreaMono = null;
    }

    private void FreezeStationarySafetyArea()
    {
        if (stationaryAreaMono == null)
        {
            Debug.LogError("stationaryAreaMono is Null FreezeStationarySafetyArea");
        }
        stationaryAreaMono.FreezeStationaryAreaPosition();
    }

    private void UnFreezeStationarySafetyArea()
    {
        if (stationaryAreaMono == null)
        {
            Debug.LogError("stationaryAreaMono is Null UnFreezeStationarySafetyArea");
        }
        stationaryAreaMono.UnFreezeStationaryAreaPosition();
    }

    //创建安全网格
    private void CreateSafetyArea(Mesh mesh)
    {
        GameObject safetyArea = GameObject.Find(PLAY_AREA_NAME);
        if (safetyArea != null)
        {
            GameObject.Destroy(safetyArea);
        }
        safetyArea = new GameObject(PLAY_AREA_NAME);
        PlayAreaMono playAreaMono = safetyArea.AddComponent<PlayAreaMono>();
        playAreaMono.SetMesh(mesh);
    }

    //切换到游戏区域步骤
    private void SwitchToPlayAreaStep()
    {
        SafetyAreaManager.Instance.ChangeStep(SafetyAreaStepEnum.PlayArea);
    }

    //确认安全区域的步骤点击取消
    private void OnConfirmPlayAreaCancel()
    {
        GameObject safetyArea = GameObject.Find(PLAY_AREA_NAME);
        if (safetyArea != null)
        {
            GameObject.Destroy(safetyArea);
        }
        SafetyAreaManager.Instance.ChangeStep(SafetyAreaStepEnum.PlayArea);
    }

    //原地区域切换游戏区域
    private void OnSwitchPlayAreaClick()
    {
        DestroyStationaryArea();
        SwitchToPlayAreaStep();
    }

    //原地区域确认点击
    private void OnStationaryAreaConfirmClick()
    {
        FreezeStationarySafetyArea();
        ExitSafetyAreaProcess();
    }

    //原地区域点击取消
    private void OnStationaryAreaCancelClick()
    {
        DestroyStationaryArea();
        ExitSafetyAreaProcess();
    }

    //退出设定安全区域流程
    private void ExitSafetyAreaProcess()
    {
        DestroySafetyPlane();
        SafetyAreaManager.Instance.ExitSafeAreaStep();
    }
}
