using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeSpawnersInformations
{
    [MenuItem("Assets/Create/ScriptableObject/SpawnersInformations")]
    public static void CreateAsset()
    {
        SpawnersInformations asset = ScriptableObject.CreateInstance<SpawnersInformations>();

        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/NewSpawnerInformations.asset");

        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
