
namespace SC.XR.Unity {
    public interface ISCLifeCycle {
        void OnSCAwake();
        void OnSCEnable();
        void OnSCStart();
        void OnSCUpdate();
        void OnSCLateUpdate();
        void OnSCFuncitonWaitForEndOfFrame();
        void OnSCDisable();
        void OnSCDestroy();
    }
}
