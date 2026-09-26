using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SV.Numbers.Editor
{
    [CustomPropertyDrawer(typeof(BoundFloat))]
    public class BoundFloatDrawer : PropertyDrawer
    {
        public VisualTreeAsset visualTree;

        private FloatField minField;
        private FloatField maxField;
        private Slider valueSlider;

        private readonly StyleColor warningColor = new Color(1, 0, 0, 0.6f);
        private readonly StyleColor baseColor = new Color(0, 0, 0, 0.0f);
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = visualTree.CloneTree();
            root.Q<Foldout>("Foldout").text = property.displayName;
            
            minField = root.Q<FloatField>("MinField");
            maxField = root.Q<FloatField>("MaxField");
            valueSlider = root.Q<Slider>("ValueSlider");
            
            minField.RegisterCallback<ChangeEvent<float>>(evt =>
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
            maxField.RegisterCallback<ChangeEvent<float>>(evt =>
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
