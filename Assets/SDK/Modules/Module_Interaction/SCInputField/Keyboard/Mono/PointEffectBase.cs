using UnityEngine;
using UnityEngine.EventSystems;

namespace SC {
    public abstract class PointEffectBase : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerClickHandler,
        IDragHandler,
        IPointerEnterHandler,
        IPointerExitHandler {
        public virtual void OnPointerClick( PointerEventData eventData ) { }

        public virtual void OnPointerEnter( PointerEventData eventData ) { }

        public virtual void OnPointerExit( PointerEventData eventData ) { }

        public virtual void OnPointerUp( PointerEventData eventData ) { }

        public virtual void OnPointerDown( PointerEventData eventData ) { }

        public virtual void OnDrag( PointerEventData eventData ) { }

        /// <summary>
        /// Click后半段
        /// </summary>
        /// <param name="eventData"></param>
        public virtual void ClickFinish() { }

        protected virtual void Awake() { }
        protected virtual void OnEnable() { }
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void OnDisable() { }
        protected virtual void OnDestroy() { }


    }
}