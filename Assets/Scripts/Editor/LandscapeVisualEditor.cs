using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LandscapeVisual))]
public class LandscapeVisualEditor : Editor
{
    ReorderableList list;
    LandscapeVisual landscapeBuilder;
    float lineHeight;
    float lineHeightSpace;

    private void OnEnable()
    {
        if (target == null)
        {
            return;
        }
        lineHeight = EditorGUIUtility.singleLineHeight;
        lineHeightSpace = lineHeight + 10;

        landscapeBuilder = (LandscapeVisual)target;
        var cells = landscapeBuilder.CellPrefab;
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("CellPrefab"), true, true, true, true);
        list.elementHeight = 60;
        list.drawHeaderCallback = (Rect rect) => 
        {
            EditorGUI.LabelField(rect, "Block Types");
        };
        list.onAddCallback = (ReorderableList list) =>
        {
            cells.Add(null);
        };
        list.onRemoveCallback = (ReorderableList list) =>
        {
            cells.RemoveAt(list.index);
        };
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var newObject = EditorGUI.ObjectField(new Rect(rect.x + 60, rect.y + 5, rect.width / 2, lineHeight), cells[index], typeof(GameObject), false) as GameObject;
            cells[index] = newObject;
            Texture2D icon;
            if (newObject != null)
            {
                SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

                icon = AssetPreview.GetAssetPreview(cells[index]);
            }
            else
            {
                icon = Texture2D.whiteTexture;
            }
            EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y + 5, 50, 50), icon);
        };
    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        list.DoLayoutList();
        if (GUILayout.Button("Build"))
        {
            landscapeBuilder.Build();
        }
    }
}
