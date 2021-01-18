using UnityEngine.EventSystems;

/// <summary>
/// IPointerHandler
/// </summary>
public interface IPointerHandler : 
    IPointerExitHandler, 
    IPointerEnterHandler, 
    IPointerDownHandler,
    IPointerClickHandler,
    IPointerUpHandler, 
    IDragHandler{
}