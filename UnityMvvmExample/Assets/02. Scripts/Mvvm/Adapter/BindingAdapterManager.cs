using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityMvvmExample.Mvvm
{
    public interface IUIComponentAdapter
    {
        bool CanHandle(object uiObject);
        Action<object> GetUpdateTargetAction(object uiObject, DependencyProperty property);
        Func<object> GetTargetValueFunc(object uiObject, DependencyProperty property);
        void SubscribeToValueChanged(object uiObject, DependencyProperty property, Action updateSourceAction);
    }

    public class BindingAdapterManager
    {
        private static readonly List<IUIComponentAdapter> _adapters = new List<IUIComponentAdapter>();

        public static void RegisterAdapter(IUIComponentAdapter adapter)
        {
            _adapters.Add(adapter);
        }

        public static IUIComponentAdapter GetAdapter(object uiObject)
        {
            return _adapters.FirstOrDefault(adapter => adapter.CanHandle(uiObject));
        }
    }
}
