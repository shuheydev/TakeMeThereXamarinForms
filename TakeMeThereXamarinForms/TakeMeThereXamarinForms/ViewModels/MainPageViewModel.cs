using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMeThereXamarinForms.Models;
using Xamarin.Forms;
using Essentials = Xamarin.Essentials;

namespace TakeMeThereXamarinForms.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INavigationAware
    {
        private LocationInformation _targetInfo;
        public LocationInformation TargetInfo
        {
            get => _targetInfo;
            set => SetProperty(ref _targetInfo, value);
        }

        private double _directionNorth;
        public double DirectionNorth
        {
            get => _directionNorth;
            set => SetProperty(ref _directionNorth, value);
        }


        private double _directionToTarget;
        public double DirectionToTarget
        {
            get => _directionToTarget;
            set => SetProperty(ref _directionToTarget, value);
        }


        private Essentials.Location _location;
        public Essentials.Location Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }


        private double _distance;
        public double Distance
        {
            get => _distance;
            set => SetProperty(ref _distance, value);
        }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            App.Geolocation.OnGetGeolocation += (s, e) =>
            {
                var geolocation = s as Geolocation;

                this.Location = geolocation.Location;

                this.Distance = Utility.CalculateDistance(this.Location,geolocation.Location);
            };

            App.Compass.OnReadingValueChanged += (s, e) =>
            {
                var compass = s as Compass;

                this.DirectionNorth = 360 - compass.HeadingNorth;
            };
        }



        public Command<string> NavigateCommand =>
            new Command<string>(name => this.NavigationService.NavigateAsync(name));


        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            base.OnNavigatingTo(parameters);

            var targetInfo = parameters[nameof(LocationInformation)] as LocationInformation;

            if (targetInfo == null)
                return;

            this.TargetInfo = targetInfo;

        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
        }
    }
}
