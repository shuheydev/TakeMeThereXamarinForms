﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Essentials = Xamarin.Essentials;
using Google.OpenLocationCode;
using System.Text.RegularExpressions;
using System.Linq;

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


        private double _speedKPH;
        public double SpeedKPH
        {
            get => _speedKPH;
            set => SetProperty(ref _speedKPH, value);
        }


        private double _expectedRequiredTimeToTarget;
        public double ExpectedRequiredTimeToTarget
        {
            get => _expectedRequiredTimeToTarget;
            set => SetProperty(ref _expectedRequiredTimeToTarget, value);
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

        private FixSizedQueue<Essentials.Location> _locations = new FixSizedQueue<Essentials.Location>(10);
        private async Task UpdateInformationAsync()
        {
            //現在の位置情報を取得
            this.Location = await Essentials.Geolocation.GetLocationAsync(request);

            this._locations.Enqueue(this.Location);

            //var locs = this._locations.Queue.ToList();
            //double totalDistance = 0;
            //double totalHours = 0;
            //for (int i = 0; i < locs.Count - 1; i++)
            //{
            //    var loc1 = locs[i];
            //    var loc2 = locs[i + 1];

            //    totalDistance += Utility.CalculateDistance(loc1, loc2);//km
            //    totalHours += Math.Abs((loc2.Timestamp - loc1.Timestamp).TotalHours);//h
            //}

            //var averageSpeed = totalDistance / totalHours;
            //this.SpeedKPH = double.IsNaN(averageSpeed)?0.0:averageSpeed;
            this.SpeedKPH = Utility.CalculateAverageSpeed(this._locations.Queue);

            //目的地がセットされている場合に限る
            if (TargetInfo != null && this.Location != null)
            {
                this.TargetLocation = Utility.GetLocationFromLocalCode(this.TargetInfo.PlusCode, this.Location);
                this.TargetInfo.Latitude = this.TargetLocation.Latitude;
                this.TargetInfo.Longitude = this.TargetLocation.Longitude;

                this.DirectionToTarget = Utility.CalculateTargetDirection(this.Location, this.TargetLocation);
                this.DistanceToTarget = Utility.CalculateDistance(this.Location, this.TargetLocation);

                this.ExpectedRequiredTimeToTarget = this.DistanceToTarget / this.SpeedKPH;
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
