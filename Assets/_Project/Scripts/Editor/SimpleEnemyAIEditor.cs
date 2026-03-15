using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SimpleEnemyAI))]
public class SimpleEnemyAIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default stuff (like script reference)
        serializedObject.Update();

        // Always show the Order enum at the top
        SerializedProperty orderProp = serializedObject.FindProperty("_currentOrder");
        EditorGUILayout.PropertyField(orderProp);

        EnemyOrder order = (EnemyOrder)orderProp.enumValueIndex;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField($"{order} Configuration", EditorStyles.boldLabel);

        switch (order)
        {
            case EnemyOrder.Patrol:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_patrolPoints"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_stopDistance"));
                break;

            case EnemyOrder.Guard:
                EditorGUILayout.PropertyField(serializedObject.FindProperty("_guardPost"));
                break;
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Base AI Data", EditorStyles.miniBoldLabel);

        serializedObject.ApplyModifiedProperties();
    }
}
