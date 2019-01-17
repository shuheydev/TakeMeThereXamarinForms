﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace TakeMeThereXamarinForms.Models
{
    public class TargetInformation:BindableBase
    {
        [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int Id { get; set; }

        private string _plusCode;
        public string PlusCode
        {
            get => _plusCode;
            set => SetProperty(ref _plusCode, value);
        }

        private string _name="";
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

    }
}
