using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMeThereXamarinForms.Models;

namespace TakeMeThereXamarinForms.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private double _latitude;
        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private double _longitude;
        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private double _headingNorth;
        public double HeadingNorth
        {
            get => _headingNorth;
            set => SetProperty(ref _headingNorth, value);
        }


        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            var geolocation = Geolocation.GetInstance();

            geolocation.OnGetGeolocation += (s, e) =>
            {
                this.Latitude = geolocation.Latitude;
                this.Longitude = geolocation.Longitude;
            };

            var compass = Compass.GetInstance();
            compass.OnReadingValueChanged += (s, e) =>
            {
                this.HeadingNorth = 360 - compass.HeadingNorth;
            };
            compass.Start();

            geolocation.Start(new TimeSpan(0, 0, 10));

        }


    }
}
