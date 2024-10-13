using System;
using UnityEngine.UI;

namespace UnityMvvmExample.Mvvm
{
    public static class InputFieldProperty
    {
        public static readonly DependencyProperty Text =
            DependencyProperty.Register("text", typeof(string), typeof(InputField), new InputFieldAdapter());
    }

    public class InputFieldAdapter : IUIComponentAdapter
    {
        public bool CanHandle(object uiObject)
        {
            return uiObject is InputField;
        }

        public Action<object> GetUpdateTargetAction(object uiObject, DependencyProperty property)
        {
            var inputField = (InputField)uiObject;
            if (property.Name == "text")
                return value => inputField.text = (string)value;
            throw new InvalidOperationException($"Property {property.Name} not supported for InputField");
        }

        public Func<object> GetTargetValueFunc(object uiObject, DependencyProperty property)
        {
            var inputField = (InputField)uiObject;
            if (property.Name == "text")
                return () => inputField.text;
            throw new InvalidOperationException($"Property {property.Name} not supported for InputField");
        }

        public void SubscribeToValueChanged(object uiObject, DependencyProperty property, Binding binding, Action updateSourceAction)
        {
            var inputField = (InputField)uiObject;
            if (property.Name == "text")
                inputField.onValueChanged.AddListener(_ => updateSourceAction());
            else
                throw new InvalidOperationException($"Property {property.Name} not supported for InputField");
        }
    }
}
