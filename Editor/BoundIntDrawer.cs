using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SV.Numbers.Editor
{
    [CustomPropertyDrawer(typeof(BoundInt))]
    public class BoundIntDrawer : PropertyDrawer
    {
        public VisualTreeAsset visualTree;

        private IntegerField minField;
        private IntegerField maxField;
        private SliderInt valueSlider;

        private readonly StyleColor warningColor = new Color(1, 0, 0, 0.6f);
        private readonly StyleColor baseColor = new Color(0, 0, 0, 0.0f);
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = visualTree.CloneTree();
            root.Q<Foldout>("Foldout").text = property.displayName;
            
            minField = root.Q<IntegerField>("MinField");
            maxField = root.Q<IntegerField>("MaxField");
            valueSlider = root.Q<SliderInt>("ValueSlider");
            
            minField.RegisterCallback<ChangeEvent<int>>(evt =>
            {
                if (evt.newValue > maxField.value)
                {
                    minField.style.backgroundColor = warningColor;
                    maxField.style.backgroundColor = warningColor;
                }
                else
                {
                    minField.style.backgroundColor = baseColor;
                    maxField.style.backgroundColor = baseColor;
                }
                valueSlider.lowValue = evt.newValue;
            });
            maxField.RegisterCallback<ChangeEvent<int>>(evt =>
            {
                if (evt.newValue < minField.value)
                {
                    minField.style.backgroundColor = warningColor;
                    maxField.style.backgroundColor = warningColor;
                }
                else
                {
                    minField.style.backgroundColor = baseColor;
                    maxField.style.backgroundColor = baseColor;
                }
                valueSlider.highValue = evt.newValue;
            });
            return root;
        }
    }
}
