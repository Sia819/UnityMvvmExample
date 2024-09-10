using Microsoft.Extensions.DependencyInjection;
using System;
using UnityMvvmExample.ViewModels;

namespace UnityMvvmExample.Mvvm
{
    public class ViewModelLocator
    {
        private static ViewModelLocator _instance;
        public static ViewModelLocator Instance => _instance ??= (_instance = new ViewModelLocator());

        public IServiceProvider Services { get; }

        public ViewModelLocator()
        {
            var services = new ServiceCollection();
            services.AddSingleton<MainViewModel>();
            Services = services.BuildServiceProvider();
        }

        public MainViewModel MainViewModel => Services.GetRequiredService<MainViewModel>();
    }
}