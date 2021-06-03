using UnityEngine;

namespace Vee {
    public static class JsonUtils {
        public static void SaveJsonToFile (string savePath, object obj) {
            string jsonContent = JsonUtility.ToJson(obj);
            FileUtils.CreatFile(savePath, jsonContent);
            Debug.Log("Saved To " + savePath);
        }
        
        /// <summary> 把对象转换为Json字符串 </summary>
        /// <param name="obj">对象</param>
        public static string toJson<T> (T obj) {
            if (obj == null) return "null";

            if (typeof (T).GetInterface ("IList") != null) {
                Pack<T> pack = new Pack<T> ();
                pack.data = obj;
                string json = JsonUtility.ToJson (pack);
                if (json.Length < 8) {
                    return json;
                } else {
                    return json.Substring (8, json.Length - 9);
                }
            }

            return JsonUtility.ToJson (obj);
        }

        /// <summary> 解析Json </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="json">Json字符串</param>
        public static T fromJson<T> (string json) {
            if (json == "null" && typeof (T).IsClass) return default (T);

            if (typeof (T).GetInterface ("IList") != null) {
                json = "{\"data\":{data}}".Replace ("{data}", json);
                Pack<T> Pack = JsonUtility.FromJson<Pack<T>> (json);
                return Pack.data;
            }

            return JsonUtility.FromJson<T> (json);
        }


        /// <summary> 内部包装类 </summary>
        private class Pack<T> {
            public T data;
        }



        // public static string toJson<T, U> (Dictionary<T, U> dic) {
        //     List<DictionaryValuePair<T, U>> dicToList = new List<DictionaryValuePair<T, U>>();
        //     Utils.ForEachInDictionary(dic, (k, v)=>{
        //         var newPair = new DictionaryValuePair<T, U> ();
        //         newPair.key = k;
        //         newPair.data = v;
        //         dicToList.Add(newPair);
        //     });

        //     return toJson(dicToList);
        // }
        // [Serializable]
        // public class DictionaryValuePair<T, U> {
        //     public T key;
        //     public U data;
        // }

    }
}