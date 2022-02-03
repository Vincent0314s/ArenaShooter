using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnumListEditor : EditorWindow
{

    [SerializeField]
    private List<EnumList> enumLists = new List<EnumList> {
        new EnumList("VisualEffect"),
        new EnumList("SoundEffect"),
        new EnumList("UI_SoundEffects"),
        new EnumList("AIOwner"),
    };

    private MultipleEnumList _multiepleEnumList;
    //Need to Update path in EnumListManager as well
    string path = "Assets/External/Vincent_Tools/Lists/EnumListAsset.asset";



    [MenuItem("VincentTools/Enum/Enum Generator")]
    public static void ShowWindow()
    {
        GetWindow<EnumListEditor>("EnumList");
    }

    [MenuItem("VincentTools/Enum/Generate Scene List")]
    public static void GenerateSceneList()
    {
        EnumListManager.AddSceneList();
    }

    private void OnEnable()
    {
        LoadListAsset();

        //Old loading function
        //var data = EditorPrefs.GetString("SaveList", JsonUtility.ToJson(this, false));
        //JsonUtility.FromJsonOverwrite(data, this);


    }


    void OnGUI()
    {
        ScriptableObject target = this;
        SerializedObject so = new SerializedObject(target);
        SerializedProperty stringsProperty = so.FindProperty("enumLists");

        EditorGUILayout.PropertyField(stringsProperty, true);
        so.ApplyModifiedProperties();

        if (GUILayout.Button("Update Enum List", GUILayout.Width(200), GUILayout.Height(30)))
        {
            EnumListManager.AddNewEnum(enumLists);

            CreateListAsset();
        }

        //if (GUILayout.Button("Load Enum List", GUILayout.Width(200), GUILayout.Height(30)))
        //{
        //    LoadListAsset();
        //}
    }

    void CreateListAsset() {
        MultipleEnumList asset = ScriptableObject.CreateInstance<MultipleEnumList>();

        _multiepleEnumList = asset;
        _multiepleEnumList.enumLists = new List<EnumList>(enumLists);
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
    }

    void LoadListAsset() {
        _multiepleEnumList = AssetDatabase.LoadAssetAtPath(path, typeof(MultipleEnumList)) as MultipleEnumList;
        if (_multiepleEnumList)
        {
            enumLists = new List<EnumList>(_multiepleEnumList.enumLists);
        }
    }




    //public MultipleEnumList multipleEnumList;

    //[MenuItem("VincentTools/Enum/Enum Generator")]
    //static void Init()
    //{
    //    EditorWindow.GetWindow(typeof(EnumListEditor));
    //}

    //void OnEnable()
    //{
    //    if (EditorPrefs.HasKey("ListPath"))
    //    {
    //        string objectPath = EditorPrefs.GetString("ListPath");
    //        multipleEnumList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(MultipleEnumList)) as MultipleEnumList;
    //    }
    //}

    //private void OnGUI()
    //{
    //    GUILayout.BeginHorizontal();
    //    GUILayout.Label("EnumList Editor", EditorStyles.boldLabel);
    //    if (GUILayout.Button("Create New Item List", GUILayout.ExpandWidth(false)))
    //    {
    //        CreateNewEnumList();
    //    }

    //    if (GUILayout.Button("Add Enum List", GUILayout.Width(200), GUILayout.Height(30)))
    //    {
    //        AddList();
    //    }

    //    if (multipleEnumList.enumLists.Count > 0) {


    //    }

    //    if (GUI.changed)
    //    {
    //        EditorUtility.SetDirty(multipleEnumList);
    //    }
    //}

    //void CreateNewEnumList() {
    //    // There is no overwrite protection here!
    //    // There is No "Are you sure you want to overwrite your existing object?" if it exists.
    //    // This should probably get a string from the user to create a new name and pass it ...
    //    multipleEnumList = CreateEnumListChild.Create();
    //    if (multipleEnumList)
    //    {
    //        multipleEnumList.enumLists = new List<EnumList>();
    //        string relPath = AssetDatabase.GetAssetPath(multipleEnumList);
    //        EditorPrefs.SetString("ListPath", relPath);
    //    }
    //}

    //void AddList() {
    //    EnumList newList = new EnumList();
    //    newList.name = "New List";
    //    multipleEnumList.enumLists.Add(newList);

    //}
}
