using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    private SerializedProperty _sounds;

    private void OnEnable()
    {
        _sounds = serializedObject.FindProperty("_sounds");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));
        EditorGUI.EndDisabledGroup();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        for (int i = 0; i < _sounds.arraySize; i++)
        {
            var element = _sounds.GetArrayElementAtIndex(i);
            
            if (element == null)
                continue;

            var name = element.FindPropertyRelative("_name");
            var clip = element.FindPropertyRelative("_clip");
            var volume = element.FindPropertyRelative("_volume");
            var pitch = element.FindPropertyRelative("_pitch");
            var loop = element.FindPropertyRelative("_loop");
            var playOnAwake = element.FindPropertyRelative("_playOnAwake");

            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.labelWidth = 45; 
            EditorGUILayout.PropertyField(name, new GUIContent("Name"), GUILayout.ExpandWidth(true));
            GUILayout.Space(10);
            EditorGUIUtility.labelWidth = 33; 
            EditorGUILayout.PropertyField(clip, new GUIContent("Clip"), GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();

            EditorGUIUtility.labelWidth = 35; 
            EditorGUILayout.PropertyField(volume, new GUIContent("Volume"));
            EditorGUILayout.PropertyField(pitch, new GUIContent("Pitch"));

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(loop, new GUIContent("Loop"));
            GUILayout.Space(0);
            EditorGUIUtility.labelWidth = 90; 
            EditorGUILayout.PropertyField(playOnAwake, new GUIContent("Play on awake"));

            EditorGUILayout.BeginVertical(GUILayout.Width(70));
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Remove", GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                _sounds.DeleteArrayElementAtIndex(i);
                break;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical(); 
        }

        if (GUILayout.Button("Add Sound"))
        {
            _sounds.arraySize++;
            var newElement = _sounds.GetArrayElementAtIndex(_sounds.arraySize - 1);
            if (newElement != null)
            {
                newElement.FindPropertyRelative("_name").stringValue = "New Sound";
                newElement.FindPropertyRelative("_clip").objectReferenceValue = null;
                newElement.FindPropertyRelative("_volume").floatValue = 1.0f;
                newElement.FindPropertyRelative("_pitch").floatValue = 1.0f;
                newElement.FindPropertyRelative("_loop").boolValue = false;
                newElement.FindPropertyRelative("_playOnAwake").boolValue = false;
            }
        }
        
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
