using System;
using UnityEngine.UI;

namespace UnityMvvmExample.Mvvm
{
    public static class ToggleProperty
    {
        public static readonly DependencyProperty IsOn = 
            DependencyProperty.Register("isOn", typeof(bool), typeof(Toggle), new ToggleAdapter());
    }

    public class ToggleAdapter : IUIComponentAdapter
    {
        public bool CanHandle(object uiObject) => uiObject is Toggle;

        public Action<object> GetUpdateTargetAction(object uiObject, DependencyProperty property)
        {
            var toggle = (Toggle)uiObject;
            if (property == ToggleProperty.IsOn)
                return value => toggle.isOn = (bool)value;
            throw new InvalidOperationException($"Property {property.Name} not supported for Toggle");
        }

        public Func<object> GetTargetValueFunc(object uiObject, DependencyProperty property)
        {
            var toggle = (Toggle)uiObject;
            if (property == ToggleProperty.IsOn)
                return () => toggle.isOn;
            throw new InvalidOperationException($"Property {property.Name} not supported for Toggle");
        }

        public void SubscribeToValueChanged(object uiObject, DependencyProperty property, Action updateSourceAction)
        {
            var toggle = (Toggle)uiObject;
            if (property == ToggleProperty.IsOn)
                toggle.onValueChanged.AddListener(_ => updateSourceAction());
            else
                throw new InvalidOperationException($"Property {property.Name} not supported for Toggle");
        }
    }
}
