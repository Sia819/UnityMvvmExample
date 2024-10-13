using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityMvvmExample.Mvvm;

namespace Assets._02._Scripts.Mvvm.Adapter
{
    // TMP_Text Property
    public static class TMP_TextProperty
    {
        public static readonly DependencyProperty Text =
            DependencyProperty.Register("text", typeof(string), typeof(TMP_Text), new UnityTMP_TextAdapter());

        public static readonly DependencyProperty Color =
            DependencyProperty.Register("color", typeof(Color), typeof(TMP_Text), new UnityTMP_TextAdapter());

        public static readonly DependencyProperty FontSize =
            DependencyProperty.Register("fontSize", typeof(float), typeof(TMP_Text), new UnityTMP_TextAdapter());
    }

    // TMP_Text Adapter
    public class UnityTMP_TextAdapter : IUIComponentAdapter
    {
        public bool CanHandle(object uiObject) => uiObject is TMP_Text;

        public Action<object> GetUpdateTargetAction(object uiObject, DependencyProperty property)
        {
            var tmpText = (TMP_Text)uiObject;
            if (property == TMP_TextProperty.Text)
                return value => tmpText.text = (string)value;
            if (property == TMP_TextProperty.Color)
                return value => tmpText.color = (Color)value;
            if (property == TMP_TextProperty.FontSize)
                return value => tmpText.fontSize = (float)value;
            throw new InvalidOperationException($"Property {property.Name} not supported for TMP_Text");
        }

        public Func<object> GetTargetValueFunc(object uiObject, DependencyProperty property)
        {
            var tmpText = (TMP_Text)uiObject;
            if (property == TMP_TextProperty.Text)
                return () => tmpText.text;
            if (property == TMP_TextProperty.Color)
                return () => tmpText.color;
            if (property == TMP_TextProperty.FontSize)
                return () => tmpText.fontSize;
            throw new InvalidOperationException($"Property {property.Name} not supported for TMP_Text");
        }

        public void SubscribeToValueChanged(object uiObject, DependencyProperty property, Action updateSourceAction)
        {
            // TMP_Text doesn't have built-in events for property changes
            // If needed, you could implement a custom solution here
        }
    }
}
