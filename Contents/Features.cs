using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(menuName = "Scriptable Object/CharacterData")]
public class Features : ScriptableObject
{
    const string cashFolderPath = "Data/Features/cash/";

    public string selectedOption = "Bottle";
    Dictionary<string, ICustomEditorItemData> itemDictionary;
    public Dictionary<string, ICustomEditorItemData> ItemDictionary
    {
        get
        {
            if (itemDictionary == null)
            {
                CreateDictionary();
            }
            return itemDictionary;
        }
    }
    public ICustomEditorItemData GetCustomEditor()
    {
        var dic = ItemDictionary;
        return dic[selectedOption];
    }
    public string assetPath;
    public int ID;
    public string Name;
    public string Description;
    public BottleEditor Bottle;
    public GlassEditor Glass;
    public ShakerEditor Shaker;
    public BlenderEditor Blender;
    public GarnishEditor Garnish;

    private void CreateDictionary()
    {
        itemDictionary = new Dictionary<string, ICustomEditorItemData>()
        {
            { "Bottle", Bottle },
            { "Glass", Glass },
            { "Shaker", Shaker },
            {"Blender", Blender },
            {"Garnish", Garnish }
        };
#if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
#endif
    }
#if UNITY_EDITOR
    private void Awake()
    {
        var loadAssetPath = GetAssetPath();
    }

    public void UpdateFile()
    {
        Bottle = GetItem<BottleEditor>();
        Glass = GetItem<GlassEditor>();
        Shaker = GetItem<ShakerEditor>();
        Blender = GetItem<BlenderEditor>();
        Garnish = GetItem<GarnishEditor>();
        CreateDictionary();

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    T CreateScriptableObject<T>() where T : ScriptableObject
    {
        string fileName = (GetAssetPath() + typeof(T).Name).GetHashCode() + ".asset";

        if (!Directory.Exists(Path.Combine("Assets/Resources/", cashFolderPath)))
        {
            Directory.CreateDirectory(Path.Combine("Assets/Resources/", cashFolderPath));
            Debug.Log("폴더가 생성되었습니다: " + Path.Combine("Assets/Resources/", cashFolderPath));
        }

        T instance = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(instance, Path.Combine(Path.Combine("Assets/Resources/", cashFolderPath), fileName));

        Debug.Log("생성 " + Path.Combine(Path.Combine("Assets/Resources/", cashFolderPath), fileName));

        //EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return instance;
    }
#endif
    T GetItem<T>() where T : ScriptableObject
    {
        string fileName = (GetAssetPath() + typeof(T).Name).GetHashCode().ToString();
        string fullPath = Path.Combine(Path.Combine("Assets/Resources/", cashFolderPath), fileName);
#if UNITY_EDITOR
        if (!File.Exists(fullPath + ".asset"))
        {
            CreateScriptableObject<T>();
        }
#endif

        string path = Path.Combine(cashFolderPath, fileName);
        var result = Resources.Load<T>(path);
        return result;
    }

    string GetAssetPath()
    {
#if UNITY_EDITOR
        if (assetPath == null || assetPath == "")
        {
            assetPath = DateTime.Now.GetHashCode().ToString();
            UpdateFile();
        }
#endif
        return assetPath;
    }
}


