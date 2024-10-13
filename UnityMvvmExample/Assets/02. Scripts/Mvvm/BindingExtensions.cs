using System.Windows.Input;
using UnityEngine;
using UnityEngine.UI;

namespace UnityMvvmExample.Mvvm
{
    public static class BindingExtensions
    {
        public static BindingExpression SetBinding(this Component uiObject, DependencyProperty dp, string path)
        {
            return BindingOperations.SetBinding(uiObject, dp, new Binding(path));
        }

        public static BindingExpression SetBinding(this Component uiObject, DependencyProperty dp, Binding binding)
        {
            return BindingOperations.SetBinding(uiObject, dp, binding);
        }

        public static void SetCommand(this Button button, ICommand command)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (command.CanExecute(null))
                {
                    command.Execute(null);
                }
            });
        }
    }
}
