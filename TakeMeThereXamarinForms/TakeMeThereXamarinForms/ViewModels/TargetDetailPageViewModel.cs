using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using TakeMeThereXamarinForms.Models;
using System.Threading.Tasks;

namespace TakeMeThereXamarinForms.ViewModels
{
    public class TargetDetailPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        public TargetDetailPageViewModel(INavigationService navigationService)
        {
            this._navigationService = navigationService;

            this.TargetInfo = new TargetInformation();
        }


        private TargetInformation _targetInfo;
        public TargetInformation TargetInfo
        {
            get => _targetInfo;
            set => SetProperty(ref _targetInfo, value);
        }

        public Command SaveCommand =>
            new Command(_ =>
            {
                var targetInfo = new TargetInformation
                {
                    PlusCode = this.TargetInfo.PlusCode,
                    Name = this.TargetInfo.Name,
                    Latitude = this.TargetInfo.Latitude,
                    Longitude = this.TargetInfo.Longitude,
                };

                //var parameters = new NavigationParameters
                //{
                //    {"ViewModelName",nameof(TargetDetailPageViewModel) },
                //    {"TargetInfo",targetInfo },
                //};

                App.Database.SaveItemAsync(targetInfo);

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
    }
}
