using System;
using System.Collections.Generic;
using UnityEngine;

namespace SC.XR.Unity {

    public abstract class SCModuleMono : MonoBehaviour, ISCModule {

        public string ModuleName { get; set; }
        public bool IsModuleInit { get; set; }
        public bool IsModuleStarted { get; set; }
        public ISCModule FatherModule { get; set; }

        List<ISCModule> subModuleList = new List<ISCModule>();
        public bool IsMono { get; set; }
        public SCModulePriority Priority { get; set; }
        public bool IsEffectGameObject { get; set; }


        /// <summary>
        /// Module 初始化 ===================================
        /// </summary>
        public void ModuleInit(bool isEffectGameObject = true, SCModulePriority priority = SCModulePriority.Middle) {
            if(IsModuleInit) {
                DebugMy.LogError("ModuleInit Had Invoke", this);
                return;
            }

            IsModuleInit = true;
            ModuleName = GetType().ToString();
            IsMono = true;

            IsEffectGameObject = isEffectGameObject;
            Priority = priority;

            OnSCAwake();

            if(IsEffectGameObject) {
                gameObject.SetActive(false);
            }
        }
        public virtual void OnSCAwake() {
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCAwake", this);
        }
        /// <summary>
        /// Module 初始化 ===================================
        /// </summary>





        /// <summary>
        /// ModuleEnable ===================================
        /// </summary>
        [Obsolete("Should not use the EventFunction")]
        public void OnSCEnable() {
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCEnable", this);
        }
        /// <summary>
        /// ModuleEnable ===================================
        /// </summary>







        /// <summary>
        /// Module 启动 ===================================
        /// </summary>
        public void ModuleStart() {
            if(!IsModuleInit) {
                DebugMy.Log("Please Invoke ModuleInit First", this);
                return;
            }
            if(IsModuleStarted) {
                DebugMy.Log("ModuleStart Had Invoke", this);
                return;
            }
            
            IsModuleStarted = true;
            
            if(IsEffectGameObject) {
                gameObject.SetActive(true);
            }

            OnSCStart();
        }

        public virtual void OnSCStart() {
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCStart", this);
        }
        /// <summary>
        /// Module 启动 ===================================
        /// </summary>
        /// 






        /// <summary>
        /// Module Update ===================================
        /// </summary>
        public void ModuleUpdate() {

            if(IsModuleInit == false || IsModuleStarted == false) {
                //DebugMy.Log("Pleaase Invoke ModuleInit First", this);
                return;
            }

            OnSCUpdate();
        }
        public virtual void OnSCUpdate() {

            lock(subModuleList) {
                foreach(var Module in subModuleList) {
                    Module.ModuleUpdate();
                }
            }

            //DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCUpdate", this);
        }
        /// <summary>
        /// Module Update ===================================
        /// </summary>




        /// <summary>
        /// Module LateUpdate ===================================
        /// </summary>
        public void ModuleLateUpdate() {
            if(IsModuleInit == false || IsModuleStarted == false) {
                //DebugMy.Log("Please Invoke ModuleInit First", this);
                return;
            }

            OnSCLateUpdate();
        }
        public virtual void OnSCLateUpdate() {

            lock(subModuleList) {
                foreach(var Module in subModuleList) {
                    Module.ModuleLateUpdate();
                }
            }
            //DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCLateUpdate", this);
        }
        /// <summary>
        /// Module LateUpdate ===================================
        /// </summary>






        /// <summary>
        /// ModuleEndOfFrame ===================================
        /// </summary>
        public void ModuleEndOfFrame() {
            if(IsModuleInit == false || IsModuleStarted == false) {
                //DebugMy.Log("Please Invoke ModuleInit First", this);
                return;
            }

            OnSCFuncitonWaitForEndOfFrame();
        }
        public virtual void OnSCFuncitonWaitForEndOfFrame() {

            lock(subModuleList) {
                foreach(var Module in subModuleList) {
                    Module.ModuleEndOfFrame();
                }
            }

            //DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCFuncitonWaitForEndOfFrame", this);
        }
        /// <summary>
        /// ModuleEndOfFrame ===================================
        /// </summary>





        /// <summary>
        /// ModuleStop ===================================
        /// </summary>
        public void ModuleStop() {
            if(IsModuleInit == false) {
                DebugMy.Log("ModuleStop: Please Invoke ModuleInit First", this);
                return;
            }

            if(IsModuleStarted == false) {
                DebugMy.Log("ModuleStop: Please Invoke ModuleStart First", this);
                return;
            }

            OnSCDisable();

            IsModuleStarted = false;

            if(IsEffectGameObject) {
                gameObject.SetActive(false);
            }
        }
        public virtual void OnSCDisable() {

            lock(subModuleList) {
                foreach(var Module in subModuleList) {
                    Module.ModuleStop();
                }
            }

            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCDisable", this);
        }
        /// <summary>
        /// ModuleStop ===================================
        /// </summary>




        /// <summary>
        /// ModuleDestroy ===================================
        /// </summary>
        public void ModuleDestroy() {

            if(IsModuleInit == false) {
                DebugMy.Log("ModuleDestroy: Please Invoke ModuleInit First", this);
                return;
            }

            OnSCDestroy();

            IsModuleInit = false;
            IsModuleStarted = false;
            FatherModule = null;
            ModuleName = "";

        }
        public virtual void OnSCDestroy() {

            lock(subModuleList) {
                foreach(var Module in subModuleList) {
                    Module.ModuleDestroy();
                }
            }
            subModuleList.Clear();
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "OnSCDestroy", this);
        }
        /// <summary>
        /// ModuleDestroy ===================================
        /// </summary>











        public void AddModule(ISCModule Module, bool isEffectGameObject = true, SCModulePriority priority = SCModulePriority.Middle) {
            if(Module == null)
                return;

            Module.FatherModule = this;
            Module.ModuleInit(isEffectGameObject, priority);

            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "Module '" + Module.GetType() + "' [HashCode: " + Module.GetHashCode() + "] Add To Module '" + GetType(), this);
            lock(subModuleList) {
                subModuleList.Add(Module);
            }
        }

        public void RemoveModule(ISCModule Module) {
            if(Module == null)
                return;

            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "Module '" + Module.GetType() + "' [HashCode: " + Module.GetHashCode() + "] Remove From Module '" + GetType(), this);
            lock(subModuleList) {
                subModuleList.Remove(Module);
            }

            Module.ModuleDestroy();
            Module.FatherModule = null;
        }

        public void RemoveAllModule() {
            lock(subModuleList) {
                foreach(var item in subModuleList) {
                    item.ModuleDestroy();
                }
            }
            DebugMy.Log("[HashCode: " + GetHashCode() + "] " + "Remove All Module '" + GetType(), this);
            subModuleList.Clear();
        }

        public virtual T GetSubModule<T>() where T : ISCModule {

            lock(subModuleList) {
                foreach(var Module in subModuleList) {
                    if(typeof(T).ToString() == Module.ModuleName) {
                        DebugMy.Log("[HashCode: " + GetHashCode() + "] Module" + typeof(T).ToString() + " Found ", this);
                        return (T)Module;
                    }
                }
            }
            DebugMy.Log("[HashCode: " + GetHashCode() + "] Module" + typeof(T).ToString() + " No Found ", this);
            return default(T);
        }

        /// <summary>
        /// 返回直接父
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual T GetFatherModule<T>() where T : ISCModule {
            ISCModule father = FatherModule;

            while(father != null) {
                if(father.ModuleName == typeof(T).ToString()) {
                    DebugMy.Log("[HashCode: " + GetHashCode() + "] FatherModule" + father.ModuleName + " Found ", this);
                    return (T)father;
                }
                father = father.FatherModule;
            }
            DebugMy.Log("[HashCode: " + GetHashCode() + "] FatherModule" + typeof(T).ToString() + " Found ", this);
            return default(T);
        }


        public T Transition<T>(ISCModule data) where T : class {
            if((data as T) == null)
                throw new ArgumentException(String.Format("Invalid type: {0} passed to event expecting {1}", data.GetType(), typeof(T)));
            return data as T;
        }

    }
}
