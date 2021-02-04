
using UnityEngine;
using UnityEditor;
using Unity.Collections;

[ExecuteAlways]//Script will run in any circumstance
public class SCGridLayoutGroup : SCBaseLayoutGroup
{
    /// <summary>
    /// The layout of the set of type
    /// </summary>
    [SerializeField]
    private LayoutTypes layoutType = LayoutTypes.Horizontal;
    public LayoutTypes LayoutType 
    {
        get { return layoutType; }
        set { layoutType = value; }
    }
    /// <summary>
    /// Less than a row when the child object alignment
    /// </summary>
    [SerializeField]
    private GroupHorizontalAlignment groupHorizontalAlign = GroupHorizontalAlignment.Left;
    public GroupHorizontalAlignment GroupHorizontalAlign
    {
        get { return groupHorizontalAlign; }
        set { groupHorizontalAlign = value; }
    }
    /// <summary>
    /// Less than a column when the child object alignment
    /// </summary>
    [SerializeField]
    private GroupVerticalAlignment groupVerticalAlign = GroupVerticalAlignment.Top;
    public GroupVerticalAlignment GroupVerticalAlign
    {
        get { return groupVerticalAlign; }
        set { groupVerticalAlign = value; }
    }
    /// <summary>
    /// The child object types of formation
    /// </summary>
    [SerializeField]
    private GroupArrayTypes groupArrayType =
        GroupArrayTypes.Plane;
    public GroupArrayTypes GroupArrayType {
        get { return groupArrayType; }
        set { groupArrayType = value; }
    }
    /// <summary>
    /// The position of the group anchor point relative to the base class
    /// </summary>
    [SerializeField]
    private AnchorRelativeBase anchorLayout = AnchorRelativeBase.MiddleCenter;
    public AnchorRelativeBase AnchorLayout
    {
        get { return anchorLayout; }
        set { anchorLayout = value; }
    }
    /// <summary>
    /// The child object oriented
    /// </summary>
    [SerializeField]
    private ObjFacingTypes facingType = ObjFacingTypes.None;
    public ObjFacingTypes FacingType
    {
        get { return facingType; }
        set { facingType = value; }
    }
    /// <summary>
    /// The number of columns
    /// </summary>
    [SerializeField]
    private int columns = 2;
    public int Columns {
        get { return columns; }
        set {
            if (LayoutType == LayoutTypes.Horizontal)
            {
                return;
            }
            columns = value; }
    }
    /// <summary>
    /// The number of rows
    /// </summary>
    [SerializeField]
    private int rows = 2;
    public int Rows
    {
        get { return rows; }
        set
        {
            if (LayoutType == LayoutTypes.Vertical)
            {
                return;
            }
            rows = value;
        }
    }
    /// <summary>
    /// Horizontal spacing
    /// </summary>
    [SerializeField]
    private float spaceX = 2f;
    public float SpaceX {
        get { return spaceX; }
        set { spaceX = value; }
    }

    /// <summary>
    /// The vertical spacing
    /// </summary>
    [SerializeField]
    private float spaceY = 2f;
    public float SpaceY
    {
        get { return spaceY; }
        set { spaceY = value; }
    }

    [SerializeField]
    [Range(-100f, 100f)]
    private float childOffsetX = 0f;
    public float ChildOffsetX
    {
        get { return childOffsetX; }
        set { childOffsetX = value; }
    }
    [SerializeField]
    [Range(-100f, 100f)]
    private float childOffsetY= 0f;
    public float ChildOffsetY
    {
        get { return childOffsetY; }
        set { childOffsetY = value; }
    }
    /// <summary>
    /// The location of the child object Z axis relative to the base class
    /// </summary>
    [SerializeField]
    [Range(-100f, 100f)]
    private float childOffsetZ = 0f;
    public float ChildOffsetZ
    {
        get { return childOffsetZ; }
        set { childOffsetZ = value; }
    }
    /// <summary>
    /// The radius 
    /// </summary>
    [SerializeField]
    [Range(0.05f,200)]
    private float radius = 2f;
    public float Radius
    {
        get { return radius; }
        set { radius = value; }
    }
    /// <summary>
    /// Radiation Angle
    /// </summary>
    [SerializeField]
    [Range(5f, 360f)]
    private float radialRange = 180f;
    public float RadialRange {
        get { return radialRange; }
        set { radialRange = value; }
    }
    /// <summary>
    /// If the anchor point with the shaft alignment
    /// </summary>
    [SerializeField]
    private bool isAnchorWithAxis = false;
    public bool IsAnchorWithAxis
    {
        get { return isAnchorWithAxis; }
        set { isAnchorWithAxis = value; }
    }


    private GroupObj tempobj;
    private Vector3[] childPos;
    private Vector3 newPos;
    private int objCount;
    private int rowMax;
    private int colMax;
    private float acnhoroffsetX;
    private float anchoroffsetY;
    private float shortOffsetX;
    private float shortOffsetY;

    private void Init()//The initialization value
    {
        childPos = new Vector3[ObjList.Count];
        newPos = Vector3.zero;
        objCount = 0;
        rowMax = 0;
        colMax = 0;
        acnhoroffsetX = 0;
        anchoroffsetY = 0;
        shortOffsetX = 0;
        shortOffsetY = 0;
    }
    protected override void LayoutChildren()
    {
        Init(); 
        SetObjPos();
        ArrayType();
    }

    private void Update()
    {

    }
    private void SetObjPos()//The location of the object
    {       
        switch (LayoutType)     
        {
            case LayoutTypes.Horizontal:
                colMax = Columns;
                rowMax= Mathf.CeilToInt((float)ObjList.Count / Columns);
                break;
            case LayoutTypes.Vertical:
                rowMax = Rows;
                colMax = Mathf.CeilToInt((float)ObjList.Count / Rows);
                break;
        }

        acnhoroffsetX = (colMax * 0.5f * SpaceX);
        if (AnchorLayout == AnchorRelativeBase.LowerLeft || AnchorLayout == AnchorRelativeBase.UpperLeft || AnchorLayout == AnchorRelativeBase.MiddleLeft)
        {
            acnhoroffsetX = IsAnchorWithAxis ? 0.5f * SpaceX : 0;
        }
        else if (AnchorLayout == AnchorRelativeBase.LowerRight || AnchorLayout == AnchorRelativeBase.UpperRight || AnchorLayout == AnchorRelativeBase.MiddleRight)
        {
            acnhoroffsetX = IsAnchorWithAxis ? (colMax - 0.5f) * SpaceX : colMax * SpaceX;
        }

        anchoroffsetY = (rowMax * 0.5f * SpaceY);
        if (AnchorLayout == AnchorRelativeBase.UpperLeft || AnchorLayout == AnchorRelativeBase.UpperCenter || AnchorLayout == AnchorRelativeBase.UpperRight)
        {
            anchoroffsetY = IsAnchorWithAxis ? 0.5f * SpaceY : 0;
        }
        else if (AnchorLayout == AnchorRelativeBase.LowerLeft || AnchorLayout == AnchorRelativeBase.LowerCenter || AnchorLayout == AnchorRelativeBase.LowerRight)
        {
            anchoroffsetY = IsAnchorWithAxis ? (rowMax - 0.5f) * SpaceY : rowMax * SpaceY;
        }
        if (LayoutType == LayoutTypes.Horizontal)
        {
            for (int i = 0; i < rowMax; i++)
            {
                for (int j = 0; j < colMax; j++)
                {
                    if (i==rowMax-1)
                    {
                        if (GroupHorizontalAlign == GroupHorizontalAlignment.Left)
                        {
                            shortOffsetX = 0;
                        }
                        else if (GroupHorizontalAlign == GroupHorizontalAlignment.Center)
                        {
                            shortOffsetX = SpaceX * ((colMax - (ObjList.Count % colMax)) % colMax) * 0.5f;
                        }
                        else if (GroupHorizontalAlign == GroupHorizontalAlignment.Right)
                        {
                            shortOffsetX = SpaceX * ((colMax - (ObjList.Count % colMax)) % colMax);
                        }
                    }

                    if (objCount< ObjList.Count)
                    {
                        childPos[objCount].Set(0.5f * SpaceX * (1 + 2 * j) - acnhoroffsetX + shortOffsetX+ ChildOffsetX, (0.5f * SpaceY * (-1 - 2 * i)) + anchoroffsetY + shortOffsetY+ChildOffsetY, 0.0f);
                    }
                    objCount++;
                }
            }
        }
        else
        {
            for (int i = 0; i < colMax; i++)
            {
                for (int j = 0; j < rowMax; j++)
                {
                    if (i==colMax-1)
                    {
                        if (GroupVerticalAlign == GroupVerticalAlignment.Top)
                        {
                            shortOffsetY = 0;
                        }
                        else if (GroupVerticalAlign == GroupVerticalAlignment.Middle)
                        {
                            shortOffsetY = -SpaceY * ((rowMax - (ObjList.Count % rowMax)) % rowMax) * 0.5f;
                        }
                        else if (GroupVerticalAlign == GroupVerticalAlignment.Bottom)
                        {
                            shortOffsetY = -SpaceY * ((rowMax - (ObjList.Count % rowMax)) % rowMax);
                        }
                    }
                    if (objCount< ObjList.Count)
                    {
                        childPos[objCount].Set(0.5f * SpaceX * (1 + 2 * i )- acnhoroffsetX + shortOffsetX+ ChildOffsetX, (0.5f * SpaceY * ( - 1 - 2 * j))+ anchoroffsetY + shortOffsetY+childOffsetY, 0.0f);
                    }
                    objCount++;
                }
            }
        }
    }

    private void ArrayType()//Give the child object position
    {
        switch (GroupArrayType) {
            case GroupArrayTypes.Plane:
                for (int  i = 0;  i < ObjList.Count;  i++)
                {
                    tempobj = ObjList[i];
                    newPos = childPos[i];
                    newPos.z = ChildOffsetZ;
                    tempobj.Transform.localPosition = newPos;
                    ObjFacing(tempobj);
                    ObjList[i] = tempobj;
                }
                break;
            case GroupArrayTypes.Cylinder:
                for (int i = 0; i < ObjList.Count; i++)
                {
                    tempobj = ObjList[i];
                    newPos = CylinderArray(childPos[i], radius);
                    tempobj.Transform.localPosition = newPos;
                    ObjFacing(tempobj);
                    ObjList[i] = tempobj;
                }
                break;
            case GroupArrayTypes.Radial:
                int tempColumn=0;
                int tempRow=1;
                for (int i = 0; i < ObjList.Count ; i++)
                {
                    tempobj = ObjList[i];
                    newPos = RadialArray(childPos[i], radius, radialRange, tempRow, rowMax, tempColumn, colMax);
                    if (tempColumn==columns-1)
                    {
                        tempColumn = 0;
                        tempRow++;
                    }
                    else
                    {
                        tempColumn++;
                    }

                    tempobj.Transform.localPosition = newPos;
                    ObjFacing(tempobj);
                    ObjList[i] = tempobj;
                }
                break;
            case GroupArrayTypes.Sphere:

                for (int i = 0; i < ObjList.Count ; i++)
                {
                    tempobj = ObjList[i];
                    newPos = SphereArray(childPos[i], radius);
                    tempobj.Transform.localPosition = newPos;
                    ObjFacing(tempobj);
                    ObjList[i] = tempobj;
                }
                break;
            case GroupArrayTypes.Round:
                for (int i = 0; i < ObjList.Count; i++)
                {
                    tempobj = ObjList[i];
                    newPos = RoundArray( childPos[i],radius, i+1,ObjList.Count);
                        newPos.Set(newPos.x+ChildOffsetX, newPos.y + ChildOffsetY, newPos.z +ChildOffsetZ);                                 
                    tempobj.Transform.localPosition = newPos;
                    ObjFacing(tempobj);
                    ObjList[i] = tempobj;
                }
                break;
        }
        
    }

    private Vector3 CylinderArray(Vector3 objPos,float radius)
    {
        float circ = 2.0f * Mathf.PI * radius;
        float xAngle = (objPos.x / circ) * 360f;
        objPos.Set(0.0f, objPos.y, radius);
        Quaternion rot = Quaternion.Euler(0.0f, xAngle, 0.0f);
        objPos = rot * objPos;

        return objPos;
    }

    private Vector3 SphereArray(Vector3 objPos, float radius)
    {
        float circ = 2.0f * Mathf.PI * radius;
        float XAngle = (objPos.x / circ) * 360f;
        float YAngle = -(objPos.y / circ) * 360f;
        objPos.Set(0.0f, 0.0f, radius);
        Quaternion rot = Quaternion.Euler(YAngle, XAngle, 0.0f);
        objPos = rot * objPos;
        return objPos;
    }
    private Vector3 RadialArray(Vector3 objPos, float radius ,float  radialRange,int row,int rowMax,int column,int colMax)
    {
        float radialSpaceAngle = radialRange / colMax;
        objPos.Set(0.0f, 0.0f, (radius / rowMax) * row);
        float YAngle= radialSpaceAngle * (column - (colMax * 0.5f)) + (radialSpaceAngle * .5f);

        Quaternion rot = Quaternion.Euler(0.0f, YAngle, 0.0f);
        objPos = rot * objPos;

        return objPos;
    }
    private Vector3 RoundArray(Vector3 objPos , float radius,int i,int totalCount)
    {        
            float angle = (360f *i )/ totalCount;
            float x =  radius * Mathf.Cos(angle* Mathf.PI / 180f);
            float y =  radius * Mathf.Sin(angle* Mathf.PI / 180f);
            objPos.Set(x,0,y);       
        return objPos;
    }


    private void ObjFacing(GroupObj obj)//The child object toward
    {
        Vector3 centerPos;
        Vector3 FacingNode;
        switch (FacingType)
        {
            case ObjFacingTypes.None:
                obj.Transform.transform.localEulerAngles = Vector3.zero;
                break;
            case ObjFacingTypes.FaceOrigin:
                obj.Transform.rotation = Quaternion.LookRotation(obj.Transform.position-transform.position,transform.up);
                break;
            case ObjFacingTypes.FaceOriginReversed:
                obj.Transform.rotation = Quaternion.LookRotation(transform.position- obj.Transform.position,transform.up);
                break;
            case ObjFacingTypes.FaceCenterAxis:
                centerPos = Vector3.Project(obj.Transform.position-transform.position,transform.up);//Returns a vector projection on the specified axis
                FacingNode = transform.position + centerPos;
                obj.Transform.rotation = Quaternion.LookRotation(obj.Transform.position - FacingNode, transform.up);
                break;
            case ObjFacingTypes.FaceCenterAxisReversed:
                centerPos = Vector3.Project(obj.Transform.position - transform.position, transform.up);//Returns a vector projection on the specified axis
                FacingNode = transform.position + centerPos;
                obj.Transform.rotation = Quaternion.LookRotation(FacingNode- obj.Transform.position  , transform.up);
                break;
            case ObjFacingTypes.FaceParentDown:
                obj.Transform.forward = transform.rotation * Vector3.down;
                break;
            case ObjFacingTypes.FaceParentUp:
                obj.Transform.forward = transform.rotation * Vector3.up;
                break;
            case ObjFacingTypes.FaceParentFoward:
                obj.Transform.forward = transform.rotation * Vector3.forward;
                break;
            case ObjFacingTypes.FaceParentBack:
                obj.Transform.forward = transform.rotation * Vector3.back;
                break;
           
        } 

    }

    private void setNull()
    {
        tempobj = null;
        childPos = null;
    }
    private void OnDestroy()
    {
        setNull();
    }
}
