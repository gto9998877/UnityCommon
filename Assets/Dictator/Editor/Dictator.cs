using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(Dict), true)]
public class Dictator : PropertyDrawer {
    private List<int> _errorIDs = new List<int>();
	SerializedProperty _errors;
    int _selected;

    private SerializedProperty prop;
    void CheckForErrorsUpdate(SerializedProperty p){
		_errors = p.FindPropertyRelative("_errorList");
		_errorIDs.Clear();
		if(_errors != null)
		{
			for (int i = 0; i < _errors.arraySize; i++)
			{
				_errorIDs.Add(_errors.GetArrayElementAtIndex(i).intValue);
			}
		}

	}
	/// <summary>
    /// Initialize ReorderableList and assigning callbacks
    /// </summary>
    /// <param name="prop"></param>
    /// <returns></returns>
    ReorderableList InitDisplay(SerializedProperty p)
	{

        CheckForErrorsUpdate(p);

        ReorderableList inst = new ReorderableList(p.serializedObject, p.FindPropertyRelative("_key"), true, true, true ,true);
		
		inst.onChangedCallback += OnChangedCallback;


	    inst.onRemoveCallback += OnRemoveCallback;

		
		inst.onAddCallback += OnAddCallback;
		
		inst.elementHeightCallback = ElementHeightCallback; 


	    inst.onReorderCallback += OnReorderCallback;

		inst.drawElementCallback = DrawElementCallback;

		inst.drawHeaderCallback = DrawHeaderCallback;
		inst.onSelectCallback += OnSelectCallback;
        inst.onCanRemoveCallback += OnCanRemoveCallback;

//	    ReorderableList.defaultBehaviours;
		return inst;
		
	}


    private void OnAddCallback(ReorderableList list)
    {
        var localKeys = prop.FindPropertyRelative("_key");
        var localvals = prop.FindPropertyRelative("_val");
        int idx = localKeys.arraySize;
        localKeys.InsertArrayElementAtIndex(idx);
        localvals.InsertArrayElementAtIndex(idx);
        var val = localvals.GetArrayElementAtIndex(idx);
        var key = localKeys.GetArrayElementAtIndex(idx);
        CopyValue(prop.FindPropertyRelative("_valType"), ref val);
        CopyValue(prop.FindPropertyRelative("_keyType"), ref key);

        Activate(list.index);
    }

    private float ElementHeightCallback(int index)
    {
        float ret;
        if (prop != null)
        {
            ret = (index >= prop.FindPropertyRelative("_val").arraySize)
                ? EditorGUIUtility.singleLineHeight
                : EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("_val").GetArrayElementAtIndex(index));
        }
        else
        {
            ret = EditorGUIUtility.singleLineHeight;
        }
        return ret;
    }

    void Activate(int index)
    {
        var k = prop.FindPropertyRelative("_key");
        var v = prop.FindPropertyRelative("_val");
        for (int i = 0; i < k.arraySize; i++)
        {
            if (i == index)
            {
                k.GetArrayElementAtIndex(i).isExpanded = true;
                v.GetArrayElementAtIndex(i).isExpanded = true;
            }
            else
            {
                k.GetArrayElementAtIndex(i).isExpanded = false;
                v.GetArrayElementAtIndex(i).isExpanded = false;
            }
        }
    }

    void DeactivateAllChild(int index)
    {
        var k = prop.FindPropertyRelative("_key");
        var v = prop.FindPropertyRelative("_val");
        _list.draggable = false;
        for (int i = 0; i < k.arraySize; i++)
        {
            k.GetArrayElementAtIndex(i).isExpanded = false;
            v.GetArrayElementAtIndex(i).isExpanded = false;
        }
    }
    private void DrawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
    {
        if (isActive)
        {
            Activate(index);
        }


        float KVWidth = prop.FindPropertyRelative("_divider").floatValue+0.5f;
        Rect left = new Rect(rect.x, rect.y, rect.width * KVWidth, rect.height);
        Rect right = new Rect(rect.x + rect.width * KVWidth, rect.y, rect.width * (1f - KVWidth), rect.height);

        

        GUIContent emptylabel = new GUIContent();
        if (_errorIDs.Contains(index))
        {
            Color temp = GUI.color;
            GUI.color = Color.red;
            if (index < prop.FindPropertyRelative("_key").arraySize)
                EditorGUI.PropertyField(left, prop.FindPropertyRelative("_key").GetArrayElementAtIndex(index),
                    emptylabel, true);
            if (index < prop.FindPropertyRelative("_val").arraySize)
                EditorGUI.PropertyField(right, prop.FindPropertyRelative("_val").GetArrayElementAtIndex(index),
                    emptylabel, true);
            GUI.color = temp;
        }
        else
        {
            if (index < prop.FindPropertyRelative("_key").arraySize)
                EditorGUI.PropertyField(left, prop.FindPropertyRelative("_key").GetArrayElementAtIndex(index),
                    emptylabel, true);
            if (index < prop.FindPropertyRelative("_val").arraySize)
                EditorGUI.PropertyField(right, prop.FindPropertyRelative("_val").GetArrayElementAtIndex(index),
                    emptylabel, true);
        }
    }

    private bool reordering;
    private void OnReorderCallback(ReorderableList list)
    {
        reordering = true;
        prop.FindPropertyRelative("_val").MoveArrayElement(_selected, list.index);
        Activate(list.index);
    }

    private void OnChangedCallback(ReorderableList list)
    {
        reordering = false;
        _list = InitDisplay(prop);
        CheckForErrorsUpdate(list.serializedProperty);
        if (_errors != null)
        {
            for (int i = 0; i < _errors.arraySize; i++)
            {
                _errorIDs.Add(_errors.GetArrayElementAtIndex(i).intValue);
            }
        }
    }

    private void DrawHeaderCallback(Rect rect)
    {
        rect.width = rect.width + 50f;
        rect.x += 10f;
        EditorGUI.Slider(rect, prop.FindPropertyRelative("_divider"), -0.5f, 0.5f, "");
    }

    private void OnSelectCallback(ReorderableList list)
    {
        if (list == _list)
        {
            reordering = true;
            _selected = list.index;
            prop.FindPropertyRelative("_selected").intValue = list.index;
        }
    }


    private bool OnCanRemoveCallback(ReorderableList list)
    {
        return true;
    }

    private void OnRemoveCallback(ReorderableList list)
    {
        if (list.serializedProperty.arraySize == 1)
        {
            prop.FindPropertyRelative("_val").ClearArray();
            prop.FindPropertyRelative("_key").ClearArray();
            _errors.ClearArray();
        }
        else
        {
            prop.FindPropertyRelative("_val").DeleteArrayElementAtIndex(list.index);
            prop.FindPropertyRelative("_key").DeleteArrayElementAtIndex(list.index);
            CheckForErrorsUpdate(prop);
        }
    }

    protected void CopyValue(SerializedProperty type, ref SerializedProperty val)
    {


        switch (type.propertyType)
        {
            case SerializedPropertyType.AnimationCurve:
                val.animationCurveValue = type.animationCurveValue;
                break;
            case SerializedPropertyType.Boolean:
                val.boolValue = false;
                break;
            case SerializedPropertyType.Bounds:
                val.boundsValue = type.boundsValue;
                break;
            case SerializedPropertyType.Character:
            case SerializedPropertyType.Generic:
            case SerializedPropertyType.Gradient:
            case SerializedPropertyType.LayerMask:
                //no good way to initialize a good base value
                var reset = val.FindPropertyRelative("reset");
                if (reset != null)
                {
                    reset.boolValue = true;
                }
                break;
            case SerializedPropertyType.ObjectReference:
                val.objectReferenceValue = type.objectReferenceValue;
                break;
            case SerializedPropertyType.Color:
                val.colorValue = Color.black;
                break;
            case SerializedPropertyType.Enum:
                val.enumValueIndex = type.enumValueIndex;
                break;
            case SerializedPropertyType.Float:
                val.floatValue = 0;
                break;
            case SerializedPropertyType.String:
                val.stringValue = "";
                break;
            case SerializedPropertyType.Integer:
                val.intValue = 0;
                break;
            case SerializedPropertyType.Quaternion:
                val.quaternionValue = type.quaternionValue;
                break;
            case SerializedPropertyType.Rect:
                val.rectValue = type.rectValue;
                break;
            case SerializedPropertyType.Vector2:
                val.vector2Value = Vector2.zero;
                break;
            case SerializedPropertyType.Vector3:
                val.vector3Value = Vector3.zero;
                break;
            case SerializedPropertyType.Vector4:
                val.vector4Value = Vector4.zero;
                break;
        }
    }

    private ReorderableList _list;
	public override void OnGUI(Rect pos, SerializedProperty p, GUIContent label)
	{

		string foldText = label.text;
			
		if(p.isExpanded)
		{
            prop = p;
            if (p.depth == 0)
		    {
		        Rect reorderpos = new Rect(pos);
		        reorderpos.y += EditorGUIUtility.singleLineHeight;

		        _list = _list ?? InitDisplay(p);

		        _list.DoList(reorderpos);
		    }
		    else
		    {
                if (!reordering)
                    _list = InitDisplay(p);
                _list.DoList(pos);
            }
        }
	    else if (p.depth >0)
        {
            foldText += " [" + p.FindPropertyRelative("_key").arraySize + "]";
        }
        foldText += " <" + p.FindPropertyRelative("_keyType").propertyType + ", " + p.FindPropertyRelative("_valType").propertyType + ">";
        foldText = foldText.Replace("ObjectReference", "Object");
        if (p.depth == 0)
        {
            p.isExpanded = EditorGUI.Foldout(pos, p.isExpanded, new GUIContent(foldText), true);
        }
        else if (!p.isExpanded)
	    {
            EditorGUI.LabelField(pos,foldText);
        }
    }
	public override float GetPropertyHeight(SerializedProperty p, GUIContent label)
	{
		return p.isExpanded? InitDisplay(p).GetHeight()+base.GetPropertyHeight(p, label) : base.GetPropertyHeight(p, label);
	}
}
