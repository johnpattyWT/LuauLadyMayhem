using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.IO;
using System.Linq;

public class ExtendedMissingScriptCleaner
{
    [MenuItem("Tools/Cleanup/Remove Missing Scripts In Project (Safe)")]
    public static void CleanProject()
    {
        int totalRemoved = 0;
        string[] assetPaths = AssetDatabase.GetAllAssetPaths();

        foreach (string path in assetPaths)
        {
            if (!path.StartsWith("Assets/"))
                continue;

            // Skip .unity scenes to avoid thread issues
            if (path.EndsWith(".prefab") || path.EndsWith(".asset"))
            {
                GameObject[] objects = AssetDatabase.LoadAllAssetsAtPath(path)
                    .OfType<GameObject>()
                    .ToArray();

                foreach (GameObject go in objects)
                {
                    int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(go);
                    if (removed > 0)
                    {
                        totalRemoved += removed;
                        Debug.Log($"Removed {removed} missing script(s) from prefab/asset: {path}", go);
                        EditorUtility.SetDirty(go);
                    }
                }
            }
        }

        AssetDatabase.SaveAssets();

        // Now clean all open scenes
        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            var scene = EditorSceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                foreach (GameObject go in scene.GetRootGameObjects())
                {
                    int removed = RemoveFromHierarchy(go);
                    totalRemoved += removed;
                }

                EditorSceneManager.MarkSceneDirty(scene);
            }
        }

        Debug.Log($"✅ Finished cleanup. Total missing scripts removed: {totalRemoved}");
    }

    private static int RemoveFromHierarchy(GameObject root)
    {
        int removed = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(root);
        foreach (Transform child in root.transform)
        {
            removed += RemoveFromHierarchy(child.gameObject);
        }
        return removed;
    }
}
