/// <summary>
/// Towards the object's surface
/// </summary>
public enum ObjFacingTypes
{
    /// <summary>
    /// Not rotating
    /// </summary>
    None = 0,
    /// <summary>
    /// Toward the origin
    /// </summary>
    FaceOrigin,
    /// <summary>
    /// Back to the origin
    /// </summary>
    FaceOriginReversed,
    /// <summary>
    /// Toward the front of the base class
    /// </summary>
    FaceParentFoward,
    /// <summary>
    /// Towards the rear of the base class
    /// </summary>
    FaceParentBack,
    /// <summary>
    /// Toward the top of the base class
    /// </summary>
    FaceParentUp,
    /// <summary>
    /// Toward the bottom of the base class
    /// </summary>
    FaceParentDown,
    /// <summary>
    /// Toward the center shaft
    /// </summary>
    FaceCenterAxis,
    /// <summary>
    /// Back to central shaft
    /// </summary>
    FaceCenterAxisReversed
}
