﻿using Google.OpenLocationCode;
using Prism.Mvvm;
using System;
using System.Text.RegularExpressions;
using Xamarin.Essentials;
using static Google.OpenLocationCode.OpenLocationCode;

namespace TakeMeThereXamarinForms.Models
{
    public class Place : BindableBase
    {
        public Place()
        {
            this.CreatedAt = DateTimeOffset.Now;
            this.UpdatedAt = DateTimeOffset.Now;
            this.SelectedAt = DateTimeOffset.Now;
        }

        private Place(bool asGurdian)
        {
            this.CreatedAt = DateTimeOffset.Now;
            this.UpdatedAt = DateTimeOffset.Now;
            this.SelectedAt = DateTimeOffset.Now;

            this.IsGuardian = asGurdian;
        }

        static public Place CreateGuardian()
        {
            return new Place(true);
        }

        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int Id { get; set; }

        private string _plusCode;

        public string PlusCode
        {
            get => _plusCode;
            set => SetProperty(ref _plusCode, value);
        }

        private string _name = "";

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private double _latitude;

        public double Latitude
        {
            get => _latitude;
            set => SetProperty(ref _latitude, value);
        }

        private double _longitude;

        public double Longitude
        {
            get => _longitude;
            set => SetProperty(ref _longitude, value);
        }

        private DateTimeOffset _updateAt;

        public DateTimeOffset UpdatedAt
        {
            get => _updateAt;
            set => SetProperty(ref _updateAt, value);
        }

        public DateTimeOffset CreatedAt { get; }

        private DateTimeOffset _selectedAt;

        public DateTimeOffset SelectedAt
        {
            get => _selectedAt;
            set => SetProperty(ref _selectedAt, value);
        }

        public bool IsGuardian { get; }

        public void UpdateCoordinateFromPlusCode(Location referenceLocation)
        {
            UpdateCoordinateFromPlusCode(referenceLocation.Latitude, referenceLocation.Longitude);
        }

        public void UpdateCoordinateFromPlusCode(double referenceLatitude, double referenceLongitude)
        {
            //目的地の位置情報を計算
            OpenLocationCode recoveredOlc;

            //Open Location Codeで経緯度に変換
            //ローカルコード
            var locationObject = Utility.GetShortCodeFromPlusCode(this.PlusCode);

            recoveredOlc = locationObject.RecoverNearest(referenceLatitude, referenceLongitude);

            var decoded = recoveredOlc.Decode();

            this.Latitude = decoded.CenterLatitude;
            this.Longitude = decoded.CenterLongitude;
        }
    }
}