using System;
using UnityEngine.UI;

namespace UnityMvvmExample.Mvvm
{
    public static class ImageAdapterProperty
    { 

    }

    public class ImageAdapter : IUIComponentAdapter
    {
        public bool CanHandle(object uiObject) => uiObject is Image;

        public Func<object> GetTargetValueFunc(object uiObject, DependencyProperty property)
        {
            throw new NotImplementedException();
        }

        public Action<object> GetUpdateTargetAction(object uiObject, DependencyProperty property)
        {
            throw new NotImplementedException();
        }

        public void SubscribeToValueChanged(object uiObject, DependencyProperty property, Binding binding, Action updateSourceAction)
        {
            throw new NotImplementedException();
        }
    }
}
