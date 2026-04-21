using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ReplaceNpcMaterialsToURP : EditorWindow
{
    private GameObject rootObject;

    private const string sourceFolder = "Assets/npc_casual_set_00/Materials";
    private const string urpFolder = "Assets/npc_casual_set_00/MaterialsUPR";

    [MenuItem("Tools/NPC/Replace Materials To URP")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceNpcMaterialsToURP>("Replace NPC Materials");
    }

    private void OnGUI()
    {
        GUILayout.Label("Remplacement Materials -> MaterialsURP", EditorStyles.boldLabel);
        rootObject = (GameObject)EditorGUILayout.ObjectField("Root Object", rootObject, typeof(GameObject), true);

        EditorGUILayout.Space();

        if (GUILayout.Button("Corriger les matériaux"))
        {
            if (rootObject == null)
            {
                Debug.LogError("Aucun objet racine sélectionné.");
                return;
            }

            ReplaceMaterials(rootObject);
        }
    }

    private static void ReplaceMaterials(GameObject root)
    {
        Dictionary<string, Material> urpMaterialsByName = LoadUrpMaterials();
        if (urpMaterialsByName.Count == 0)
        {
            Debug.LogError("Aucun matériau trouvé dans : " + urpFolder);
            return;
        }

        int rendererCount = 0;
        int materialReplacedCount = 0;

        SkinnedMeshRenderer[] renderers = root.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        foreach (SkinnedMeshRenderer smr in renderers)
        {
            rendererCount++;

            Material[] mats = smr.sharedMaterials;
            bool changed = false;

            for (int i = 0; i < mats.Length; i++)
            {
                Material currentMat = mats[i];
                if (currentMat == null)
                    continue;

                string currentPath = AssetDatabase.GetAssetPath(currentMat);

                if (string.IsNullOrEmpty(currentPath))
                    continue;

                if (!currentPath.StartsWith(sourceFolder))
                    continue;

                if (urpMaterialsByName.TryGetValue(currentMat.name, out Material urpMat))
                {
                    mats[i] = urpMat;
                    changed = true;
                    materialReplacedCount++;
                    Debug.Log($"[OK] {smr.name} : {currentMat.name} -> {urpMat.name}");
                }
                else
                {
                    Debug.LogWarning($"[MANQUANT] Aucun équivalent URP pour : {currentMat.name}");
                }
            }

            if (changed)
            {
                Undo.RecordObject(smr, "Replace NPC Materials To URP");
                smr.sharedMaterials = mats;
                EditorUtility.SetDirty(smr);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"Terminé. Renderers scannés : {rendererCount}, matériaux remplacés : {materialReplacedCount}");
    }

    private static Dictionary<string, Material> LoadUrpMaterials()
    {
        Dictionary<string, Material> dict = new Dictionary<string, Material>();

        string[] guids = AssetDatabase.FindAssets("t:Material", new[] { urpFolder });

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (mat != null && !dict.ContainsKey(mat.name))
                dict.Add(mat.name, mat);
        }

        return dict;
    }
}