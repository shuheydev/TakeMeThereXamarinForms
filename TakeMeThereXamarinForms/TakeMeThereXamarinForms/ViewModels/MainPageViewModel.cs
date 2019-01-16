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

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            this.Geolocation = Geolocation.GetInstance();
            this.Geolocation.Start(new TimeSpan(0, 0, 10));

            this.Compass = Compass.GetInstance();
            this.Compass.Start();

        }


    }
}
