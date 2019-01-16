using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Essentials = Xamarin.Essentials;


namespace TakeMeThereXamarinForms.Models
{
    interface IGeolocation
    {
        double Latitude { get; set; }
        double Longitude { get; set; }
        double? Altitude { get; set; }
        double? Speed { get; set; }
        double? Cource { get; set; }


        void Start(TimeSpan timeSpan);
        void Stop();

    }

    public class Geolocation : BindableBase, IGeolocation
    {
        private Essentials.Location _location;
        public Essentials.Location Location
        {
            get => _location;
            set => SetProperty(ref _location, value);
        }

        private double _latitude;
        public double Latitude { get => _latitude; set => SetProperty(ref _latitude, value); }

        private double _longitude;
        public double Longitude { get => _longitude; set => SetProperty(ref _longitude, value); }

        private double? _altitude;
        public double? Altitude { get => _altitude; set => SetProperty(ref _altitude, value); }

        private double? _speed;
        public double? Speed { get => _speed; set => SetProperty(ref _speed, value); }

        private double? _cource;
        public double? Cource { get => _cource; set => SetProperty(ref _cource, value); }

        private double _distance;
        public double Distance
        {
            get => _distance;
            set => SetProperty(ref _distance, value);
        }

        private double _targetDirection;
        public double TargetDirection
        {
            get => _targetDirection;
            set => SetProperty(ref _targetDirection, value);
        }

        public Essentials.Location TargetLocation { get; private set; }



        private static Geolocation _singletonInstance = new Geolocation();
        private Geolocation()
        {
        }
        public static Geolocation GetInstance()
        {
            return _singletonInstance;
        }


        public event EventHandler OnGetGeolocation;


        readonly Essentials.GeolocationRequest request = new Essentials.GeolocationRequest(Essentials.GeolocationAccuracy.Best);

        private async Task UpdateInformationAsync()
        {
            this.Location = await Essentials.Geolocation.GetLocationAsync(request);

            this.Latitude = this.Location.Latitude;
            this.Longitude = this.Location.Longitude;
            this.Altitude = this.Location.Altitude;
            this.Speed = this.Location.Speed;
            this.Cource = this.Location.Course;

            this.Distance = Essentials.Location.CalculateDistance(this.Location,
                this.TargetLocation == null ? this.Location : this.TargetLocation,
                Essentials.DistanceUnits.Kilometers);

            this.TargetDirection = CalculateTargetDirection();

            OnGetGeolocation?.Invoke(this, EventArgs.Empty);
        }



        private static bool _timerWorking;
        public void Start(TimeSpan timeSpan)
        {
            _timerWorking = true;
            Device.StartTimer(timeSpan, () =>
             {
                 UpdateInformationAsync();

                 return _timerWorking;
             });
        }

        public void Stop()
        {
            _timerWorking = false;
        }


        public async Task<Essentials.Location> GetCurrentLocationNowAsync()
        {
            return await Essentials.Geolocation.GetLocationAsync();
        }

        public void SetTargetLocation(double latitude, double longitude)
        {
            this.TargetLocation = new Essentials.Location(latitude, longitude);
        }

        private double CalculateTargetDirection()
        {
            if (this.Location == null || this.TargetLocation == null)
                return double.NaN;

            var lat1 = this.Location.Latitude;
            var lon1 = this.Location.Longitude;
            var lat2 = this.TargetLocation.Latitude;
            var lon2 = this.TargetLocation.Longitude;


            double y = Math.Cos(lon2 * Math.PI / 180) * Math.Sin(lat2 * Math.PI / 180 - lat1 * Math.PI / 180);
            double x = Math.Cos(lon1 * Math.PI / 180) * Math.Sin(lon2 * Math.PI / 180) - Math.Sin(lon1 * Math.PI / 180) * Math.Cos(lon2 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180 - lat1 * Math.PI / 180);

            double dirE0 = 18 * Math.Atan2(y, x) / Math.PI;

            if (dirE0 < 0)
            {
                dirE0 = dirE0 + 360;//0~360に保つため
            }

            double dirN0 = (dirE0 + 90) % 360;

            return dirN0;
        }

    }
}
