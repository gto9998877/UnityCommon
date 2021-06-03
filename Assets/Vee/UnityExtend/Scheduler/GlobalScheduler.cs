using System.Collections;
using System.Collections.Generic;

namespace Vee
{
    public class GlobalScheduler : MonoSingleton<GlobalScheduler>
    {
        protected override void OnDestroy()
        {
            Scheduler.BreakAllCoroutines();
            base.OnDestroy();
        }

        public IEnumerator StartNew(IEnumerator func, CoroutineHandle handle)
        {
            yield return func;

            Scheduler.StopCoroutine(handle);
        }
    }
}