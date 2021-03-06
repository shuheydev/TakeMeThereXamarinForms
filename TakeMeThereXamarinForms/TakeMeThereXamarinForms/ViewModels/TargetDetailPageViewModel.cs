﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using TakeMeThereXamarinForms.Models;
using System.Threading.Tasks;
using Google.OpenLocationCode;
using System.Text.RegularExpressions;
using Essentials = Xamarin.Essentials;
using System.Windows.Input;

namespace TakeMeThereXamarinForms.ViewModels
{
    public class TargetDetailPageViewModel : BindableBase, INavigationAware
    {
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private INavigationService _navigationService;
        public TargetDetailPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.TargetInfo = new LocationInformation();
            this.Geolocation = App.Geolocation;
        }


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


        public ICommand SaveCommand =>
            new DelegateCommand(() =>
            {
                App.Database.SaveItemAsync(this.TargetInfo);
                _navigationService.GoBackAsync();
            }, () =>
            {
                return !string.IsNullOrWhiteSpace(this.TargetInfo.PlusCode);
            }).
            ObservesProperty(() => this.TargetInfo.PlusCode);


        public Command CancelCommand =>
            new Command(() =>
            {
                _navigationService.GoBackAsync();
            });


        public ICommand DeleteCommand =>
            new DelegateCommand(() =>
            {
                App.Database.DeleteItemAsync(TargetInfo);
                _navigationService.GoBackAsync();
            },
                () => this.TargetInfo.Id != 0)
            .ObservesProperty(() => this.TargetInfo.Id);


        public Command OpenMapCommand =>
        new Command(() =>
        {
            if (this.TargetInfo.Id == 0)
            {
                if(this.Geolocation.Location==null)
                {
                    this.Geolocation.SetInitialLocation();
                }

                var options = new Essentials.MapLaunchOptions
                {
                    Name = "現在地",
                };
                Essentials.Map.OpenAsync(this.Geolocation.Location, options);
            }
            else
            {
                var options = new Essentials.MapLaunchOptions
                {
                    Name = this.TargetInfo.Name,
                };
                Essentials.Map.OpenAsync(new Essentials.Location(this.TargetInfo.Latitude, this.TargetInfo.Longitude), options);
            }
        });


        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            var targetInfo = parameters[nameof(LocationInformation)] as LocationInformation;

            if (targetInfo == null)
            {
                this.Title = "New";
                return;
            }

            this.TargetInfo = targetInfo;
            this.Title = "Edit";
        }
    }
}
