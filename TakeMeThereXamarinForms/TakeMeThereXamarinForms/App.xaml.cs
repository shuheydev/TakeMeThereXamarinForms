using Prism;
using Prism.AppModel;
using Prism.Ioc;
using System;
using System.IO;
using TakeMeThereXamarinForms.Data;
using TakeMeThereXamarinForms.Models;
using TakeMeThereXamarinForms.Repositories;
using TakeMeThereXamarinForms.ViewModels;
using TakeMeThereXamarinForms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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

        public App() : this(null)
        {
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        public static int GeolocationTimeSpanSeconds = 10;

        protected override async void OnInitialized()
        {
            InitializeComponent();

            ////リソース
            //Resources = new ResourceDictionary();
            //Resources.Add("TileColor", Color.FromHex("F2F3F7"));
            //Resources.Add("BackgroundColor", Color.FromHex("F2F3F7"));

            App.Geolocation.Start(new TimeSpan(0, 0, GeolocationTimeSpanSeconds));
            App.Compass.Start();

            await NavigationService.NavigateAsync($"{nameof(NavigationPage)}/{nameof(MainPage)}");
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

            App.Geolocation.Start(new TimeSpan(0, 0, GeolocationTimeSpanSeconds));
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
        /// </summary>
        public static TargetInfoDatabase Database
        {
            get
            {
                if (_database == null)
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