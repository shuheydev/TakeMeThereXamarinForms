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

        private Geolocation _geolocation;
        public Geolocation Geolocation
        {
            get => _geolocation;
            set => SetProperty(ref _geolocation, value);
        }

        private Compass _compass;
        public Compass Compass
        {
            get => _compass;
            set => SetProperty(ref _compass, value);
        }


        private double _directionToNorth;
        public double DirectionToNorth
        {
            get => _directionToNorth;
            set => SetProperty(ref _directionToNorth, value);
        }


        private double _directionToTarget;
        public double DirectionToTarget
        {
            get => _directionToTarget;
            set => SetProperty(ref _directionToTarget, value);
        }


        //private Essentials.Location _location;
        //public Essentials.Location Location
        //{
        //    get => _location;
        //    set => SetProperty(ref _location, value);
        //}


        //private double _distance;
        //public double Distance
        //{
        //    get => _distance;
        //    set => SetProperty(ref _distance, value);
        //}



        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            this.Geolocation = App.Geolocation;
            this.Compass = App.Compass;
            this.Compass.SetGeolocation(this.Geolocation);
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
            this.Geolocation.SetTarget(this.TargetInfo);

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
