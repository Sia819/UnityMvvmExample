using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityMvvmExample.Mvvm;

namespace UnityMvvmExample.Mvvm
{
    // TMP_InputField Property
    public static class TMP_InputFieldProperty
    {
        public static readonly DependencyProperty Text =
            DependencyProperty.Register("text", typeof(string), typeof(TMP_InputField), new UnityTMP_InputFieldAdapter());

        public static readonly DependencyProperty CharacterLimit =
            DependencyProperty.Register("characterLimit", typeof(int), typeof(TMP_InputField), new UnityTMP_InputFieldAdapter());

        public static readonly DependencyProperty IsInteractable =
            DependencyProperty.Register("interactable", typeof(bool), typeof(TMP_InputField), new UnityTMP_InputFieldAdapter());
    }

    public class UnityTMP_InputFieldAdapter : IUIComponentAdapter
    {
        public bool CanHandle(object uiObject) => uiObject is TMP_InputField;

        public Action<object> GetUpdateTargetAction(object uiObject, DependencyProperty property)
        {
            var tmpInputField = (TMP_InputField)uiObject;
            if (property == TMP_InputFieldProperty.Text)
                return value => tmpInputField.text = (string)value;
            if (property == TMP_InputFieldProperty.CharacterLimit)
                return value => tmpInputField.characterLimit = (int)value;
            if (property == TMP_InputFieldProperty.IsInteractable)
                return value => tmpInputField.interactable = (bool)value;
            throw new InvalidOperationException($"Property {property.Name} not supported for TMP_InputField");
        }

        public Func<object> GetTargetValueFunc(object uiObject, DependencyProperty property)
        {
            var tmpInputField = (TMP_InputField)uiObject;
            if (property == TMP_InputFieldProperty.Text)
                return () => tmpInputField.text;
            if (property == TMP_InputFieldProperty.CharacterLimit)
                return () => tmpInputField.characterLimit;
            if (property == TMP_InputFieldProperty.IsInteractable)
                return () => tmpInputField.interactable;
            throw new InvalidOperationException($"Property {property.Name} not supported for TMP_InputField");
        }

        public void SubscribeToValueChanged(object uiObject, DependencyProperty property, Action updateSourceAction)
        {
            var tmpInputField = (TMP_InputField)uiObject;
            if (property == TMP_InputFieldProperty.Text)
                tmpInputField.onValueChanged.AddListener(_ => updateSourceAction());
            //else if (property == TMP_InputFieldProperty.IsInteractable)
            //    // No direct event for interactable changes, you might need a custom solution if needed
            //    ;
            //else if (property == TMP_InputFieldProperty.CharacterLimit)
            //    // No direct event for character limit changes, you might need a custom solution if needed
            //    ;
            //else
            //    throw new InvalidOperationException($"Property {property.Name} not supported for TMP_InputField");
        }
    }
}
