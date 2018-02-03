using UnityEngine;
using UnityEditor;
using Pathfinding;
using System.Collections;

[CustomGraphEditor (typeof(PlatformerPathfinding), "Platformer Pathfinding")]
public class PlatformerPathfindingEditor : GraphEditor {
	
	public override void OnInspectorGUI (NavGraph target) 
	{			
		PlatformerPathfinding g = target as PlatformerPathfinding;		
        
		g.ground = EditorGUILayoutx.LayerMaskField ("Level Layers", g.ground);
        g.width = EditorGUILayout.IntField("Width", g.width);
        g.height = EditorGUILayout.IntField("Height", g.height);
        g.startPos = EditorGUILayout.Vector2Field("Starting Position", g.startPos);
        g.scale = EditorGUILayout.FloatField("Scale", g.scale);
    }
}