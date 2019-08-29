using UnityEditor;
using UnityEngine;

namespace UnityLeaf.PluginEditor
{
    public class DrawHelper
    {
        public void DrawConnection(string name1, string name2)
        {
            Handles.BeginGUI();

            Rect area = GUILayoutUtility.GetRect(100f, 40.0f);

            var point1 = new Vector3(area.center.x / 3, area.center.y, 0);
            var point2 = new Vector3(area.center.x * 5 / 3, area.center.y, 0);

            Handles.color = Color.blue;
            Handles.DrawSolidDisc(point1, Vector3.forward, 4f);
            Handles.DrawSolidDisc(point2, Vector3.forward, 4f);
            Handles.DrawLine(point1, point2);
            Handles.Label(point1 + new Vector3(0, 10, 0), name1 != null ? name1 : "-");
            Handles.Label(point2 + new Vector3(0, 10, 0), name2 != null ? name2 : "-");

            Handles.EndGUI();
        }
    }
}