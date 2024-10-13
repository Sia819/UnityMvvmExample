using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace UnityMvvmExample.Mvvm
{
    public abstract class ViewBase : MonoBehaviour
    {
        public object DataContext { get; set; }

        protected virtual void Awake()
        {
            // Base initialization logic
        }
    }

    public abstract class ViewBase<T> : ViewBase where T : INotifyPropertyChanged
    {
        public T ViewModel
        {
            get => (T)base.DataContext;
            set => base.DataContext = value;
        }

        protected override void Awake()
        {
            base.Awake();
            ViewModel = ViewModelLocator.Instance.Services.GetRequiredService<T>();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        protected virtual void OnDestroy()
        {
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }
        }

        protected virtual void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Override in derived classes if needed
        }
    }


    /// <summary>
    /// <![CDATA[
    /// public class MainView : OldViewBase<MainViewModel>
    /// {
    ///     [SerializeField] private TMP_Text resultText;
    ///     [SerializeField] private TMP_InputField displayNameInput;
    ///     [SerializeField] private Button applyButton;
    ///     
    ///     private void Start()
    ///     {
    ///         TwoWayBinding<string>(nameof(ViewModel.Name),
    ///                               () => displayNameInput.text,
    ///                               value => displayNameInput.text = value,
    ///                               displayNameInput.onValueChanged);
    /// 
    ///         OneWayBinding<string>(nameof(ViewModel.DisplayName),
    ///                               value => resultText.text = value);
    /// 
    ///         CommandBinding(applyButton.onClick,
    ///                        () => ViewModel.ApplyCommand.Execute(null));
    ///     }
    /// }
    /// ]]>
    /// </summary>
    [Obsolete]
    public abstract class OldViewBase<T> : MonoBehaviour where T : INotifyPropertyChanged
    {
        public object DataContext { get; set; }
    
        public T ViewModel => (T)DataContext;
    
        protected virtual void Awake()
        { 
            DataContext = ViewModelLocator.Instance.Services.GetRequiredService<T>();
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
    
        protected TProperty GetPropertyValue<TProperty>(object obj, string propertyName)
        {
            return (TProperty)obj.GetType().GetProperty(propertyName).GetValue(obj);
        }
    
        protected void SetPropertyValue<TProperty>(object obj, string propertyName, TProperty value)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value);
        }
    
        protected void OneWayBinding<TProperty>(string propertyName,
                                                Action<TProperty> updateViewAction)
        {
            Observable.FromEvent<PropertyChangedEventHandler, string>(h => (s, e) => h(e.PropertyName),
                                                                      h => ViewModel.PropertyChanged += h,
                                                                      h => ViewModel.PropertyChanged -= h)
                      .Where(prop => prop == propertyName)
                      .Select(_ => GetPropertyValue<TProperty>(DataContext, propertyName))
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
                SetPropertyValue(DataContext, propertyName, getViewValue());
            });
    
            // ViewModel to View
            OneWayBinding<TProperty>(propertyName, setViewValue);
        }
    
        protected void CommandBinding(UnityEvent viewEvent, Action command)
        {
            viewEvent.AddListener(() => command());
        }
    }
}
