
namespace SC.XR.Unity {

    public enum SCModulePriority { 
        Hight,
        Middle,
        Low,
    }

    public interface ISCModule : ISCLifeCycle {

        string ModuleName { get; set; }

        ISCModule FatherModule { get; set; }

        bool IsModuleInit { get; set; }
        
        bool IsModuleStarted { get; set; }

        bool IsMono { get; set; }

        bool IsEffectGameObject { get; set; }

        SCModulePriority Priority { get; set; }

        T GetSubModule<T>() where T : ISCModule;

        T GetFatherModule<T>() where T : ISCModule;

        /// <summary>
        /// 模块初始化
        /// </summary>
        void ModuleInit(bool isEffectGameObject = true,SCModulePriority priority = SCModulePriority.Middle);
        //void ModuleEnable();
        /// <summary>
        /// 模块启动,同Mono OnEnable
        /// </summary>
        void ModuleStart();
        void ModuleUpdate();
        void ModuleLateUpdate();
        void ModuleEndOfFrame();
        /// <summary>
        /// 模块停止，同Mono OnDisable
        /// </summary>
        void ModuleStop();
        /// <summary>
        /// 模块销毁
        /// </summary>
        void ModuleDestroy();

    }
}
