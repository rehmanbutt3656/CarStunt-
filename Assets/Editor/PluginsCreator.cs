using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class PluginsCreator : EditorWindow
{
    [MenuItem("AdmobMediation/Create Ads Manager")]
    public static void CreateAdsManager()
    {
        GameObject go = new GameObject("Ads Manager");
        go.AddComponent<AdmobAdsManager>();
        Selection.activeObject = go;
    }
}
