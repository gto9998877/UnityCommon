using System.IO;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


namespace Vee.Editor
{
    public class AssetBundleTool
    {
        /// <summary>
        /// use mouse select folder in unity editor, then click"Veewo/AssetBundle/Rename sources a new name"button to rename sources
        /// note: only can select one folder at one time
        /// </summary>
        [MenuItem("Veewo/AssetBundle/RenameSourcesInFolder", false, 11)]
        private static void RenameSources()
        {
            string tempSplit = "__";//use this sign to split directory

            string tempSelectedFolder = "";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                tempSelectedFolder = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(tempSelectedFolder) && File.Exists(tempSelectedFolder))
                {
                    tempSelectedFolder = Path.GetDirectoryName(tempSelectedFolder);
                    break;
                }
            }
            if (string.IsNullOrEmpty(tempSelectedFolder))
                return;
            tempSelectedFolder = tempSelectedFolder.Replace("Assets", Application.dataPath);
            DirectoryInfo tempDInfo = new DirectoryInfo(tempSelectedFolder);
            FileInfo[] tempFInfo = tempDInfo.GetFiles("*.*", SearchOption.AllDirectories);
            float tempProcessed = 0;
            float tempTotal = tempFInfo.Length;
            foreach (var param in tempFInfo)
            {
                tempProcessed++;
                EditorUtility.DisplayProgressBar("Progress", param.Name, tempProcessed / tempTotal);
                string tempPath = param.ToString();
                if (tempPath.EndsWith(".meta"))
                    continue;
                if (tempPath.Contains(tempSplit))//already name it, donnt need to rename it again
                    continue;
                tempPath = tempPath.Replace(@"\", "/");
                tempPath = tempPath.Substring(tempPath.IndexOf("Assets"));
                int tempIndex = tempPath.IndexOf("/") + 1;
                string tempName = tempPath.Substring(tempIndex);
                tempIndex = tempName.LastIndexOf(".");
                tempName = tempName.Substring(0, tempIndex);
                tempName = tempName.Replace("/", tempSplit);
                string tempError = AssetDatabase.RenameAsset(tempPath, tempName);
                if (!string.IsNullOrEmpty(tempError))
                    Debug.Log(tempError);
            }
            EditorUtility.ClearProgressBar();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// 为文件夹下面所有文件设置统一的包名
        /// use mouse select folder
        /// </summary>
        [MenuItem("Veewo/AssetBundle/SetABnameForAllInFolder", false, 12)]
        private static void SetFilesAnAssetBundleName()
        {
            string tempSelectedFolder = "";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                tempSelectedFolder = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(tempSelectedFolder) && File.Exists(tempSelectedFolder))
                {
                    tempSelectedFolder = Path.GetDirectoryName(tempSelectedFolder);
                    break;
                }
            }
            if (string.IsNullOrEmpty(tempSelectedFolder))
                return;

            //string tempPrefabPath = Application.dataPath + "/Example";
            SetAssetBundleNameInDir(tempSelectedFolder);
        }

        /// <summary>
        /// 为每个文件单独设置包名
        /// </summary>
        [MenuItem("Veewo/AssetBundle/SetABnameForFile", false, 13)]
        private static void SetFileAnAssetBundleName()
        {
            string tempSelectedFolder = "";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                tempSelectedFolder = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(tempSelectedFolder) && File.Exists(tempSelectedFolder))
                {
                    string bundleName = Path.GetFileNameWithoutExtension(tempSelectedFolder);
                    SetAssetBundleNameForFile(tempSelectedFolder, bundleName);
                }
            }
        }

        public static void SetAssetBundleNameInDir (string dir) {
            DirectoryInfo tempDInfo = new DirectoryInfo(dir);
            if (!tempDInfo.Exists) {
                Debug.LogWarning(dir + " is not exist");
                return;
            }
            
            FileInfo[] tempFInfo = tempDInfo.GetFiles("*.*", SearchOption.AllDirectories);
            //IList<string> tempList = new List<string>();
            //for (int i = 0; i < tempFInfo.Length; i++)
            //{
            //    string tempFileName = tempFInfo[i].Name.ToLower();
            //    if (tempFileName.EndsWith(".prefab") || tempFileName.EndsWith(".mat"))
            //        tempList.Add(tempFileName);
            //}
            foreach (var VARIABLE in tempFInfo)
            {
                string tempPath = VARIABLE.ToString();
                if (tempPath.EndsWith(".meta")) continue;
                SetAssetBundleNameForFile(tempPath);
            }

            Debug.Log("Set Asset Bundle Name Finished ! " + "(" + dir + ")");
            AssetDatabase.Refresh();
        }

        static string GetAssetPath (string dir) {
            var tempPath = dir.Replace(@"\", " / ");
            tempPath = tempPath.Replace(" ", "");
            tempPath = tempPath.Substring(tempPath.IndexOf("Assets"));
            return tempPath;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileDir"></param>
        /// <param name="bundleName">不要带扩展名</param>
        public static void SetAssetBundleNameForFile (string fileDir, string bundleName = "") {
            var tempPath = GetAssetPath(fileDir);
            AssetImporter tempAImporter = AssetImporter.GetAtPath(tempPath);

            string finalBundleName = bundleName;
            if (string.IsNullOrEmpty(finalBundleName)) {
                int tempIndex = tempPath.IndexOf("/") + 1;
                tempPath = tempPath.Substring(tempIndex);
                tempIndex = tempPath.LastIndexOf("/");
                tempPath = tempPath.Substring(0, tempIndex);
                tempPath = tempPath.Replace("/", "");
                tempPath = tempPath.ToLower();

                finalBundleName = tempPath;
            }

            if (tempAImporter != null)
                tempAImporter.assetBundleName = finalBundleName;
            else
                Debug.LogError(fileDir + " this asset cann't set name: " + finalBundleName);
        }

        static void ClearAssetBundleNameForFile (string fileDir) {
            var tempPath = GetAssetPath(fileDir);
            AssetImporter tempAImporter = AssetImporter.GetAtPath(tempPath);
            if (tempAImporter != null)
                tempAImporter.assetBundleName = string.Empty;
        }

        /// <summary>
        /// 清除选定的文件夹中所有文件的包名
        /// </summary>
        [MenuItem("Veewo/AssetBundle/CleanABnameInFolder", false, 12)]
        private static void CleanAssetBundleName()
        {
            string tempSelectedFolder = GetSelectFolder();
            if (string.IsNullOrEmpty(tempSelectedFolder))
                return;

            //string tempPrefabPath = Application.dataPath + "/Example";
            ClearAssetBundleNameInDir(tempSelectedFolder);

            Debug.Log("Clean Asset Bundle Name Finished !");
            AssetDatabase.Refresh();
        }

        public static void ClearAssetBundleNameInDir (string dir) {
            DirectoryInfo tempDInfo = new DirectoryInfo(dir);
            if (!tempDInfo.Exists) {
                Debug.LogWarning(dir + " is not exist");
                return;
            }
            
            FileInfo[] tempFInfo = tempDInfo.GetFiles("*.*", SearchOption.AllDirectories);
            foreach (var VARIABLE in tempFInfo)
            {
                string tempPath = VARIABLE.ToString();
                if (tempPath.EndsWith(".meta")) continue;
                ClearAssetBundleNameForFile(tempPath);
            }
        }

        static string GetSelectFolder()
        {
            string tempSelectedFolder = "";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                tempSelectedFolder = AssetDatabase.GetAssetPath(obj);
                if (!string.IsNullOrEmpty(tempSelectedFolder) && File.Exists(tempSelectedFolder))
                {
                    tempSelectedFolder = Path.GetDirectoryName(tempSelectedFolder);
                    break;
                }
            }

            return tempSelectedFolder;
        }
    }
}