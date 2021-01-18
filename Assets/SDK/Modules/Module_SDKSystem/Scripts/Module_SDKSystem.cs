using SC.XR.Unity;
using SC.XR.Unity.Module_Device;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity.Module_SDKSystem {

    public class Module_SDKSystem : SCModuleMono {

        public static Module_SDKSystem Instance { get; private set; }

        [SerializeField]
        private bool EnableInputSystem = true;

        public bool DebugLog;

        bool isStart = false;

        public bool IsRunning {
            get; private set;
        }
        public bool Initialized {
            get; private set;
        }

        private Module_InputSystem.Module_InputSystem mInputSystem;
        public Module_InputSystem.Module_InputSystem InputSystem {
            get {
                if(EnableInputSystem && mInputSystem == null) {
                    mInputSystem = GetComponentInChildren<Module_InputSystem.Module_InputSystem>(true);
                }
                return mInputSystem;
            }
        }

        private SvrManager mSvrManager;
        public SvrManager SvrManager {
            get {
                if (mSvrManager == null) {
                    mSvrManager = GetComponentInChildren<SvrManager>(true);
                }
                return mSvrManager;
            }
        }

        Coroutine waitSlam = null;
        Coroutine isRunningCoroutine = null;

        private Coroutine updateWaitForEndOfFrame = null;

        #region MonoBehavior Driver

        void Awake() {
            ModuleInit(false);
        }

        void OnEnable() {
            if(updateWaitForEndOfFrame == null) {
                updateWaitForEndOfFrame = StartCoroutine(UpdateWaitForEndOfFrame());
            }
            if(isStart == true) {
                ModuleStart();
            }
        }

        void Start() {
            isStart = true;
            ModuleStart();
        }

        void Update() {
            ModuleUpdate();
        }

        void LateUpdate() {
            ModuleLateUpdate();
        }

        void OnApplicationPause(bool pause) {
            if(isStart) {
                if(pause) {
                    ModuleStop();
                } else {

                    ModuleStart();
                }
            }
        }

        IEnumerator UpdateWaitForEndOfFrame() {
            while(true) {
                yield return new WaitForEndOfFrame();
                if(InputSystem && InputSystem.IsModuleStarted) {
                    InputSystem.ModuleEndOfFrame();
                }
            }
        }


        void OnDisable() {

            if(updateWaitForEndOfFrame != null) {
                StopCoroutine(updateWaitForEndOfFrame);
            }

            ModuleStop();
        }

        void OnDestroy() {
            ModuleDestroy();
            isStart = false;
        }

        #endregion


        #region Module Behavoir

        public override void OnSCAwake() {
            base.OnSCAwake();

            if(Instance != null) {
                DestroyImmediate(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            Instance = this;
            DebugMy.isShowNormalLog = DebugLog;

            DebugMy.Log("Awake", this, true);
            DebugMy.Log("SDK Version:"+API_Module_SDKVersion.Version,this,true);

            Module_Device.Module_Device.getInstance.Current.ShowInfo();

            SvrManager?.gameObject.SetActive(false);

            AddModule(InputSystem);
        }



        public override void OnSCStart() {
            base.OnSCStart();
            
            SvrManager?.gameObject.SetActive(true);

            if (waitSlam == null) {
                waitSlam = StartCoroutine(WaitSlamAction());
            }

            if (isRunningCoroutine == null) {
                isRunningCoroutine = StartCoroutine(Running());
            }
        }

        IEnumerator WaitSlamAction() {
            yield return new WaitUntil(() => SvrManager.Instance.IsRunning);
            InputSystem?.ModuleStart();
            waitSlam = null;
        }

        public override void OnSCDisable() {
            base.OnSCDisable();

            if (waitSlam != null) {
                StopCoroutine(waitSlam);
            }
            IsRunning = false;

            //不能操作 灭屏唤醒否则起不来
            //SvrManager?.gameObject.SetActive(false);
        }

        public override void OnSCDestroy() {
            base.OnSCDestroy();
            if (Instance != this)
                return;

            if (waitSlam != null) {
                StopCoroutine(waitSlam);
            }
            if (isRunningCoroutine != null) {
                StopCoroutine(isRunningCoroutine);
            }

            SvrManager?.gameObject.SetActive(false);
        }

        #endregion


        IEnumerator Running() {

            ///SLam model
            yield return new WaitUntil(() =>  SvrManager.Instance.IsRunning);

            if (InputSystem) {
                yield return new WaitUntil(() =>InputSystem.IsRunning);
            }

            IsRunning = true;
            isRunningCoroutine = null;
            DebugMy.Log("SDKSystem Module IsRuning !", this,true);
        }


    }
}

