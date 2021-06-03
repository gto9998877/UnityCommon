using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Vee
{
    public delegate IEnumerator CommonCoroutineNoParam();

    public delegate void Callback();
    public delegate void Callback<T>(T arg1);
    public delegate void Callback<T, U>(T arg1, U arg2);
    public delegate void Callback<T, U, V>(T arg1, U arg2, V arg3);

    public delegate void Action<T>(T arg1);
    public delegate void Action<T, U>(T arg1, U arg2);

    public delegate void UIDelegate(GameObject trigger);
    public delegate void UIPinchDelegate(GameObject trigger, float pinchFactor);
    public delegate void UIBaseEventDelegate(GameObject trigger, BaseEventData eventData);
    public delegate void UIAxisEventDelegate(GameObject trigger, AxisEventData eventData);
    public delegate void UIPointerEventDelegate(GameObject trigger, PointerEventData eventData);

    public delegate void UIDragDeltaDelegate(GameObject trigger, Vector2 dragDelta);
    public delegate void UIDragPosDelegate(GameObject dragGo, Vector2 screenPos);
    public delegate void UIDragOverDelegate(GameObject dragGo, GameObject dragOver);
    public delegate void UIDropOnDelegate(GameObject dragGo, GameObject dropOn);

    public delegate bool VeePredicate<in T>(T obj);
    public delegate float VeeGetFloat();

}


