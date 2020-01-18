using Prism.AppModel;
using Prism.Navigation;
using System.Threading.Tasks;
using TakeMeThereXamarinForms.Models;
using TakeMeThereXamarinForms.Repositories;
using Xamarin.Forms;
using System.Linq;

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
                this.NavigationService.NavigateAsync(name);
            });

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            var targetInfo = parameters[nameof(Place)] as Place;

            if (targetInfo == null)
            {
                targetInfo = (await _placeRepository.ReadAll()).OrderByDescending(x => x.SelectedAt).FirstOrDefault();
            }

            if(targetInfo==null)
            {
                return;
            }

            this.TargetInfo = targetInfo;
            this.Geolocation.SetTarget(this.TargetInfo);

            await StoreInfoAsync();
        }

        public async void OnResume()
        {
            await RestoreInfo();
        }

        public async void OnSleep()
        {
            await StoreInfoAsync();
        }

        private async Task StoreInfoAsync()
        {
            this._applicationStore.Properties[nameof(TargetInfo)] = TargetInfo;
            await this._applicationStore.SavePropertiesAsync();
        }

        private async Task RestoreInfo()
        {
            //アプリケーション用の一時保存領域から取得
            if (this._applicationStore.Properties.TryGetValue(nameof(TargetInfo), out var targetInfo))
            {
                this.TargetInfo = targetInfo as Place;
                return;
            }

            //アプリケーションの一時保存領域にデータが見つからなかった場合
            //DBから探す
            var mostRecentSelectedPlaceInDB = (await _placeRepository.ReadAll()).OrderByDescending(x=>x.SelectedAt).FirstOrDefault();
            this.TargetInfo = mostRecentSelectedPlaceInDB;
        }
    }
}