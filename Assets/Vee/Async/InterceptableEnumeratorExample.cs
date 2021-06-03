
#if UNITY_EDITOR

using UnityEngine;
using System.Collections;

using UnityEngine.Networking;

using Vee.Consts;
namespace Vee.Async
{
    public class InterceptableEnumeratorExample : MonoBehaviour
    {
        IEnumerator Start()
        {
            yield return this.TestInterceptException();
        }

        protected IEnumerator TestInterceptException()
        {
//            var routine = new InterceptableEnumerator(DoTask());
            var routine = new InterceptableEnumerator(DoTaskHasException());

            routine.RegisterCatchBlock((e) =>
            {
                Debug.LogError($"Exeption is Catched in Coroutine ! {e.ToString()}");
            });

            routine.RegisterFinallyBlock(() =>
            {
                Debug.Log($"Finally block for Coroutine !");
            });
            
            
            StartCoroutine(routine);

            yield break;
        }

        IEnumerator DoTask()
        {
            var www = UnityWebRequest.Get(Urls.ServerTime);
            yield return www.SendWebRequest();

            if (! string.IsNullOrEmpty(www.error))
            {
                Debug.LogWarning(www.error);
            } 
            else
            {
                string rawText = www.downloadHandler.text;
                Debug.Log("Server Time : " + rawText);
            }
        }

        IEnumerator DoTaskHasException()
        {
            yield return new WaitForSeconds(1f);
            
            throw new System.Exception("This is a test, not a bug.");
            
//            Debug.Log($"this message cannot be logged");
        }
    }
}
#endif