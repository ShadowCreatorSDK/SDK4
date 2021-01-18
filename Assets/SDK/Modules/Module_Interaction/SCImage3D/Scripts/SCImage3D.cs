using UnityEngine;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class SCImage3D : SCUI3D
{
    private RectTransform m_RectTransform;
    private RectTransform RectTransform
    {
        get
        {
            if (m_RectTransform == null)
            {
                m_RectTransform = this.GetComponent<RectTransform>();
            }
            return m_RectTransform;
        }
    }

    /// <summary>
    /// 纹理
    /// </summary>
    [SerializeField]
    private Texture2D m_Texture = null;

    public Texture2D Texture
    {
        get => m_Texture;
        set
        {
            m_Texture = value;
            MarkUIDirty();
        }
    }

    /// <summary>
    /// 颜色
    /// </summary>
    [SerializeField]
    private Color m_Color = Color.white;

    public Color Color
    {
        get => m_Color;
        set
        {
            m_Color = value;
            MarkUIDirty();
        }
    }

    /// <summary>
    /// 材质
    /// </summary>
    [SerializeField]
    private Material m_Material;

    public Material Material
    {
        get => m_Material;
        set
        {
            m_Material = value;
            MarkUIDirty();
        }
    }

    /// <summary>
    /// 是否可以被交互
    /// </summary>
    [SerializeField]
    private bool m_RaycastTarget = true;

    public bool RaycastTarget
    {
        get => m_RaycastTarget;
        set
        {
            m_RaycastTarget = value;
            MarkUIDirty();
        }
    }

    /// <summary>
    /// 是否使用自定义ColliderSize
    /// </summary>
    [SerializeField]
    private bool m_CustomColliderSize = false;

    public bool CustomColliderSize
    {
        get => m_CustomColliderSize;
        set
        {
            m_CustomColliderSize = value;
            MarkUIDirty();
        }
    }

    public override void RebuildUI()
    {
        UpdateSize();
        UpdateMaterial();
        UpdateRaycaster();
    }

    public void UpdateSize()
    {
        MeshFilter meshFilter = this.GetComponent<MeshFilter>();
        Vector2 pivot = RectTransform.pivot;
        Vector2 pivotOffset = new Vector2((0.5f - pivot.x) * RectTransform.rect.width, (0.5f - pivot.y) * RectTransform.rect.height);
        meshFilter.sharedMesh = GenerateMesh(RectTransform.rect.size);
        if (!m_CustomColliderSize)
        {
            BoxCollider collider = this.GetComponent<BoxCollider>();
            float colliderSizeZ = collider.size.z;
            float colliderCenterZ = collider.center.z;
            collider.center = new Vector3(pivotOffset.x, pivotOffset.y, colliderCenterZ);
            collider.size = new Vector3(RectTransform.rect.width, RectTransform.rect.height, colliderSizeZ);
        }
    }

    public void UpdateMaterial()
    {
        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        if (m_Material == null)
        {
            renderer.sharedMaterial = Resources.Load<Material>(SCUI3DGlobalConstant.SC_3DUI_MATERIAL_PATH);
        }

        if (m_Material != null)
        {
            renderer.sharedMaterial = m_Material;
        }

        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        if (m_Texture != null)
        {
            materialPropertyBlock.SetTexture(SCUI3DGlobalConstant.SC_TEXTURE_PROPERTY_NAME, m_Texture);
        }
        materialPropertyBlock.SetColor(SCUI3DGlobalConstant.SC_COLOR_PROPERTY_NAME, m_Color);
        renderer.SetPropertyBlock(materialPropertyBlock);
    }

    public void UpdateRaycaster()
    {
        BoxCollider collider = this.GetComponent<BoxCollider>();
        collider.enabled = m_RaycastTarget;
    }

    private Mesh GenerateMesh(Vector2 size)
    {
        Vector2 pivot = RectTransform.pivot;
        Vector2 pivotOffset = new Vector2((0.5f - pivot.x) * size.x, (0.5f - pivot.y) * size.y);
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[4]
        {
            new Vector3(-size.x / 2f + pivotOffset.x, -size.y / 2f + pivotOffset.y, 0f),
            new Vector3(-size.x / 2f + pivotOffset.x, size.y / 2f + pivotOffset.y, 0f),
            new Vector3(size.x / 2f + pivotOffset.x, size.y /2f + pivotOffset.y, 0f),
            new Vector3(size.x / 2f + pivotOffset.x, -size.y / 2f + pivotOffset.y, 0f)
        };

        mesh.uv = new Vector2[4]
        {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f)
        };

        mesh.triangles = new int[6]
        {
            0, 1, 2,
            0, 2, 3
        };

        return mesh;
    }

    protected void OnRectTransformDimensionsChange()
    {
        MarkUIDirty();
    }

    protected void OnValidate()
    {
        MarkUIDirty();
    }
}
