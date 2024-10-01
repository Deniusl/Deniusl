using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class SelectByTagMenu : EditorWindow
{
	[MenuItem("Tools/Select By Tag")]
	private static void ShowTagMenu()
	{
		GenericMenu menu = new GenericMenu();
		string[] tags = InternalEditorUtility.tags;

		foreach (string tag in tags)
		{
			menu.AddItem(new GUIContent(tag), false, SelectGameObjectsWithTag, tag);
		}

		menu.ShowAsContext();
	}

	private static void SelectGameObjectsWithTag(object tag)
	{
		string selectedTag = (string)tag;
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(selectedTag);
		Selection.objects = gameObjects;
	}
}