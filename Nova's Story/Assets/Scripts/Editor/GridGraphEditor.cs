﻿using UnityEditor;
using Pathfinding;
[CustomGraphEditor(typeof(PlatformerGenerator), "Platformer")]
public class GridGraphEditor : GraphEditor
{
    // Here goes the GUI
    public override void OnInspectorGUI(NavGraph target)
    {
        var graph = target as PlatformerGenerator;
        graph.ground = EditorGUILayout.LayerField("Ground Layer", graph.ground);
        graph.startPos = EditorGUILayout.Vector3Field("Start Position", graph.startPos);
        graph.width = EditorGUILayout.IntField("Width", graph.width);
        graph.height = EditorGUILayout.IntField("Height", graph.height);
        graph.maxAngle = EditorGUILayout.FloatField("Max Slope", graph.maxAngle);
        graph.jumpHeight = EditorGUILayout.IntField("Jump Height", graph.jumpHeight);
        graph.scale = EditorGUILayout.FloatField("Scale", graph.scale);
    }
}