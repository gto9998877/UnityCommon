using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Sirenix.OdinInspector;

public class ShowEventSystem : MonoBehaviour {

#if UNITY_EDITOR
    [ReadOnly]
    public bool PointOverClickable = false;
    [ReadOnly]
    public string SelectObject;
    [ReadOnly]
    public bool MultiTouchEnabled;
    
    [ReadOnly]
    public GameObject FirstRaycast;
    [ReadOnly]
    public List<GameObject> RaycastQueue = new List<GameObject>();


    private static List<RaycastResult> tempRaycastResults = new List<RaycastResult>();
    private static PointerEventData tempPointerEventData;
    private static EventSystem tempEventSystem;

    EventSystem CurES {
        get {
            if (tempEventSystem == null) {
                tempEventSystem = EventSystem.current;
            }

            return tempEventSystem;
        }
    }

    PointerEventData PointData {
        get {
            if (tempPointerEventData == null) {
                tempPointerEventData = new PointerEventData(CurES);
            }

            return tempPointerEventData;
        }
    }
    
    // Use this for initialization
    void Start () {
        PointOverClickable = false;
        SelectObject = "";
        FirstRaycast = null;
    }

    // Update is called once per frame
    void Update () {
        MultiTouchEnabled = Input.multiTouchEnabled;
        if (CurES == null) return;
        
        PointOverClickable = CurES.IsPointerOverGameObject ();
        var selectGo = CurES.currentSelectedGameObject;
        if (selectGo != null) {
            string objName = selectGo.name;
            Transform node = selectGo.transform;
            int whileCnt = 0;
            while (node.parent != null) {
                node = node.parent;
                objName = node.gameObject.name + "/" + objName;
                whileCnt++;
                if (whileCnt > 10) break;
            }
            SelectObject = objName;
        } else {
            SelectObject = "";
        }

        UpdateEventSystemRaycast();

        // if (Input.GetMouseButton (0) && FieldCamera.instance != null) {

        //     var castRes = FieldCamera.instance.RaycastAll (Input.mousePosition);
        //     if (castRes.Length > 0) {
        //         FirstRaycast = castRes[0].collider;
        //     } else {
        //         FirstRaycast = null;
        //     }
        // }
    }
    
    
    void UpdateEventSystemRaycast() {
        if (Input.GetMouseButton(0)) {
            tempRaycastResults.Clear();
            FirstRaycast = null;
            RaycastQueue.Clear();
            
            if (CurES != null)
            {                
                PointData.Reset();

                // Raycast event system at the specified point
                PointData.position = Input.mousePosition;

                CurES.RaycastAll(PointData, tempRaycastResults);
                var resultCount = tempRaycastResults.Count;
                // Loop through all results and remove any that don't match the layer mask
//                if (tempRaycastResults.Count > 0)
//                {
//                    for (var i = tempRaycastResults.Count - 1; i >= 0; i--)
//                    {
//                        var raycastResult = tempRaycastResults[i];
//                        var raycastLayer  = 1 << raycastResult.gameObject.layer;
//
//                        if ((raycastLayer & layerMask) == 0)
//                        {
//                            tempRaycastResults.RemoveAt(i);
//                        }
//                    }
//                }

                for (var i = 0; i < resultCount; ++i) {
                    var raycastResult = tempRaycastResults[i];
                    RaycastQueue.Add(raycastResult.gameObject);
                }

                if (resultCount > 0) {
                    FirstRaycast = tempRaycastResults[0].gameObject;
                }
            }
        }
    }
#endif
}