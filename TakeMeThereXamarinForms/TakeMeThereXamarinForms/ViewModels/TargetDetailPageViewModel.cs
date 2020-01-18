using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Windows.Input;
using TakeMeThereXamarinForms.Models;
using TakeMeThereXamarinForms.Repositories;
using Xamarin.Forms;
using Essentials = Xamarin.Essentials;

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
        private readonly IPlaceRepository _placeRepository;

        public TargetDetailPageViewModel(INavigationService navigationService, IPlaceRepository placeRepository)
        {
            this._navigationService = navigationService;
            this._placeRepository = placeRepository;
            this.TargetInfo = new Place();
            this.Geolocation = App.Geolocation;
        }

        private Place _targetInfo;

        public Place TargetInfo
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
                //更新日時の更新
                this.TargetInfo.UpdatedAt = DateTimeOffset.Now;

                _placeRepository.AddAsync(TargetInfo);
                _navigationService.GoBackAsync();
            }, () =>
            {
                return !string.IsNullOrWhiteSpace(this.TargetInfo.PlusCode);
            })
            .ObservesProperty(() => this.TargetInfo.PlusCode);

        public Command CancelCommand =>
            new Command(() =>
            {
                _navigationService.GoBackAsync();
            });

        public ICommand DeleteCommand =>
            new DelegateCommand(() =>
            {
                //App.Database.DeleteItemAsync(TargetInfo);
                _placeRepository.DeleteAsync(TargetInfo);
                _navigationService.GoBackAsync();
            },
                () => this.TargetInfo.Id != 0)
            .ObservesProperty(() => this.TargetInfo.Id);

        public Command OpenMapCommand =>
        new Command(() =>
        {
            if (this.TargetInfo.Id == 0)
            {
                if (this.Geolocation.Location == null)
                {
                    this.Geolocation.SetInitialLocation().Wait();
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
            var targetInfo = parameters[nameof(Place)] as Place;

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