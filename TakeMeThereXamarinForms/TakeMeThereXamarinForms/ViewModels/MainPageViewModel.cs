using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TakeMeThereXamarinForms.Models;
using Xamarin.Forms;

namespace TakeMeThereXamarinForms.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INavigationAware
    {
        private string _targetName;
        public string TargetName
        {
            get => _targetName;
            set => SetProperty(ref _targetName, value);
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

        private double _directionNorthToTarget;
        public double DirectionNorthToTarget
        {
            get => _directionNorthToTarget;
            set => SetProperty(ref _directionNorthToTarget, value);
        }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";


            this.Geolocation = Geolocation.GetInstance();
            this.Geolocation.SetTargetLocation(35.6585805, 139.7432442);//東京タワー

            this.Geolocation.Start(new TimeSpan(0, 0, 10));

            this.Compass = Compass.GetInstance();
            this.Compass.OnReadingValueChanged += (s, e) =>
            {
                this.DirectionNorthToTarget = this.Geolocation.TargetDirection + this.Compass.CompassNorth;
            };

            this.Compass.Start();

        }


        public Command<string> TestCommand { get; set; }


        public Command<string> NavigateCommand =>
            new Command<string>(name => this.NavigationService.NavigateAsync(name));


        public override void OnNavigatingTo(INavigationParameters parameters)
        {
            var targetInfo = parameters[nameof(TargetInformation)] as TargetInformation;

            if (targetInfo == null)
                return;

            this.Geolocation.SetTargetLocation(targetInfo.Latitude, targetInfo.Longitude);
            this.TargetName = targetInfo.Name;
        }
    }
}
