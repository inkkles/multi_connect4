using UnityEditor;
using UnityEngine;
using System.IO;

namespace multi_connect4
{
    public class AutoScriptMover : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string assetPath in importedAssets)
            {
                if (assetPath.EndsWith(".cs") && IsInAssetsRoot(assetPath))
                {
                    string fileName = Path.GetFileName(assetPath);

                    bool moveScript = EditorUtility.DisplayDialog(
                        "Move Script",
                        $"Would you like to move {fileName} to the Scripts folder?",
                        "Yes", "No");

                    if (moveScript)
                    {
                        string targetFolder = "Assets/Scripts"; // Specify the folder where you want to move the script

                        if (!AssetDatabase.IsValidFolder(targetFolder))
                        {
                            AssetDatabase.CreateFolder("Assets", "Scripts");
                        }
                        string newPath = Path.Combine(targetFolder, fileName);
                        AssetDatabase.MoveAsset(assetPath, newPath);
                        Debug.Log($"Moved {fileName} to {targetFolder}");
                    }
                    else
                    {
                        Debug.Log($"{fileName} was not moved.");
                    }
                }
            }
        }

        private static bool IsInAssetsRoot(string assetPath)
        {
            string directory = Path.GetDirectoryName(assetPath).Replace("\\", "/");
            return directory == "Assets";
        }
    }
}