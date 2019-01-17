using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using TakeMeThereXamarinForms.Models;
using System.Collections.ObjectModel;

namespace TakeMeThereXamarinForms.ViewModels
{
    public class SelectTargetPageViewModel : BindableBase, INavigationAware
    {
        private INavigationService _navigationService;
        public SelectTargetPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            //RestoreList();
        }


        public Command<string> NavigateCommand =>
            new Command<string>(name =>
            {
                _navigationService.NavigateAsync(name);
            });

        public ObservableCollection<TargetInformation> Targets { get; set; } = new ObservableCollection<TargetInformation>();

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            RestoreList();
        }

        private void RestoreList()
        {
            var infos = App.Database.GetItemsAsync().Result;
            Targets.Clear();
            foreach (var info in infos)
            {
                Targets.Add(info);
            }
        }

        public void OnNavigatingTo(INavigationParameters parameters)
        {
        }
    }
}
