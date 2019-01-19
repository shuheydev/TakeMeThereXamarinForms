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

        private Essentials.Location _targetLocation;
        public Essentials.Location TargetLocation
        {
            get => _targetLocation;
            set => SetProperty(ref _targetLocation, value);
        }

        private double _directionToTarget;
        public double DirectionToTarget
        {
            get => _directionToTarget;
            set => SetProperty(ref _directionToTarget, value);
        }


        private double _distanceToTarget;
        public double DistanceToTarget
        {
            get => _distanceToTarget;
            set => SetProperty(ref _distanceToTarget, value);
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

            //目的地がセットされている場合に限る
            if (TargetInfo != null && this.Location != null)
            {
                this.TargetLocation = Utility.GetLocationFromLocalCode(this.TargetInfo.PlusCode, this.Location);
                this.TargetInfo.Latitude = this.TargetLocation.Latitude;
                this.TargetInfo.Longitude = this.TargetLocation.Longitude;

                this.DirectionToTarget = Utility.CalculateTargetDirection(this.Location, this.TargetLocation);
                this.DistanceToTarget = Utility.CalculateDistance(this.Location, this.TargetLocation);
            }


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

        public LocationInformation TargetInfo { get; private set; }
        public void SetTarget(LocationInformation targetInfo)
        {
            this.TargetInfo = targetInfo;
        }

        public void ClearTarget()
        {
            this.TargetInfo = null;
        }

    }
}
