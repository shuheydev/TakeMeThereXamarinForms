using Prism.Commands;
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


namespace TakeMeThereXamarinForms.ViewModels
{
    public class TargetDetailPageViewModel : BindableBase,INavigationAware
    {
        private INavigationService _navigationService;
        public TargetDetailPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.TargetInfo = new LocationInformation();
        }


        private LocationInformation _targetInfo;
        public LocationInformation TargetInfo
        {
            get => _targetInfo;
            set => SetProperty(ref _targetInfo, value);
        }

        public Command SaveCommand =>
            new Command(_ =>
            {
                App.Database.SaveItemAsync(this.TargetInfo);

                _navigationService.GoBackAsync();
            });

        public Command CancelCommand =>
            new Command(_ =>
            {
                _navigationService.GoBackAsync();
            });

        public Command DeleteAllCommand =>
            new Command(_ => {
                App.Database.DeleteAllItemsAsync();
                _navigationService.GoBackAsync();
            });

        public Command DeleteCommand =>
            new Command(_ => {
                App.Database.DeleteItemAsync(TargetInfo);
                _navigationService.GoBackAsync();
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
                return;

            this.TargetInfo = targetInfo;
        }
    }
}
