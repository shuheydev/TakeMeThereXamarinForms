using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using TakeMeThereXamarinForms.Models;
using System.Collections.ObjectModel;
using TakeMeThereXamarinForms.Views;
using Prism.AppModel;

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
        public SelectTargetPageViewModel(
            INavigationService navigationService,
            IApplicationStore applicationStore)
        {
            this.Title = "Select Destination";

            _navigationService = navigationService;
            _applicationStore = applicationStore;
            //RestoreList();
        }


        public Command<string> NavigateCommand =>
            new Command<string>(name =>
            {
                _navigationService.NavigateAsync(name);
            });

        public ObservableCollection<LocationInformation> Targets { get; set; } = new ObservableCollection<LocationInformation>();

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        private void RestoreList()
        {
            //DBから目的地リストを取得して、選択日時降順にソート。
            var targetGeolocationInfors = App.Database.GetItemsAsync().Result.OrderByDescending(info=>info.SelectedAt);
            Targets.Clear();
            foreach (var info in targetGeolocationInfors)
            {
                Targets.Add(info);
            }

            //リストの一番下に番兵を入れる。
            //これはリストの一番下のアイテムが追加ボタンにかぶって編集ボタンが押しにくいため、空のアイテムを追加する。
            Targets.Add(new LocationInformation(true));

        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
            RestoreList();
        }


        public Command<LocationInformation> ItemSelectedCommand =>
            new Command<LocationInformation>(targetInfo =>
            {
                //選択日時を更新
                targetInfo.SelectedAt = DateTimeOffset.Now;

                //DBへ反映
                App.Database.SaveItemAsync(targetInfo);

                var parameter = new NavigationParameters();
                parameter.Add(nameof(LocationInformation), targetInfo);

                _navigationService.GoBackAsync(parameter);
            });


        public Command<LocationInformation> EditItemCommand =>
            new Command<LocationInformation>(targetInfo =>
            {

                var parameters = new NavigationParameters {
                    { nameof(LocationInformation), targetInfo},
                };

                _navigationService.NavigateAsync(nameof(TargetDetailPage), parameters);
            });
    }
}
