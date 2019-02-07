using UnityEngine;
using UnityEditor;
using Pathfinding;
using System.Collections;

[CustomGraphEditor(typeof(PlatformerPathfinding), "Platformer Pathfinding")]
public class PlatformerPathfindingEditor : GraphEditor
{
    public override void OnInspectorGUI(NavGraph target)
    {
        base.OnInspectorGUI(target);
        PlatformerPathfinding g = target as PlatformerPathfinding;

        g.ground = EditorGUILayoutx.LayerMaskField("Level Layers", g.ground);
        g.width = EditorGUILayout.IntField("Width", g.width);
        g.height = EditorGUILayout.IntField("Height", g.height);
        g.resolution = EditorGUILayout.FloatField("Resolution", g.resolution);
        g.startPos = EditorGUILayout.Vector3Field("Starting Position", g.startPos);
        g.maxSlope = EditorGUILayout.FloatField("Max Angle", g.maxSlope);

    }

    public override void OnSceneGUI(NavGraph target)
    {
        base.OnSceneGUI(target);
        PlatformerPathfinding g = target as PlatformerPathfinding;
        var pos = g.startPos;
        Vector3[] verts = {new Vector3(pos.x, pos.y, pos.z),
                    new Vector3(pos.x + g.width, pos.y, pos.z),
                    new Vector3(pos.x + g.width, pos.y + g.height, pos.z),
                    new Vector3(pos.x, pos.y + g.height, pos.z) };
        Handles.DrawSolidRectangleWithOutline(verts, new Color(1, 1, 1, 0.2f), new Color(0, 0, 0, 1));
        if (Tools.current == Tool.Move)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newTargetPosition = Handles.PositionHandle(g.startPos, Quaternion.identity);
            if (EditorGUI.EndChangeCheck() && Tools.viewTool != ViewTool.Orbit)
            {
                g.startPos = newTargetPosition;
            }
        }

    }
}