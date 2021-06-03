using System;
using Vee.Async;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Vee.Events
{
    #region Base
    [Serializable] public class BoolEvent : UnityEvent<bool> { }
    [Serializable] public class IntEvent : UnityEvent<int> { }
    [Serializable] public class FloatEvent : UnityEvent<float> { }
    [Serializable] public class StringEvent : UnityEvent<string> {}
    [Serializable] public class IntArrayEvent : UnityEvent<int[]> { }

    [Serializable] public class Int2Event : UnityEvent<int, int> { }
    [Serializable] public class Int3Event : UnityEvent<int, int, int> { }
    
    [Serializable] public class I2BEvent : UnityEvent<int, int, bool> { }
    
    [Serializable] public class Vector2Event : UnityEvent<Vector2> { }
    [Serializable] public class Vector3Event : UnityEvent<Vector3> { }
    
    [Serializable] public class Vector3x2Event : UnityEvent<Vector3, Vector3> { }
    
    [Serializable] public class Vector3IntEvent : UnityEvent<Vector3Int> { }
    [Serializable] public class Vector3Intx2Event : UnityEvent<Vector3Int, Vector3Int> { }
    [Serializable] public class StringParamsEvent : UnityEvent<string, EventParams> {}
    [Serializable] public class ExceptionEvent : UnityEvent<Exception> { }
    [Serializable] public class CustomAsyncOperationEvent : UnityEvent<CustomAsyncOperation> { }
    [Serializable] public class VeeResultEvent : UnityEvent<VeeResult> {}
    
    [Serializable] public class PointerEvent : UnityEvent<PointerEventData> { }


    #endregion
}