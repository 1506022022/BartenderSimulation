using System.Linq;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using static MyCustomEditor;



[CustomEditor(typeof(Features), true), CanEditMultipleObjects]
public class FeaturesEditor : Editor
{
    string[] popups;
    public override void OnInspectorGUI()
    {
        Features targetScript = (Features)target;
        popups = targetScript.ItemDictionary.Keys.ToArray();
        var titleStyle = GetTitleStyle();
        var contentStyle = GetContentStyle();

        // 팝업
        int selectedOption = popups.ToList().IndexOf(targetScript.selectedOption);
        var index = EditorGUILayout.Popup("Select Option", selectedOption, popups);
        if (selectedOption != index)
        {
            targetScript.selectedOption = popups[index];
        }

        GUILayout.Space(10);
        LabelField("아래에 " + popups[index] + "의 데이터를 입력하세요.", titleStyle);
        GUILayout.Space(10);

        // 아이템 정보
        LabelField("아이템데이터", contentStyle);
        IntField("ID", ref targetScript.ID);
        TextArea("Name", ref targetScript.Name);
        TextArea("Description", ref targetScript.Description);

        // 데이터 입력
        targetScript.GetCustomEditor()?.UpdateInspectorOverride(targetScript);

        // 변경 내용을 저장
        if (UnityEngine.GUI.changed)
        {
            //targetScript.UpdateFile();
            EditorUtility.SetDirty(targetScript);
            AssetDatabase.SaveAssets();
        }
    }
}
#endif
public interface ICustomEditorItemData
{
    public void Create(GameObject go);
    public void Destroy(GameObject go);
#if UNITY_EDITOR
    public void UpdateInspectorOverride(Features features);
    public Mesh GetGizmosMesh();
#endif
}
public abstract class CustomEditorItemData : ScriptableObject, ICustomEditorItemData
{
    public Material _material;
    public Mesh _mesh;
    public abstract void Create(GameObject go);

    public virtual void Destroy(GameObject go) { }

#if UNITY_EDITOR
    public Mesh GetGizmosMesh()
    {
        return _mesh;
    }
    public void UpdateInspector(Features features)
    {
        UpdateInspectorOverride(features);
        EditorUtility.SetDirty(this);
    }
    public abstract void UpdateInspectorOverride(Features features);
#endif
}
#if UNITY_EDITOR
#region CustomEditor


public static class MyCustomEditor
{
    public static void ObjectField<T>(string label, ref T field) where T : UnityEngine.Object
    {
        field = EditorGUILayout.ObjectField(label, field, typeof(T), true) as T;
    }
    public static void Toggle(string label, ref bool target)
    {
        target = EditorGUILayout.Toggle(label, target);
    }
    public static GUIStyle GetTitleStyle()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(50, 50, 50);
        style.fontSize = 12;
        style.fontStyle = FontStyle.Bold;
        return style;
    }
    public static GUIStyle GetContentStyle()
    {
        GUIStyle style = new GUIStyle();
        style.normal.textColor = new Color(0.776f, 0.776f, 0.776f);
        style.fontSize = 12;
        style.fontStyle = FontStyle.Bold;
        return style;
    }

    public static void LabelField(string text, GUIStyle style = null)
    {
        if (style == null) EditorGUILayout.LabelField(text);
        else EditorGUILayout.LabelField(text, style);
    }

    public static void IntField(string label, ref int intValue)
    {
        intValue = EditorGUILayout.IntField(label, intValue);
    }
    public static void FloatField(string label, ref float floatValue)
    {
        floatValue = EditorGUILayout.FloatField(label, floatValue);
    }
    public static void TextArea(string label, ref string stringValue_Area)
    {
        var s = GetTitleStyle();
        EditorGUILayout.LabelField(label);
        stringValue_Area = EditorGUILayout.TextArea(stringValue_Area);
    }
    public static void Vector4Field(string label, ref Vector4 value)
    {
        value = EditorGUILayout.Vector4Field(label, value);
    }
    public static void Vector3Field(string label, ref Vector3 value)
    {
        value = EditorGUILayout.Vector3Field(label, value);
    }
    public static void ColorField(string label, ref Color value)
    {
        value = EditorGUILayout.ColorField(label, value);
    }
    public static void IntListField(string label, List<int> list)
    {
        var contentStyle = GetContentStyle();
        LabelField(label, contentStyle);
        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.BeginHorizontal();
            list[i] = EditorGUILayout.IntField("Element " + i, list[i]);
            if (GUILayout.Button("Remove"))
            {
                list.RemoveAt(i);
                i--;
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Element"))
        {
            list.Add(0);
        }
    }
    public static void ObjectListField<T>(string label, List<T> list) where T : UnityEngine.Object
    {
        var contentStyle = GetContentStyle();
        LabelField(label, contentStyle);

        for (int i = 0; i < list.Count; i++)
        {
            GUILayout.BeginHorizontal();

            list[i] = EditorGUILayout.ObjectField("Element " + i, list[i], typeof(T), true) as T;

            if (GUILayout.Button("Remove"))
            {
                list.RemoveAt(i);
                i--;
            }

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add Element"))
        {
            list.Add(null); // 초기화되지 않은 GameObject을 추가할 수도 있습니다.
        }
    }

}

#endregion
#endif
