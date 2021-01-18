using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SCImage3D))]
public class SCImage3DInspector : Editor
{
    protected SCImage3D scImage3D;

    protected SerializedProperty imageTexture;
    protected SerializedProperty imageColor;
    protected SerializedProperty imageMaterial;
    protected SerializedProperty raycastTarget;
    protected SerializedProperty customColliderSize;

    [MenuItem("GameObject/SC3DUI/SCImage3D", priority = 0)]
    private static void Init()
    {
        var obj = Instantiate(Resources.Load<SCImage3D>(typeof(SCImage3D).Name));
        obj.name = (typeof(SCImage3D).Name);
        if (obj)
        {
            var parent = Selection.activeGameObject;
            obj.transform.SetParent(parent ? parent.transform : null, obj.transform);
            Selection.activeGameObject = obj.gameObject;
        }
    }

    protected virtual void OnEnable()
    {
        scImage3D = target as SCImage3D;

        imageTexture = serializedObject.FindProperty("m_Texture");
        imageColor = serializedObject.FindProperty("m_Color");
        imageMaterial = serializedObject.FindProperty("m_Material");
        raycastTarget = serializedObject.FindProperty("m_RaycastTarget");
        customColliderSize = serializedObject.FindProperty("m_CustomColliderSize");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();

        if (scImage3D != null)
        {
            scImage3D.UpdateSize();
            scImage3D.UpdateMaterial();
            scImage3D.UpdateRaycaster();
        }
    }
}
