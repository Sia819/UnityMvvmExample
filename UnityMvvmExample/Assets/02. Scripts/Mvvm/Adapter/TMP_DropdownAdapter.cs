using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace UnityMvvmExample.Mvvm
{
    public static class TMP_DropdownProperty
    {
        public static readonly DependencyProperty SelectedIndex = DependencyProperty.Register(
            "SelectedIndex", typeof(int), typeof(TMP_Dropdown), new UnityTMP_DropdownAdapter());

        public static readonly DependencyProperty Options = DependencyProperty.Register(
            "Options", typeof(IEnumerable<string>), typeof(TMP_Dropdown), new UnityTMP_DropdownAdapter());
    }

    public class UnityTMP_DropdownAdapter : IUIComponentAdapter
    {
        private TMP_Dropdown _dropdown;
        private object _dataContext;
        private PropertyInfo _sourceProperty;

        public bool CanHandle(object uiObject) => uiObject is TMP_Dropdown;

        public Action<object> GetUpdateTargetAction(object uiObject, DependencyProperty property)
        {
            _dropdown = (TMP_Dropdown)uiObject;
            if (property == TMP_DropdownProperty.SelectedIndex)
                return value => _dropdown.value = (int)value;
            if (property == TMP_DropdownProperty.Options)
                return value => UpdateDropdownOptions((IEnumerable<string>)value);
            throw new InvalidOperationException($"Property {property.Name} not supported for TMP_Dropdown");
        }

        public Func<object> GetTargetValueFunc(object uiObject, DependencyProperty property)
        {
            var dropdown = (TMP_Dropdown)uiObject;
            if (property == TMP_DropdownProperty.SelectedIndex)
                return () => dropdown.value;
            if (property == TMP_DropdownProperty.Options)
                return () => dropdown.options.Select(option => option.text);
            throw new InvalidOperationException($"Property {property.Name} not supported for TMP_Dropdown");
        }

        public void SubscribeToValueChanged(object uiObject, DependencyProperty property, Binding binding, Action updateSourceAction)
        {
            _dropdown = (TMP_Dropdown)uiObject;
            _dataContext = GetDataContext(_dropdown);

            if (_dataContext == null)
            {
                Debug.LogError("DataContext is null. Make sure the View is properly set up.");
                return;
            }

            // Use the Binding's Path instead of the DependencyProperty's Name
            string propertyPath = binding.Path;
            _sourceProperty = _dataContext.GetType().GetProperty(propertyPath);

            if (_sourceProperty == null)
            {
                Debug.LogError($"Property '{propertyPath}' not found on ViewModel of type {_dataContext.GetType().Name}");
                return;
            }

            if (property == TMP_DropdownProperty.SelectedIndex)
            {
                _dropdown.onValueChanged.AddListener(_ => updateSourceAction());
            }
            else if (property == TMP_DropdownProperty.Options)
            {
                var sourceValue = _sourceProperty.GetValue(_dataContext);

                // Subscribe to collection changes
                if (sourceValue is INotifyCollectionChanged observableCollection)
                {
                    observableCollection.CollectionChanged += OnCollectionChanged;
                }

                // Subscribe to property changes (in case the entire collection is replaced)
                if (_dataContext is INotifyPropertyChanged notifyPropertyChanged)
                {
                    notifyPropertyChanged.PropertyChanged += OnPropertyChanged;
                }

                // Initial update
                UpdateDropdownOptions((IEnumerable<string>)sourceValue);
            }
            else
            {
                throw new InvalidOperationException($"Property {property.Name} not supported for TMP_Dropdown");
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Debug.Log("Collection changed event triggered");
            UnityMainThreadDispatcher.Instance.Enqueue(() =>
            {
                UpdateDropdownOptions((IEnumerable<string>)_sourceProperty.GetValue(_dataContext));
            });
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _sourceProperty.Name)
            {
                Debug.Log("Property changed event triggered for Options");
                UnityMainThreadDispatcher.Instance.Enqueue(() =>
                {
                    var newValue = _sourceProperty.GetValue(_dataContext);
                    UpdateDropdownOptions((IEnumerable<string>)newValue);
                });
            }
        }

        private void UpdateDropdownOptions(IEnumerable<string> options)
        {
            Debug.Log($"Updating dropdown options. Count: {options.Count()}");
            _dropdown.ClearOptions();
            _dropdown.AddOptions(options.ToList());
        }

        private object GetDataContext(UnityEngine.Component component)
        {
            var currentTransform = component.transform;
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