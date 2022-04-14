using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(ComponentSorter))]
public class ComponentsSorterInspector : Editor
{
    private ComponentSorter m_MyClass;

    private ComponentSorter CastTarget => target as ComponentSorter;

    public override void OnInspectorGUI()
    {
        var instructionsStyle = new GUIStyle(GUI.skin.label);
        instructionsStyle.fontSize = 14;
        GUILayout.Label(@"This will do the following things:
1. Will order components according to the reorderable list below.
2. There are special, reserved names that cannot be removed. 
3. Duplicate elements will be removed prior to sorting.
4. Components that fit a certain criterion will be sorted alphabetically.
5. Component names should NOT have spaces. Spaces will be removed."
, instructionsStyle);

        GUILayout.Label("________________________________________________________________________________________________________________________________________________________________");
        GUILayout.Space(10);

        base.OnInspectorGUI();

        var isEditingPrefab = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() == null;
        if (isEditingPrefab)
        {
            GUI.contentColor = Color.red;
            GUILayout.Label("You must be in Prefab mode to sort.");
            GUI.contentColor = Color.white;
        }
        else if (CastTarget.HasDuplicates)
        {
            GUI.contentColor = Color.red;
            GUILayout.Label("Duplicate elements will be removed when pressing the button!");
            GUI.contentColor = Color.white;
        }

        EditorGUI.BeginDisabledGroup(isEditingPrefab);
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Sort Components"))
            {
                SortComponents();
            }

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("Shuffle Components"))
            {
                ShuffleComponents();
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        EditorGUI.EndDisabledGroup();
    }

    private void SortComponents()
    {
        if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
        {
            m_MyClass = CastTarget;
            m_MyClass.SortComponents();
        }
        else
        {
            Debug.LogError("Sorting only works in Prefab mode");
        }
    }

    private void ShuffleComponents()
    {
        if (UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage() != null)
        {
            m_MyClass = CastTarget;
            m_MyClass.ShuffleComponents();
        }
        else
        {
            Debug.LogError("Randomizing only works in Prefab mode");
        }
    }
}

