using System;
using System.ComponentModel;
using System.Reflection;

namespace UnityMvvmExample.Mvvm
{
    public class BindingExpression
    {
        public object DataItem { get; }
        public Binding ParentBinding { get; }
        public object ResolvedSource { get; private set; }
        public string ResolvedSourcePropertyName { get; private set; }

        private PropertyInfo _sourceProperty;
        private Action<object> _updateTargetAction;
        private Func<object> _getTargetValueFunc;

        public BindingExpression(object dataItem, Binding parentBinding, PropertyInfo sourceProperty,
                                 Action<object> updateTargetAction, Func<object> getTargetValueFunc)
        {
            DataItem = dataItem;
            ParentBinding = parentBinding;
            ResolvedSource = dataItem;
            ResolvedSourcePropertyName = sourceProperty.Name;
            _sourceProperty = sourceProperty;
            _updateTargetAction = updateTargetAction;
            _getTargetValueFunc = getTargetValueFunc;

            if (DataItem is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += OnSourcePropertyChanged;
            }
        }

        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == ResolvedSourcePropertyName)
            {
                UpdateTarget();
            }
        }

        public void UpdateSource()
        {
            if (ParentBinding.Mode == BindingMode.OneWay) return;

            object value = _getTargetValueFunc();
            _sourceProperty.SetValue(ResolvedSource, value);
        }

        public void UpdateTarget()
        {
            if (ParentBinding.Mode == BindingMode.OneWayToSource) return;

            object value = _sourceProperty.GetValue(ResolvedSource);
            _updateTargetAction(value);
        }
    }
}