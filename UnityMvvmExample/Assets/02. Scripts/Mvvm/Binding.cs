namespace UnityMvvmExample.Mvvm
{
    public enum BindingMode
    {
        TwoWay = 0,
        OneWay = 1,
        OneTime = 2,
        OneWayToSource = 3,
        Default = 4
    }

    public class Binding
    {
        public string Path { get; set; }
        public BindingMode Mode { get; set; } = BindingMode.Default;

        public Binding() { }

        public Binding(string path)
        {
            if (path != null)
                Path = path;
        }
    }
}