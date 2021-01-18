using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using SC.XR.Unity.Module_InputSystem;
using UnityEngine.Events;
using System;

[AddComponentMenu("SDK/BoundingBox")]
public class BoundingBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    [Tooltip("Type of activation method for showing/hiding bounding box handles and controls")]
    private BoundingBoxActivationType activation = BoundingBoxActivationType.ActivateOnStart;
    public BoundingBoxActivationType ActivationType
    {
        get
        {
            return activation;
        }
    }

    [SerializeField]
    [Tooltip("Flatten bounds in the specified axis or flatten the smallest one if 'auto' is selected")]
    private FlattenModeType flattenAxis = FlattenModeType.DoNotFlatten;
    public FlattenModeType FlattenAxis
    {
        get
        {
            return flattenAxis;
        }
        set
        {
            flattenAxis = value;
            Redraw();
        }
    }

    [SerializeField]
    private HandleType activeHandle = ~HandleType.None;
    public HandleType ActiveHandle
    {
        get
        {
            return activeHandle;
        }
        set
        {
            activeHandle = value;
            Redraw();
        }
    }

    [AssetPreAssign("Assets/SDK/Common/StandardAssets/Materials/BoundingBox.mat", typeof(Material))]
    public Material boxFocusDisplayMat;
    [AssetPreAssign("Assets/SDK/Common/StandardAssets/Materials/BoundingBoxGrabbed.mat", typeof(Material))]
    public Material boxGrabDisplayMat;
    [AssetPreAssign("Assets/SDK/Common/StandardAssets/Materials/BoundingBoxHandleWhite.mat", typeof(Material))]
    public Material HandleMaterial;
    [AssetPreAssign("Assets/SDK/Common/StandardAssets/Materials/BoundingBoxHandleBlueGrabbed.mat", typeof(Material))]
    public Material HandleGrabMaterial;

    [Header("Scale Handles")]
    [AssetPreAssign("Assets/SDK/Common/StandardAssets/Prefabs/MRTK_BoundingBox_ScaleHandle.prefab", typeof(GameObject))]
    public GameObject CornerPrefab;
    [AssetPreAssign("Assets/SDK/Common/StandardAssets/Prefabs/MRTK_BoundingBox_ScaleHandle_Slate.prefab", typeof(GameObject))]
    public GameObject CornerSlatePrefab;
    [Tooltip("Minimum scaling allowed relative to the initial size")]
    public float scaleMinimum = 0.2f;
    [Tooltip("Maximum scaling allowed relative to the initial size")]
    public float scaleMaximum = 2.0f;
    [SerializeField]
    [Tooltip("Size of the cube collidable used in scale handles")]
    private float scaleHandleSize = 0.016f; // 1.6cm default handle size
    public float ScaleHandleSize
    {
        get
        {
            return scaleHandleSize;
        }
        set
        {
            scaleHandleSize = value;
            Redraw();
        }
    }

    [HideInInspector]
    private BoxCollider BoundBoxCollider;

    // Half the size of the current bounds
    private Vector3 currentBoundsExtents;
    public Vector3 CurrentBoundsExtents
    {
        get
        {
            return currentBoundsExtents;
        }
    }

    [Header("Rotation Handles")]
    [AssetPreAssign("Assets/SDK/Common/StandardAssets/Prefabs/MRTK_BoundingBox_RotateHandle.prefab", typeof(GameObject))]
    public GameObject SidePrefab;

    [SerializeField]
    [Tooltip("Radius of the handle geometry of rotation handles")]
    private float rotationHandleSize = 0.016f; // 1.6cm default handle size
    public float RotationHandleSize
    {
        get
        {
            return rotationHandleSize;
        }
        set
        {
            rotationHandleSize = value;
            Redraw();
        }
    }

    [SerializeField]
    [Tooltip("Check to show rotation handles for the X axis")]
    private bool showRotationHandleForX = true;
    public bool ShowRotationHandleForX
    {
        get
        {
            return showRotationHandleForX;
        }
        set
        {
            showRotationHandleForX = value;
            Redraw();
        }
    }

    [SerializeField]
    [Tooltip("Check to show rotation handles for the Y axis")]
    private bool showRotationHandleForY = true;
    public bool ShowRotationHandleForY
    {
        get
        {
            return showRotationHandleForY;
        }
        set
        {
            showRotationHandleForY = value;
            Redraw();
        }
    }

    [SerializeField]
    [Tooltip("Check to show rotation handles for the Z axis")]
    private bool showRotationHandleForZ = true;
    public bool ShowRotationHandleForZ
    {
        get
        {
            return showRotationHandleForZ;
        }
        set
        {
            showRotationHandleForZ = value;
            Redraw();
        }
    }

    [Header("AxisScale Handles")]
    public GameObject facePrefab;

    [SerializeField]
    [Tooltip("Radius of the handle geometry of rotation handles")]
    private float axisScaleHandleSize = 0.016f; // 1.6cm default handle size
    public float AxisScaleHandleSize
    {
        get
        {
            return axisScaleHandleSize;
        }
        set
        {
            axisScaleHandleSize = value;
            Redraw();
        }
    }

    [SerializeField]
    private AxisType activeAxis = ~AxisType.None;
    public AxisType ActiveAxis
    {
        get
        {
            return activeAxis;
        }
        set
        {
            activeAxis = value;
            Redraw();
        }
    }

    public Transform BoundingBoxContainer
    {
        get;
        set;
    }

    public BoundingBoxRoot BoundingBoxRoot
    {
        get;
        private set;
    }

    public CornerBoundingBoxRoot CornerBoundingBoxRoot
    {
        get;
        private set;
    }

    private SideBoundingBoxRoot SideBoundingBoxRoot
    {
        get;
        set;
    }

    private FaceBoundingBoxRoot FaceBoundingBoxRoot
    {
        get;
        set;
    }

    private List<IBoundingBoxRoot> BoundingBoxRootList
    {
        get;
        set;
    }

    [Header("Audio")]
    [SerializeField]
    public SCAudiosConfig.AudioType RotateStartAudio = SCAudiosConfig.AudioType.Manipulation_Start;
    [SerializeField]
    public SCAudiosConfig.AudioType RotateStopAudio = SCAudiosConfig.AudioType.Manipulation_End;
    [SerializeField]
    public SCAudiosConfig.AudioType ScaleStartAudio = SCAudiosConfig.AudioType.Manipulation_Start;
    [SerializeField]
    public SCAudiosConfig.AudioType ScaleStopAudio = SCAudiosConfig.AudioType.Manipulation_End;

    [Header("Events")]
    /// <summary>
    /// Event that gets fired when interaction with a rotation handle starts.
    /// </summary>
    public UnityEvent RotateStarted = new UnityEvent();
    /// <summary>
    /// Event that gets fired when interaction with a rotation handle stops.
    /// </summary>
    public UnityEvent RotateStopped = new UnityEvent();
    /// <summary>
    /// Event that gets fired when interaction with a scale handle starts.
    /// </summary>
    public UnityEvent ScaleStarted = new UnityEvent();
    /// <summary>
    /// Event that gets fired when interaction with a scale handle stops.
    /// </summary>
    public UnityEvent ScaleStopped = new UnityEvent();

    #region Class & Enum Define

    /// <summary>
    /// Enum which describes whether a BoundingBox handle which has been grabbed, is 
    /// a Rotation Handle (sphere) or a Scale Handle( cube)
    /// </summary>
    [Flags]
    public enum HandleType
    {
        None = 1 << 0,
        Rotation = 1 << 1,
        Scale = 1 << 2,
        AxisScale = 1 << 3,
    }

    [Flags]
    public enum AxisType
    {
        None = 1 << 0,
        X = 1 << 1,
        Y = 1 << 2,
        Z = 1 << 3,
        NX = 1 << 4,
        NY = 1 << 5,
        NZ = 1 << 6,
    }

    /// <summary>
    /// Enum which describes how an object's BoundingBox is to be flattened.
    /// </summary>
    public enum FlattenModeType
    {
        DoNotFlatten = 0,
        /// <summary>
        /// Flatten the X axis
        /// </summary>
        FlattenX,
        /// <summary>
        /// Flatten the Y axis
        /// </summary>
        FlattenY,
        /// <summary>
        /// Flatten the Z axis
        /// </summary>
        FlattenZ,
        /// <summary>
        /// Flatten the smallest relative axis if it falls below threshold
        /// </summary>
        FlattenAuto,
    }

    /// <summary>
    /// This enum defines how the BoundingBox gets activated
    /// </summary>
    public enum BoundingBoxActivationType
    {
        ActivateOnStart = 0,
        ActivateByPointer,
    }

    public class Handle
    {
        /// <summary>
        /// Handle Type
        /// </summary>
        public HandleType type;
        /// <summary>
        /// Handle Root Gameobject
        /// </summary>
        public Transform root;
        /// <summary>
        /// Handle bounds
        /// </summary>
        public Bounds bounds;

        public Vector3 localPosition;
        public Transform visualsScale;
        public GameObject visual;

        public void SetActive(bool active)
        {
            root.gameObject.SetActive(active);
        }
    }

    #endregion

    #region Unity Lifecycle Function

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    private void OnValidate()
    {
        Redraw();
    }

    #endregion

    private void Init()
    {
        CreatBoundingBoxRoot(flattenAxis);

        BoundingBoxRoot = new BoundingBoxRoot(this);
        CornerBoundingBoxRoot = new CornerBoundingBoxRoot(this);
        SideBoundingBoxRoot = new SideBoundingBoxRoot(this);
        FaceBoundingBoxRoot = new FaceBoundingBoxRoot(this);

        BoundingBoxRoot.Init();
        CornerBoundingBoxRoot.Init();
        SideBoundingBoxRoot.Init();
        FaceBoundingBoxRoot.Init();

        if (BoundingBoxRootList == null)
        {
            BoundingBoxRootList = new List<IBoundingBoxRoot>();
            BoundingBoxRootList.Add(BoundingBoxRoot);
            BoundingBoxRootList.Add(CornerBoundingBoxRoot);
            BoundingBoxRootList.Add(SideBoundingBoxRoot);
            BoundingBoxRootList.Add(FaceBoundingBoxRoot);
        }

        if (ActivationType == BoundingBoxActivationType.ActivateOnStart)
        {
            SetVisibility(true);
        }
        else
        {
            SetVisibility(false);
        }
    }

    private void CreatBoundingBoxRoot(FlattenModeType flattenMode)
    {
        RecaculateBounds();
    }

    private void Redraw()
    {
        if (BoundingBoxRootList == null)
        {
            return;
        }

        RecaculateBounds();
        for (int i = 0; i < BoundingBoxRootList.Count; i++)
        {
            IBoundingBoxRoot boundingBoxRoot = BoundingBoxRootList[i];
            boundingBoxRoot.ReDraw();
        }
    }

    private void RecaculateBounds()
    {
        // Make sure that the bounds of all child objects are up to date before we compute bounds
        Physics.SyncTransforms();

        BoundBoxCollider = GetComponent<BoxCollider>();
        if (BoundBoxCollider == null)
        {
            Debug.Log("Error! Please Add BoxCollider And Adjust Size For BoundingBoxGameobject");
            return;
        }

        // Store current rotation then zero out the rotation so that the bounds
        // are computed when the object is in its 'axis aligned orientation'.
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.identity;
        Physics.SyncTransforms(); // Update collider bounds

        currentBoundsExtents = BoundBoxCollider.bounds.extents;

        // After bounds are computed, restore rotation...
        transform.rotation = currentRotation;
        Physics.SyncTransforms();

        if (currentBoundsExtents != Vector3.zero)
        {
            if (FlattenAxis == FlattenModeType.FlattenAuto)
            {
                float min = Mathf.Min(currentBoundsExtents.x, Mathf.Min(currentBoundsExtents.y, currentBoundsExtents.z));
                flattenAxis = (min == currentBoundsExtents.x) ? FlattenModeType.FlattenX :
                    ((min == currentBoundsExtents.y) ? FlattenModeType.FlattenY : FlattenModeType.FlattenZ);
            }

            currentBoundsExtents.x = (flattenAxis == FlattenModeType.FlattenX) ? 0.0f : currentBoundsExtents.x;
            currentBoundsExtents.y = (flattenAxis == FlattenModeType.FlattenY) ? 0.0f : currentBoundsExtents.y;
            currentBoundsExtents.z = (flattenAxis == FlattenModeType.FlattenZ) ? 0.0f : currentBoundsExtents.z;

            Transform existContainerTransform = this.transform.Find(GetType().ToString());
            if (existContainerTransform != null)
            {
#if UNITY_EDITOR
                GameObject.Destroy(existContainerTransform.gameObject);
#else
                GameObject.DestroyImmediate(existContainerTransform.gameObject);
#endif
            }

            BoundingBoxContainer = new GameObject(GetType().ToString()).transform;
            BoundingBoxContainer.parent = transform;
            BoundingBoxContainer.position = BoundBoxCollider.bounds.center;
            BoundingBoxContainer.localRotation = Quaternion.identity;
        }
    }

    public void SetVisibility(bool isVisible)
    {
        for (int i = 0; i < BoundingBoxRootList.Count; i++)
        {
            IBoundingBoxRoot boundingBoxRoot = BoundingBoxRootList[i];
            boundingBoxRoot.SetVisible(isVisible);
        }
    }

    public void SetHighLight(Transform activeHandle, bool hideOtherHandle)
    {
        for (int i = 0; i < BoundingBoxRootList.Count; i++)
        {
            IBoundingBoxRoot boundingBoxRoot = BoundingBoxRootList[i];
            boundingBoxRoot.SetHighLight(activeHandle, hideOtherHandle);
        }
    }

#region BoundingBox PointerEvent

    public void OnPointerEnter(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }

        if (scPointEventData.DownPressGameObject == null)
        {
            SetVisibility(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }

        if (scPointEventData.DownPressGameObject == null)
        {
            if (activation == BoundingBoxActivationType.ActivateOnStart)
            {
                SetVisibility(true);
            }
            else
            {
                SetVisibility(false);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }
        SetHighLight(eventData.pointerCurrentRaycast.gameObject.transform, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        SCPointEventData scPointEventData = eventData as SCPointEventData;
        if (scPointEventData == null)
        {
            return;
        }

        if (activation == BoundingBoxActivationType.ActivateOnStart)
        {
            SetVisibility(true);
        }
        else
        {
            SetVisibility(true);
        }
    }

#endregion
}
