using UnityEngine;
using UnityEditor;

public class MakeMinionsInformations {
    [MenuItem("Assets/Create/ScriptableObject/MinionsInformations")]
	public static void CreateAsset()
    {
        MinionsInformations asset = ScriptableObject.CreateInstance<MinionsInformations>();

        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/NewSpawnersInformations.asset");

        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
