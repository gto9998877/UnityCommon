
using UnityEngine;

namespace Vee.Globals
{
    public class GlobalVariables : MonoSingleton<GlobalVariables>
    {

        public static bool IsInitializeFinished = false;
        public static bool IsQuitting = false;

        private void OnEnable()
        {
            Application.wantsToQuit += () => IsQuitting = true;
        }
    }
}