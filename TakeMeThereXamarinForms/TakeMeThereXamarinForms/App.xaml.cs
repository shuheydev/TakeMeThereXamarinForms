using Prism;
using Prism.Ioc;
using System;
using System.IO;
using TakeMeThereXamarinForms.ViewModels;
using TakeMeThereXamarinForms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TakeMeThereXamarinForms.Data;
using TakeMeThereXamarinForms.Models;
using Prism.AppModel;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TakeMeThereXamarinForms
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Resources = new ResourceDictionary();
            Resources.Add("TileColor", Color.FromHex("FFFFFF"));

            App.Geolocation.Start(new TimeSpan(0, 0, 5));
            App.Compass.Start();


            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SelectTargetPage, SelectTargetPageViewModel>();
            containerRegistry.RegisterForNavigation<TargetDetailPage, TargetDetailPageViewModel>();


            containerRegistry.Register<IApplicationStore, ApplicationStore>();
        }

        protected override void OnResume()
        {
            base.OnResume();

            App.Geolocation.Start(new TimeSpan(0, 0, 5));
            App.Compass.Start();

            (MainPage.BindingContext as IApplicationLifecycleAware)?.OnResume();
        }

        protected override void OnSleep()
        {
            base.OnSleep();

            App.Geolocation.Stop();
            App.Compass.Stop();

            (MainPage.BindingContext as IApplicationLifecycleAware)?.OnSleep();
        }

        private static TargetInfoDatabase _database;
        /// <summary>
        /// シングルトンとしてデータベースオブジェクトを返す。
        /// これによって
        /// </summary>
        public static TargetInfoDatabase Database
        {
            get
            {
                if(_database==null)
                {
                    _database = new TargetInfoDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TakeMeThereSQLite.db3"));
                }
                return _database;
            }
        }

        public int ResumeAtTodoId { get; set; }


        public static Geolocation Geolocation => Geolocation.GetInstance();

        public static Compass Compass => Compass.GetInstance();
    }
}
