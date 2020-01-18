using Prism.AppModel;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TakeMeThereXamarinForms.Models;
using TakeMeThereXamarinForms.Repositories;
using TakeMeThereXamarinForms.Views;
using Xamarin.Forms;

namespace TakeMeThereXamarinForms.ViewModels
{
    public class SelectTargetPageViewModel : BindableBase, INavigationAware
    {
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private INavigationService _navigationService;
        private IApplicationStore _applicationStore;
        private readonly IPlaceRepository _placeRepository;

        public SelectTargetPageViewModel(
            INavigationService navigationService,
            IApplicationStore applicationStore,
            IPlaceRepository placeRepository)
        {
            this.Title = "Select Destination";

            _navigationService = navigationService;
            _applicationStore = applicationStore;
            this._placeRepository = placeRepository;
        }

        public Command<string> NavigateCommand =>
            new Command<string>(name =>
            {
                _navigationService.NavigateAsync(name);
            });

        private ObservableCollection<Place> _places = new ObservableCollection<Place>();
        public ObservableCollection<Place> Places
        {
            get => _places;
            set => SetProperty(ref _places, value);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await RestoreList();
        }

        private async Task RestoreList()
        {
            //DBから目的地リストを取得して、選択日時降順にソート。
            //var targetGeolocationInfos = App.Database.GetItemsAsync().Result.OrderByDescending(info => info.SelectedAt);
            //var places = await _placeRepository.ReadAll();
            //Places.Clear();
            //foreach (var place in places)
            //{
            //    Places.Add(place);
            //}
            Places = new ObservableCollection<Place>((await _placeRepository.ReadAll()).OrderByDescending(x => x.SelectedAt));

            //リストの一番下に番兵を入れる。
            //これはリストの一番下のアイテムが追加ボタンにかぶって編集ボタンが押しにくいため、空のアイテムを追加する。
            Places.Add(Place.CreateGuardian());
        }

        public Command<Place> ItemSelectedCommand =>
            new Command<Place>(async targetInfo =>
            {
                //選択時のハイライトを無効にするためにView側でSelectedItem=nullにしているため、
                //アイテム選択時に2回のイベントが発火する。
                //2回目のイベントはnullがくるのでそれをはじいている。
                if (targetInfo == null)
                    return;

                //番兵の場合は処理を行わない
                if (targetInfo.IsGuardian == true)
                    return;

                //選択日時を更新
                targetInfo.SelectedAt = DateTimeOffset.Now;

                //DBへ反映
                //App.Database.SaveItemAsync(targetInfo);
                await _placeRepository.UpdateAsync(targetInfo);

                var parameter = new NavigationParameters();
                parameter.Add(nameof(Place), targetInfo);

                await _navigationService.GoBackAsync(parameter);
            });

        public Command<Place> EditItemCommand =>
            new Command<Place>(targetInfo =>
            {
                var parameters = new NavigationParameters {
                    { nameof(Place), targetInfo},
                };

                _navigationService.NavigateAsync(nameof(TargetDetailPage), parameters);
            });
    }
}