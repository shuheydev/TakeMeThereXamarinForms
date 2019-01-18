using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Essentials = Xamarin.Essentials;
using Google.OpenLocationCode;
using System.Text.RegularExpressions;

namespace TakeMeThereXamarinForms.Models
{
    public class Geolocation : BindableBase
    {
        private Essentials.Location _location;
        public Essentials.Location Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }




        private static Geolocation _singletonInstance = new Geolocation();
        private Geolocation()
        {
        }

        public static Geolocation GetInstance()
        {
            return _singletonInstance;
        }


        public event EventHandler OnGetGeolocation;


        private readonly Essentials.GeolocationRequest request = new Essentials.GeolocationRequest(Essentials.GeolocationAccuracy.Best);

        private async Task UpdateInformationAsync()
        {
            //現在の位置情報を取得
            this.Location = await Essentials.Geolocation.GetLocationAsync(request);
        }


        public bool IsWorking;

        public void Start(TimeSpan timeSpan)
        {
            IsWorking = true;
            Device.StartTimer(timeSpan, () =>
             {
                 UpdateInformationAsync();

                 OnGetGeolocation?.Invoke(this, EventArgs.Empty);

                 return IsWorking;
             });
        }

        public void Stop()
        {
            IsWorking = false;
        }

    }
}
