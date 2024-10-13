using System;
using System.Reflection;
using UnityEngine;

namespace UnityMvvmExample.Mvvm
{
    public static class BindingOperations
    {
        public static BindingExpression SetBinding(Component uiComponent, DependencyProperty dp, Binding binding)
        {
            if (uiComponent == null || dp == null || binding == null || string.IsNullOrEmpty(binding.Path))
                throw new ArgumentException("Invalid binding parameters");

            object dataContext = GetDataContext(uiComponent);
            if (dataContext == null)
                throw new InvalidOperationException("DataContext not found");

            PropertyInfo sourceProperty = dataContext.GetType().GetProperty(binding.Path);
            if (sourceProperty == null)
                throw new InvalidOperationException($"Property {binding.Path} not found on ViewModel");

            var adapter = dp.Adapter ?? BindingAdapterManager.GetAdapter(uiComponent);
            if (adapter == null)
                throw new InvalidOperationException("Unsupported UI component or property");

            var updateTargetAction = adapter.GetUpdateTargetAction(uiComponent, dp);
            var getTargetValueFunc = adapter.GetTargetValueFunc(uiComponent, dp);

            var bindingExpression = new BindingExpression(dataContext, binding, sourceProperty, updateTargetAction, getTargetValueFunc);
            bindingExpression.UpdateTarget(); // Initial update

            adapter.SubscribeToValueChanged(uiComponent, dp, bindingExpression.UpdateSource);

            return bindingExpression;
        }

        private static object GetDataContext(Component uiComponent)
        {
            // Traverse up the hierarchy to find a ViewBase
            var currentTransform = uiComponent.transform;
            while (currentTransform != null)
            {
                var viewBase = currentTransform.GetComponent<ViewBase>();
                if (viewBase != null && viewBase.DataContext != null)
                {
                    return viewBase.DataContext;
                }
                currentTransform = currentTransform.parent;
            }

            throw new InvalidOperationException("DataContext not found in hierarchy");
        }
    }
}
