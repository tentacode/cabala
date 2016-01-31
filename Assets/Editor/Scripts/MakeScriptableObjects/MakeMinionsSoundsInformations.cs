using UnityEngine;
using UnityEditor;

public class MakeMinionsSoundsInformations{
    [MenuItem("Assets/Create/ScriptableObject/MinionsSoundsInformations")]
    public static void CreateAsset()
    {
        MinionsSoundsInformations asset = ScriptableObject.CreateInstance<MinionsSoundsInformations>();

        AssetDatabase.CreateAsset(asset, "Assets/ScriptableObjects/NewSoundsInformations.asset");

        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
