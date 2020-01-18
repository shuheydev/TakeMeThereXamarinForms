﻿using Android.App;
using Android.Content.PM;
using Android.OS;
using Prism;
using Prism.Ioc;
using TakeMeThereXamarinForms.Droid.Services;
using TakeMeThereXamarinForms.Repositories;
using TakeMeThereXamarinForms.Services;
using TakeMeXamarinForms.Sqlite.Repositories;

namespace TakeMeThereXamarinForms.Droid
{
    [Activity(Label = "TakeMeThereXamarinForms", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App(new AndroidInitializer()));

            Xamarin.Essentials.Platform.Init(this, bundle);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.RegisterSingleton<IPlatformInfoService, PlatformInfoService>();
            containerRegistry.RegisterSingleton<IPlaceRepository, PlaceRepository>();
        }
    }
}