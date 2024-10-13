using System;
using UnityEngine.UI;

namespace UnityMvvmExample.Mvvm
{
    public static class ButtonProperty
    {
        public static readonly DependencyProperty Text =
            DependencyProperty.Register("text", typeof(string), typeof(Button), new ButtonAdapter());

        public static readonly DependencyProperty IsInteractable =
            DependencyProperty.Register("interactable", typeof(bool), typeof(Button), new ButtonAdapter());
    }

    public class ButtonAdapter : IUIComponentAdapter
    {
        public bool CanHandle(object uiObject) => uiObject is Button;

        public Action<object> GetUpdateTargetAction(object uiObject, DependencyProperty property)
        {
            var button = (Button)uiObject;
            if (property == ButtonProperty.Text)
                return value => button.GetComponentInChildren<Text>().text = (string)value;
            if (property == ButtonProperty.IsInteractable)
                return value => button.interactable = (bool)value;
            throw new InvalidOperationException($"Property {property.Name} not supported for Button");
        }

        public Func<object> GetTargetValueFunc(object uiObject, DependencyProperty property)
        {
            var button = (Button)uiObject;
            if (property == ButtonProperty.Text)
                return () => button.GetComponentInChildren<Text>().text;
            if (property == ButtonProperty.IsInteractable)
                return () => button.interactable;
            throw new InvalidOperationException($"Property {property.Name} not supported for Button");
        }

        public void SubscribeToValueChanged(object uiObject, DependencyProperty property, Binding binding, Action updateSourceAction)
        {
            // Buttons typically don't have properties that change from the UI side
            // If needed, you could add listeners here
        }
    }
}
