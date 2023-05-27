using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Dienstplan
{
    public partial class App : Application
    {
        public App()
        {
            var vCulture = new CultureInfo("en");

            //Thread.CurrentThread.CurrentCulture = vCulture;
            //Thread.CurrentThread.CurrentUICulture = vCulture;

            Services = ConfigureServices();

            InitializeComponent();
        }
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }

        private static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(ServiceLifetime.Singleton);
            services.AddSingleton<IMessenger>(WeakReferenceMessenger.Default);
            
            services.AddScoped<MainViewModel>();
            services.AddScoped<EditEmployeeViewModel>();
            services.AddScoped<EditGroupViewModel>();
            services.AddScoped<EmployeesViewModel>();
            services.AddScoped<GroupsViewModel>();
            services.AddScoped<RosterViewModel>();
            services.AddScoped<WeekSelectorViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
