using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace SV.Numbers.Editor
{
    [CustomPropertyDrawer(typeof(Matrix<>))]
    public class MatrixDrawer : PropertyDrawer
    {
        public VisualTreeAsset visualTree;

        private Foldout foldout;
        private VisualElement rowContainer;
        private VisualElement colIndexContainer;
        private VisualElement scrollView;
        
        private SerializedObject matrixObj;
        private SerializedProperty values;
        private SerializedProperty rows;
        private SerializedProperty columns;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            matrixObj = property.serializedObject;
            values = property.FindPropertyRelative("values");
            rows = property.FindPropertyRelative("rows");
            columns = property.FindPropertyRelative("columns");
            values.arraySize = columns.intValue * rows.intValue;
            
            VisualElement root = visualTree.CloneTree();
            
            foldout = root.Q<Foldout>("MainFold");
            foldout.text = property.displayName;
            foldout.RegisterCallback<ChangeEvent<bool>>(evt =>
            {
                if (!evt.newValue)
                    foldout.style.flexBasis = 30;
                else
                    foldout.style.flexBasis = 100 + 20 * (1 + rows.intValue);
            });
            
            Button colPlus = root.Q<Button>("ColPlus");
            colPlus.clicked += () =>
            {
                AddColumn();
                DrawMatrix();
            };
            Button colMinus = root.Q<Button>("ColMinus");
            colMinus.clicked += () =>
            {
                RemoveColumn();
                DrawMatrix();
            };
            
            Button rowPlus = root.Q<Button>("RowPlus");
            rowPlus.clicked += () =>
            {
                AddRow();
                DrawMatrix();
            };
            Button rowMinus = root.Q<Button>("RowMinus");
            rowMinus.clicked += () =>
            {
                RemoveRow();
                DrawMatrix();
            };
            
            rowContainer = root.Q<VisualElement>("RowContainer");
            colIndexContainer = root.Q<VisualElement>("ColsIndexContainer");
            scrollView = root.Q<VisualElement>("ScrollView");
            
            DrawMatrix();
            
            return root;
        }

        private void AddColumn()
        {
            for (int i = 0; i < rows.intValue; i++)
                values.InsertArrayElementAtIndex(columns.intValue + i * (columns.intValue + 1));
            
            columns.intValue++;
            matrixObj.ApplyModifiedProperties();
        }

        private void RemoveColumn()
        {
            if (columns.intValue == 0) return;
            
            for (int i = rows.intValue - 1; i >= 0; i--)
                values.DeleteArrayElementAtIndex((columns.intValue - 1) + i * (columns.intValue));
            
            columns.intValue--;
            matrixObj.ApplyModifiedProperties();
        }

        private void AddRow()
        {
            rows.intValue++;
            values.arraySize = columns.intValue * rows.intValue;
            matrixObj.ApplyModifiedProperties();
        }

        private void RemoveRow()
        {
            if (rows.intValue == 0) return;
            
            rows.intValue--;
            values.arraySize = columns.intValue * rows.intValue;
            matrixObj.ApplyModifiedProperties();
        }
        
        private void DrawMatrix()
        {
            colIndexContainer.Clear();
            rowContainer.Clear();
            scrollView.style.flexBasis = 20 * (1 + rows.intValue);
            foldout.style.flexBasis = 100 + 20 * (1 + rows.intValue);
            
            for (int i = 0; i < columns.intValue; i++)
            {
                Label l = new Label
                {
                    text = i.ToString(),
                    style =
                    {
                        flexBasis = 50,
                        unityTextAlign = TextAnchor.MiddleCenter,
                    }
                };
                colIndexContainer.Add(l);
            }
            
            for (int r = 0; r < rows.intValue; r++)
            {
                VisualElement row = new VisualElement
                {
                    style = {flexDirection = FlexDirection.Row}
                };
                
                Label l = new Label
                {
                    text = $"Row {r}",
                    style =
                    {
                        unityTextAlign = TextAnchor.MiddleRight,
                        flexBasis = 60
                    }
                    
                };
                row.Add(l);
                for (int c = 0; c < columns.intValue; c++)
                {
                    PropertyField field = new PropertyField
                    {
                        label = "",
                        style =
                        {
                            flexBasis = 50,
                            flexShrink = 0
                        }
                    };
                    field.BindProperty(values.GetArrayElementAtIndex(c + r * columns.intValue));
                    
                    row.Add(field);
                }
                rowContainer.Add(row);
            }
        }
    }
}
