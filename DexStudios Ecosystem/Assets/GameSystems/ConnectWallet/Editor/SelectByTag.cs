using UnityEngine;
using UnityEditor;

public class SelectByAnyTag : EditorWindow
{
	private string[] tags;
	private int selectedTagIndex;

	[MenuItem("Tools/Select By Any Tag")]
	public static void ShowWindow()
	{
		GetWindow<SelectByAnyTag>("Select By Any Tag");
	}

	private void OnEnable()
	{
		// Получение всех доступных тегов
		tags = UnityEditorInternal.InternalEditorUtility.tags;
		if (tags.Length > 0)
		{
			selectedTagIndex = 0;
		}
	}

	private void OnGUI()
	{
		GUILayout.Label("Select GameObjects by Tag", EditorStyles.boldLabel);

		// Выбор тега из выпадающего списка
		selectedTagIndex = EditorGUILayout.Popup("Tag to Select", selectedTagIndex, tags);

		if (GUILayout.Button("Select"))
		{
			SelectGameObjectsWithTag(tags[selectedTagIndex]);
		}
	}

	private void SelectGameObjectsWithTag(string tag)
	{
		if (string.IsNullOrEmpty(tag))
		{
			Debug.LogWarning("Selected tag is empty or null.");
			return;
		}

		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
		Selection.objects = gameObjects;
		Debug.Log($"Selected {gameObjects.Length} GameObjects with tag '{tag}'.");
	}
}