using Prism;
using Prism.Ioc;
using System;
using System.IO;
using TakeMeThereXamarinForms.ViewModels;
using TakeMeThereXamarinForms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TakeMeThereXamarinForms.Data;

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

            await NavigationService.NavigateAsync("NavigationPage/MainPage");


        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SelectTargetPage, SelectTargetPageViewModel>();
            containerRegistry.RegisterForNavigation<TargetDetailPage, TargetDetailPageViewModel>();
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
    }
}
