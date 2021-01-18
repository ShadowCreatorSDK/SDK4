using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class SDKSettings : EditorWindow {


    [InitializeOnLoadMethod]
    static void CheckSettings() {
        if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) {

            if(!IsIgnor && IsApply() == false) {
                Init();  
            } 
        } 
    }

    static EditorWindow window;

   [MenuItem("SDK/ProjectSettings")]
    static void Init() {
        if(window == null) {
            window = EditorWindow.GetWindow(typeof(SDKSettings));
            window.autoRepaintOnSceneChange = true;
            window.minSize = new Vector2(720, 420);
            window.maxSize = new Vector2(720, 420);
        }
    }
    static bool isQualityApply = true;
    static bool isPlayerApply = true; 
    void OnGUI() {
        GUILayout.Space(10);
        NoticWindow();
        
        if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android) {
            return;
        }

        GUILayout.Space(10);
        isQualityApply = QualityWindow();
        GUILayout.Space(10);
        isPlayerApply = PlayerWindow();


        if(IsApply() == false) {

            GUILayout.Space(60);
            ApplyWindow();
        }

    }

    static bool IsApply() {
        if(EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android) {
            return true;
        }

        if(PlayerCheck() && QualityCheck())
            return true;

        return false;
    }

    static bool IsIgnor { get { return GetAssetDataBase(); } }
    static void Ignor() {
        SaveAssetDataBase(true);
    }
    static string assetPath = "Assets/SDK/Editor/BuildSettings/Parm.asset";
    static void SaveAssetDataBase(bool isIgnor) {
        ParmScriptableObject asset;
        if(File.Exists(assetPath)) {
            asset = AssetDatabase.LoadAssetAtPath<ParmScriptableObject>(assetPath);
        } else {
            asset =(ParmScriptableObject) CreateInstance("ParmScriptableObject"); 
            AssetDatabase.CreateAsset(asset, assetPath);
        }
        asset.IsIgnor = isIgnor;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();//must Refresh
    }

    static bool GetAssetDataBase() {
        ParmScriptableObject asset;
        if(File.Exists(assetPath)) {
            asset = AssetDatabase.LoadAssetAtPath<ParmScriptableObject>(assetPath);
            return asset.IsIgnor;
        } 
        return false;  
    }
     
    static void NoticWindow() {
        GUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();

        GUIStyle styleNoticeText = new GUIStyle();
        styleNoticeText.alignment = TextAnchor.MiddleCenter;
        styleNoticeText.fontSize = 14;
        styleNoticeText.fontStyle = FontStyle.Bold;

        if(EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android) {
            GUILayout.Label("Notice: Recommended Project settings for SDK", styleNoticeText);
        } else {
            GUILayout.Label("This Only Effect When Platform Select Android", styleNoticeText);
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.Space(20);
    }

    void ApplyWindow() { 
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(100));

        GUIStyle styleApply = new GUIStyle("LargeButton");
        styleApply.alignment = TextAnchor.MiddleCenter; 
        if(GUILayout.Button("Apply", styleApply)) {
            QualitySet(); 
            PlayerSet();
        }
        
        EditorGUILayout.LabelField("", GUILayout.Width(100));

        GUIStyle style1Apply = new GUIStyle("LargeButton");
        styleApply.alignment = TextAnchor.MiddleCenter;
        if(GUILayout.Button("Ignor", style1Apply)) {
            Ignor();
            window.Close();
            Close();
        }

        EditorGUILayout.LabelField("", GUILayout.Width(100));
        //if(GUILayout.Button(strBtnApply[(int)elanguage], styleApply, GUILayout.Width(100), GUILayout.Height(30))) {
        //    EditorApplication.delayCall += OnClickApply;
        //}
        //styleApply = null;

        //EditorGUILayout.LabelField("", GUILayout.Width(200));
        //if(GUILayout.Button("xxx", GUILayout.Width(100), GUILayout.Height(30))) {

        //}
        EditorGUILayout.EndHorizontal();
    }

    #region ProjectSettings->Player

    static bool PlayerWindow() {
        bool isApply = true;

        GUILayout.Label("PlayerSettings", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Minimum API Level:API24(or higher)");

        if(PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel24) {

            GUIStyle styleSlide = new GUIStyle();
            styleSlide.normal.textColor = Color.red;
            GUILayout.Label("Failed", styleSlide);
            isApply = false;
        } else {

            GUIStyle styleApplied = new GUIStyle();
            styleApplied.normal.textColor = Color.green;
            GUILayout.Label("Applied", styleApplied);
        }

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("APICompatibilityLevel:.NET4.x");

        if(PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.Android) != ApiCompatibilityLevel.NET_4_6) {

            GUIStyle styleSlide = new GUIStyle();
            styleSlide.normal.textColor = Color.red;
            GUILayout.Label("Failed", styleSlide);
            isApply = false;
        } else {

            GUIStyle styleApplied = new GUIStyle();
            styleApplied.normal.textColor = Color.green;
            GUILayout.Label("Applied", styleApplied);
        }

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Multithreaded Rendering:False"); 

        if(PlayerSettings.GetMobileMTRendering(BuildTargetGroup.Android)==true) {

            GUIStyle styleSlide = new GUIStyle();
            styleSlide.normal.textColor = Color.red;
            GUILayout.Label("Failed", styleSlide);
            isApply = false;

        } else {

            GUIStyle styleApplied = new GUIStyle();
            styleApplied.normal.textColor = Color.green;
            GUILayout.Label("Applied", styleApplied);

        }

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Graphics APIs:Only OpenGLES3");
        
        UnityEngine.Rendering.GraphicsDeviceType[] gapi = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);

        if(gapi.Length != 1 || gapi[0] != UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3) {

            GUIStyle styleSlide = new GUIStyle();
            styleSlide.normal.textColor = Color.red;
            GUILayout.Label("Failed", styleSlide);
            isApply = false;
        } else {

            GUIStyle styleApplied = new GUIStyle();
            styleApplied.normal.textColor = Color.green;
            GUILayout.Label("Applied", styleApplied);
        }

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();

        int staticBatchingValue = 0;
        int dynamicBatchingValue = 0;
        Type playerSettingsType = typeof(PlayerSettings);
        MethodInfo method = playerSettingsType.GetMethod("GetBatchingForPlatform",BindingFlags.NonPublic | BindingFlags.Static);
        object[] param = new object[] { BuildTarget.Android, staticBatchingValue, dynamicBatchingValue };
        method.Invoke(null, param);

        GUILayout.Label("Dynamic Batching:Enable");
        if ((int)param[2] != 1)
        {
            GUIStyle styleSlide = new GUIStyle();
            styleSlide.normal.textColor = Color.red;
            GUILayout.Label("Failed", styleSlide);
            isApply = false;
        }
        else
        {
            GUIStyle styleApplied = new GUIStyle();
            styleApplied.normal.textColor = Color.green;
            GUILayout.Label("Applied", styleApplied);
        }

        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Asset Serialization:ForceText");
        if (EditorSettings.serializationMode != SerializationMode.ForceText)
        {
            GUIStyle styleSlide = new GUIStyle();
            styleSlide.normal.textColor = Color.red;
            GUILayout.Label("Failed", styleSlide);
            isApply = false;
        }
        else
        {

            GUIStyle styleApplied = new GUIStyle();
            styleApplied.normal.textColor = Color.green;
            GUILayout.Label("Applied", styleApplied);
        }
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Orientation:Landscape Left");
        if(PlayerSettings.defaultInterfaceOrientation != UIOrientation.LandscapeLeft) {

            GUIStyle styleSlide = new GUIStyle();
            styleSlide.normal.textColor = Color.red;
            GUILayout.Label("Failed", styleSlide);
            isApply = false;
        } else {

            GUIStyle styleApplied = new GUIStyle();
            styleApplied.normal.textColor = Color.green;
            GUILayout.Label("Applied", styleApplied);
        }
        EditorGUILayout.EndHorizontal();

        return isApply;
         
    }
    static bool PlayerCheck() {
        bool isApply = true;
        if(PlayerSettings.Android.minSdkVersion < AndroidSdkVersions.AndroidApiLevel24) {
            isApply = false;
        }
        if(PlayerSettings.GetApiCompatibilityLevel(BuildTargetGroup.Android) != ApiCompatibilityLevel.NET_4_6) {
            isApply = false;
        }
        if(PlayerSettings.GetMobileMTRendering(BuildTargetGroup.Android) == true) {
            isApply = false;
        }

        UnityEngine.Rendering.GraphicsDeviceType[] gapi = PlayerSettings.GetGraphicsAPIs(BuildTarget.Android);

        if(gapi.Length != 1 || gapi[0] != UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3) {
            isApply = false;
        }

        if(PlayerSettings.defaultInterfaceOrientation != UIOrientation.LandscapeLeft) {
            isApply = false;
        }

        if(EditorSettings.serializationMode != SerializationMode.ForceText) {
            isApply = false;
        }

        int staticBatchingValue = 0;
        int dynamicBatchingValue = 0;
        Type playerSettingsType = typeof(PlayerSettings);
        MethodInfo method = playerSettingsType.GetMethod("GetBatchingForPlatform", BindingFlags.NonPublic | BindingFlags.Static);
        object[] param = new object[] { BuildTarget.Android, staticBatchingValue, dynamicBatchingValue };
        method.Invoke(null, param);
        if((int)param[2] != 1) {
            isApply = false;
        }

        return isApply;
    }
    static void PlayerSet() {

        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;

        PlayerSettings.SetApiCompatibilityLevel(BuildTargetGroup.Android, ApiCompatibilityLevel.NET_4_6);

        PlayerSettings.SetMobileMTRendering(BuildTargetGroup.Android, false);

        PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android,false);
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android,new UnityEngine.Rendering.GraphicsDeviceType[1] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });

        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        EditorSettings.serializationMode = SerializationMode.ForceText;
        
        Type playerSettingsType = typeof(PlayerSettings);
        MethodInfo method = playerSettingsType.GetMethod("SetBatchingForPlatform", BindingFlags.NonPublic | BindingFlags.Static);
        int staticBatchingValue = 1;
        int dynamicBatchingValue = 1;
        object[] param = new object[] { BuildTarget.Android, staticBatchingValue, dynamicBatchingValue };
        method.Invoke(null, param);


    }

    [InitializeOnLoadMethod]
    static void PlayerSetMust() {
        if(PlayerSettings.productName == "New Unity Project") {
            PlayerSettings.productName = "SDK";
        }
        if(PlayerSettings.companyName == "DefaultCompany") {
            PlayerSettings.companyName = "XR";
        }

    }

    #endregion

    #region ProjectSettings->Tags and Layers

    //[InitializeOnLoadMethod]
    //static void TagsAndLayerSet() {
    //    bool isExist = false;
    //    foreach(var item in UnityEditorInternal.InternalEditorUtility.layers) {
    //        if(item == "focus") {
    //            isExist = true;
    //            break;
    //        }
    //    }
    //    if(isExist == false) {
    //        AutoAddLayer("focus");
    //    }

    //    isExist = false;
    //    foreach(var item in UnityEditorInternal.InternalEditorUtility.layers) {
    //        if(item == "no light") {
    //            isExist = true;
    //            break;
    //        }
    //    }
    //    if(isExist == false) {
    //        AutoAddLayer("no light"); 
    //    }


    //    isExist = false;
    //    foreach(var item in UnityEditorInternal.InternalEditorUtility.tags) {
    //        if(item == "SvrCamera") {
    //            isExist = true;
    //            break;
    //        }
    //    }
    //    if(isExist == false) {
    //        UnityEditorInternal.InternalEditorUtility.AddTag("SvrCamera");
    //    }
    //}
    static void AutoAddLayer(string layer) {
        SerializedObject tagMagager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Tagmanager.asset"));
        SerializedProperty it = tagMagager.GetIterator();
        while(it.NextVisible(true)) {
            if(it.name.Equals("layers")) {
                for(int i = 0; i < it.arraySize; i++) {
                    if(i <= 7) {
                        continue;
                    }
                    SerializedProperty sp = it.GetArrayElementAtIndex(i);
                    if(string.IsNullOrEmpty(sp.stringValue)) {
                        sp.stringValue = layer;
                        tagMagager.ApplyModifiedProperties();
                        return;
                    }
                }
            }
        }
    }

    //static void AutoAddLayer(string layer) {
    //    if(!HasThisLayer(layer)) {
    //        SerializedObject tagMagager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Tagmanager.asset"));
    //        SerializedProperty it = tagMagager.GetIterator();
    //        while(it.NextVisible(true)) {
    //            if(it.name.Equals("layers")) {
    //                for(int i = 0; i < it.arraySize; i++) {
    //                    if(i <= 7) {
    //                        continue;
    //                    }
    //                    SerializedProperty sp = it.GetArrayElementAtIndex(i);
    //                    if(string.IsNullOrEmpty(sp.stringValue)) {
    //                        sp.stringValue = layer;
    //                        tagMagager.ApplyModifiedProperties();
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}
    //static bool HasThisLayer(string layer) {
    //    //先清除已保存的
    //    SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/Tagmanager.asset"));
    //    SerializedProperty it = tagManager.GetIterator();
    //    while(it.NextVisible(true)) {
    //        if(it.name.Equals("layers")) {
    //            for(int i = 0; i < it.arraySize; i++) {
    //                if(i <= 7) {
    //                    continue;
    //                }
    //                SerializedProperty sp = it.GetArrayElementAtIndex(i);
    //                if(!string.IsNullOrEmpty(sp.stringValue)) {
    //                    if(sp.stringValue.Equals(layer)) {
    //                        sp.stringValue = string.Empty;
    //                        tagManager.ApplyModifiedProperties();
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    for(int i = 0; i < UnityEditorInternal.InternalEditorUtility.layers.Length; i++) {
    //        if(UnityEditorInternal.InternalEditorUtility.layers[i].Contains(layer)) {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    #endregion

    #region ProjectSettings->Quality

    static bool QualityWindow() {
        bool isApply = true;
        GUILayout.Label("QulitySettings", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("QulityLevel:Middle");

        if(QualitySettings.GetQualityLevel() != 2) {

            GUIStyle styleSlide = new GUIStyle();
            styleSlide.normal.textColor = Color.red;
            GUILayout.Label("Failed", styleSlide);
            isApply = false;
        } else {

            GUIStyle styleApplied = new GUIStyle();
            styleApplied.normal.textColor = Color.green;
            GUILayout.Label("Applied", styleApplied);
        }

        EditorGUILayout.EndHorizontal();

        return isApply;
    }
    static bool QualityCheck() {
        bool isApply = true;
        if(QualitySettings.GetQualityLevel() != 2) {
            isApply = false;
        }
        return isApply;
    }
    static void QualitySet() {
        QualitySettings.SetQualityLevel(2);
    }

    #endregion
}
