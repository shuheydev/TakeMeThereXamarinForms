using Google.OpenLocationCode;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Essentials;

namespace TakeMeThereXamarinForms.Models
{
    public class LocationInformation : BindableBase
    {
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

        private DateTimeOffset _createdAt;
        public DateTimeOffset CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        private DateTimeOffset _selectedAt;
        public DateTimeOffset SelectedAt
        {
            get => _selectedAt;
            set => SetProperty(ref _selectedAt, value);
        }

        public void UpdateCoordinateFromPlusCode(Location baseLocation)
        {
            UpdateCoordinateFromPlusCode(baseLocation.Latitude, baseLocation.Longitude);
        }
        public void UpdateCoordinateFromPlusCode(double baseLatitude, double baseLongitude)
        {
            //目的地の位置情報を計算
            OpenLocationCode recoveredOlc;

            //Open Location Codeで経緯度に変換
            //ローカルコード
            var localCode = Regex.Match(this.PlusCode, "^[23456789CFGHJMPQRVWX+]+").Value;
            var olc = new OpenLocationCode(localCode);

            recoveredOlc = olc.Recover(baseLatitude, baseLongitude);

            var decoded = recoveredOlc.Decode();

            this.Latitude = decoded.CenterLatitude;
            this.Longitude = decoded.CenterLongitude;
        }
    }
}

