using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class SceneOpener : EditorWindow
{
    private Vector2 scrollPos;
    private List<string> scenePaths;
    private List<string> allScenes;
    private const string ScenePathsKey = "SceneOpener_ScenePaths";
    private string selectedTag;
    private string nameSearchText = ""; // New field for name search

    [MenuItem("Window/Scene Opener")]
    public static void ShowWindow()
    {
        GetWindow<SceneOpener>("Scene Opener");
    }

    private void OnEnable()
    {
        scenePaths = new List<string>(EditorPrefs.GetString(ScenePathsKey, "").Split(';'));
        if (scenePaths.Count == 1 && string.IsNullOrEmpty(scenePaths[0]))
        {
            scenePaths.Clear();
        }

        allScenes = GetAllScenes();
        allScenes.Sort((path1, path2) =>
        {
            string name1 = Path.GetFileNameWithoutExtension(path1);
            string name2 = Path.GetFileNameWithoutExtension(path2);
            return string.Compare(name1, name2, System.StringComparison.Ordinal);
        });
    }

    private void OnDisable()
    {
        EditorPrefs.SetString(ScenePathsKey, string.Join(";", scenePaths));
    }

    private void OnGUI()
    {
        // Scene List Section
        GUILayout.Label("Scene List", EditorStyles.boldLabel);
        scrollPos = GUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < scenePaths.Count; i++)
        {
            GUILayout.BeginHorizontal();

            string sceneName = Path.GetFileNameWithoutExtension(scenePaths[i]);
            if (GUILayout.Button(sceneName, GUILayout.ExpandWidth(true)))
            {
                OpenScene(scenePaths[i]);
            }

            if (GUILayout.Button("−", GUILayout.Width(20)))
            {
                DeleteScene(i);
            }

            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Remove all", GUILayout.ExpandWidth(true)))
        {
            RemoveAllScenes();
        }

        if (GUILayout.Button("Add", GUILayout.ExpandWidth(true)))
        {
            ShowAddSceneMenu();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        // Combined Tag and Name Search Section
        GUILayout.Label("Search by:", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name", GUILayout.Width(37));
        
        nameSearchText = GUILayout.TextField(nameSearchText, GUILayout.ExpandWidth(true));
        GUILayout.Space(3);
        // Name Search Button
        if (GUILayout.Button("Select", GUILayout.Width(60), GUILayout.ExpandWidth(false)))
        {
            SelectObjectsWithName(nameSearchText);
        }

        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Tag", GUILayout.Width(37));
        // Tag Search Button
        if (GUILayout.Button("Select", GUILayout.Width(60), GUILayout.ExpandWidth(true)))
        {
            ShowTagMenu();
        }
        GUILayout.EndHorizontal();
    }


    private List<string> GetAllScenes()
    {
        List<string> scenes = new List<string>();
        string[] allFiles = Directory.GetFiles(Application.dataPath, "*.unity", SearchOption.AllDirectories);
        foreach (string file in allFiles)
        {
            string relativePath = "Assets" + file.Substring(Application.dataPath.Length).Replace("\\", "/");
            scenes.Add(relativePath);
        }
        return scenes;
    }

    private void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }

    private void DeleteScene(int index)
    {
        scenePaths.RemoveAt(index);
    }

    private void RemoveAllScenes()
    {
        scenePaths.Clear();
    }

    private void AddScene(string scenePath)
    {
        if (!string.IsNullOrEmpty(scenePath) && !scenePaths.Contains(scenePath) && File.Exists(scenePath))
        {
            scenePaths.Add(scenePath);
            scenePaths.Sort((path1, path2) =>
            {
                string name1 = Path.GetFileNameWithoutExtension(path1);
                string name2 = Path.GetFileNameWithoutExtension(path2);
                return string.Compare(name1, name2, System.StringComparison.Ordinal);
            });
        }
    }

    private void ShowAddSceneMenu()
    {
        GenericMenu menu = new GenericMenu();
    
        HashSet<string> addedScenes = new HashSet<string>(scenePaths);
    
        foreach (var scenePath in allScenes)
        {
            string sceneName = Path.GetFileNameWithoutExtension(scenePath);

            if (!addedScenes.Contains(scenePath))
            {
                menu.AddItem(new GUIContent(sceneName), false, () => AddScene(scenePath));
            }
        }
    
        menu.ShowAsContext();
    }

    private void ShowTagMenu()
    {
        GenericMenu menu = new GenericMenu();
        string[] tags = UnityEditorInternal.InternalEditorUtility.tags;

        System.Array.Sort(tags, (tag1, tag2) => string.Compare(tag1, tag2, System.StringComparison.Ordinal));

        foreach (string tag in tags)
        {
            menu.AddItem(new GUIContent(tag), false, () => SelectTag(tag));
        }

        menu.ShowAsContext();
    }

    private void SelectTag(string tag)
    {
        selectedTag = tag;
        HighlightObjectsWithTag(tag);
    }

    private void HighlightObjectsWithTag(string tag)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
        List<GameObject> taggedObjects = new List<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.CompareTag(tag))
            {
                taggedObjects.Add(obj);
            }
        }

        Selection.objects = taggedObjects.ToArray();
    }

    private void SelectObjectsWithName(string name)
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
        List<GameObject> matchedObjects = new List<GameObject>();

        string lowerName = name.ToLower();

        foreach (var obj in allObjects)
        {
            if (obj.name.ToLower().Contains(lowerName))
            {
                matchedObjects.Add(obj);
            }
        }

        Selection.objects = matchedObjects.ToArray();
    }

}
