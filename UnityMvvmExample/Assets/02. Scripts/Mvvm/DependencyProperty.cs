using System;

namespace UnityMvvmExample.Mvvm
{
    public class DependencyProperty
    {
        public string Name { get; }
        public Type PropertyType { get; }
        public Type OwnerType { get; }
        public IUIComponentAdapter Adapter { get; }

        private DependencyProperty(string name, Type propertyType, Type ownerType, IUIComponentAdapter adapter)
        {
            Name = name;
            PropertyType = propertyType;
            OwnerType = ownerType;
            Adapter = adapter;
        }

        public static DependencyProperty Register(string name, Type propertyType, Type ownerType, IUIComponentAdapter adapter)
        {
            return new DependencyProperty(name, propertyType, ownerType, adapter);
        }
    }
}
