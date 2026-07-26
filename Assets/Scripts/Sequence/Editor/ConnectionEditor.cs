using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Connection))]
public class ConnectionEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.Space();
        
        Connection connection = target as Connection;

        if (GUILayout.Button("Update Connection"))
        {
            Undo.RecordObject(connection, "Change Connection Points");
            connection?.ConnectPoints();
            EditorUtility.SetDirty(connection);
        }
    }
}