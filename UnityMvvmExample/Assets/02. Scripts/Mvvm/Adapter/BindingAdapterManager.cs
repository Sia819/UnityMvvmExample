using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityMvvmExample.Mvvm
{
    public interface IUIComponentAdapter
    {
        bool CanHandle(object uiObject);
        Action<object> GetUpdateTargetAction(object uiObject, DependencyProperty property);
        Func<object> GetTargetValueFunc(object uiObject, DependencyProperty property);
        void SubscribeToValueChanged(object uiObject, DependencyProperty property, Binding binding, Action updateSourceAction);
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

    public class UnityMainThreadDispatcher : MonoBehaviour
    {
        private static UnityMainThreadDispatcher _instance;
        private readonly Queue<Action> _executionQueue = new Queue<Action>();

        public static UnityMainThreadDispatcher Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<UnityMainThreadDispatcher>();
                    if (_instance == null)
                    {
                        var go = new GameObject("UnityMainThreadDispatcher");
                        _instance = go.AddComponent<UnityMainThreadDispatcher>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        public void Enqueue(Action action)
        {
            lock (_executionQueue)
            {
                _executionQueue.Enqueue(action);
            }
        }

        private void Update()
        {
            lock (_executionQueue)
            {
                while (_executionQueue.Count > 0)
                {
                    _executionQueue.Dequeue().Invoke();
                }
            }
        }
    }
}
