using Prism.AppModel;
using Prism.Navigation;
using TakeMeThereXamarinForms.Models;
using TakeMeThereXamarinForms.Repositories;
using Xamarin.Forms;

namespace TakeMeThereXamarinForms.ViewModels
{
    public class MainPageViewModel : ViewModelBase, INavigationAware, IApplicationLifecycleAware
    {
        public MainPageViewModel(
            INavigationService navigationService,
            IApplicationStore applicationStore,
            IPlaceRepository placeRepository)
            : base(navigationService)
        {
            this._applicationStore = applicationStore;
            this._placeRepository = placeRepository;
            Title = "Main Page";

            this.Geolocation = App.Geolocation;
            this.Compass = App.Compass;
            this.Compass.SetGeolocation(this.Geolocation);

            RestoreInfo();
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

        private readonly IApplicationStore _applicationStore;
        private readonly IPlaceRepository _placeRepository;

        public Command<string> NavigateCommand =>
            new Command<string>(name =>
            {
                //
                //if (this.Geolocation.TargetInfo != null)
                //    App.Database.SaveItemAsync(this.Geolocation.TargetInfo);

                this.NavigationService.NavigateAsync(name);
            });

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var targetInfo = parameters[nameof(Place)] as Place;

            if (targetInfo == null)
                return;

            this.TargetInfo = targetInfo;
            this.Geolocation.SetTarget(this.TargetInfo);

            //即更新したい
            //this.Geolocation.UpdateInformationAsync();

            StoreInfo();
        }

        public void OnResume()
        {
            RestoreInfo();
        }

        public void OnSleep()
        {
            StoreInfo();
        }

        private void StoreInfo()
        {
            this._applicationStore.Properties[nameof(TargetInfo)] = TargetInfo;
            this._applicationStore.SavePropertiesAsync();
        }

        private void RestoreInfo()
        {
            if (this._applicationStore.Properties.TryGetValue(nameof(TargetInfo), out var targetInfo))
            {
                this.TargetInfo = TargetInfo;
            }
            else
            {
                this.TargetInfo = new Place();
            }
        }
    }
}