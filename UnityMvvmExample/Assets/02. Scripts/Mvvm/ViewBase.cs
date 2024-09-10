using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityMvvmExample.ViewModels;

namespace UnityMvvmExample.Mvvm
{
    public abstract class ViewBase<T> : MonoBehaviour where T : INotifyPropertyChanged
    {
        protected T ViewModel;

        protected virtual void Awake()
        {
            ViewModel = ViewModelLocator.Instance.Services.GetRequiredService<T>();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected virtual void OnDestroy()
        {
            ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        protected virtual void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Override in derived classes if needed
        }

        protected void OneWayBinding<TProperty>(string propertyName,
                                                Action<TProperty> updateViewAction)
        {
            Observable.FromEvent<PropertyChangedEventHandler, string>(h => (s, e) => h(e.PropertyName),
                                                                      h => ViewModel.PropertyChanged += h,
                                                                      h => ViewModel.PropertyChanged -= h)
                      .Where(prop => prop == propertyName)
                      .Select(_ => GetPropertyValue<TProperty>(ViewModel, propertyName))
                      .Subscribe(updateViewAction)
                      .AddTo(this);
        }

        protected void TwoWayBinding<TProperty>(string propertyName,
                                                Func<TProperty> getViewValue,
                                                Action<TProperty> setViewValue,
                                                UnityEvent<TProperty> viewChangedEvent)
        {
            // View to ViewModel
            viewChangedEvent.AddListener(value =>
            {
                SetPropertyValue(ViewModel, propertyName, getViewValue());
            });

            // ViewModel to View
            OneWayBinding<TProperty>(propertyName, setViewValue);
        }

        protected void CommandBinding(UnityEvent viewEvent, Action command)
        {
            viewEvent.AddListener(() => command());
        }

        private TProperty GetPropertyValue<TProperty>(object obj, string propertyName)
        {
            return (TProperty)obj.GetType().GetProperty(propertyName).GetValue(obj);
        }

        private void SetPropertyValue<TProperty>(object obj, string propertyName, TProperty value)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value);
        }
    }
}
