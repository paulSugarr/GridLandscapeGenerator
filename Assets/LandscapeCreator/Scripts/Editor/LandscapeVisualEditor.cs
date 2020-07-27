using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(LandscapeVisual))]
public class LandscapeVisualEditor : Editor
{
    ReorderableList reordableCells;
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
        lineHeightSpace = lineHeight + 5;

        landscapeBuilder = (LandscapeVisual)target;
        CellsListSetup();

    }


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        reordableCells.DoLayoutList();
        if (GUILayout.Button("Build"))
        {
            landscapeBuilder.Build();
        }
    }

    private void CellsListSetup()
    {
        var cells = landscapeBuilder.CellPrefab;
        reordableCells = new ReorderableList(serializedObject, serializedObject.FindProperty("CellPrefab"), true, true, true, true);
        reordableCells.elementHeight = 60;
        reordableCells.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Block Types");
        };
        reordableCells.onAddCallback = (ReorderableList list) =>
        {
            cells.Add(null);
        };
        reordableCells.onRemoveCallback = (ReorderableList list) =>
        {
            cells.RemoveAt(list.index);
        };
        reordableCells.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.y += 5;
            var newObject = EditorGUI.ObjectField(new Rect(rect.x + 60, rect.y, rect.width / 2, lineHeight), cells[index], typeof(GameObject), false) as GameObject;

            cells[index] = newObject;
            Texture2D icon;
            if (newObject != null)
            {
                SerializedProperty element = reordableCells.serializedProperty.GetArrayElementAtIndex(index);
                icon = AssetPreview.GetAssetPreview(cells[index]);
            }
            else
            {
                icon = Texture2D.grayTexture;
            }
            EditorGUI.DrawPreviewTexture(new Rect(rect.x, rect.y, 50, 50), icon);
            rect.y += lineHeightSpace;
            EditorGUI.LabelField(new Rect(rect.x + 60, rect.y, rect.width / 2, lineHeight), string.Format("Height: {0}", index));
        };
    }
}
