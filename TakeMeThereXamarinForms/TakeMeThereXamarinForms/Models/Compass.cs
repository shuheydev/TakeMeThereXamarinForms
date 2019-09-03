using Prism.Mvvm;
using System;
using Essentials = Xamarin.Essentials;

namespace TakeMeThereXamarinForms.Models
{
    public class Compass : BindableBase
    {
        private static Compass _singletonInstance = new Compass();

        private Compass()
        { }

        public static Compass GetInstance()
        {
            return _singletonInstance;
        }

        private double _headingNorth;

        public double HeadingNorth
        {
            get => _headingNorth;
            set => SetProperty(ref _headingNorth, value);
        }

        private double _headingNorthForRotate;

        public double HeadingNorthForRotate
        {
            get => _headingNorthForRotate;
            set => SetProperty(ref _headingNorthForRotate, value);
        }

        private double _directionToTargetForRotate;

        public double DirectionToTargetForRotate
        {
            get => _directionToTargetForRotate;
            set => SetProperty(ref _directionToTargetForRotate, value);
        }

        public event EventHandler OnReadingValueChanged;

        public double Correction = 0;//補正
        public bool IsWorking;

        public void Start()
        {
            Essentials.Compass.ReadingChanged += (s, e) =>
            {
                this.HeadingNorth = (e.Reading.HeadingMagneticNorth + Correction) % 360;//0～360の間にする
                this.HeadingNorthForRotate = -this.HeadingNorth;

                if (this._geolocation != null)
                {
                    this.DirectionToTargetForRotate = this.HeadingNorthForRotate + this._geolocation.DirectionToTarget;
                }

                OnReadingValueChanged?.Invoke(this, EventArgs.Empty);
            };

            if (!Essentials.Compass.IsMonitoring)
                Essentials.Compass.Start(Essentials.SensorSpeed.UI, applyLowPassFilter: true);

            IsWorking = Essentials.Compass.IsMonitoring;
        }

        public void Stop()
        {
            Essentials.Compass.Stop();

            IsWorking = Essentials.Compass.IsMonitoring;
        }

        private Geolocation _geolocation;

        public void SetGeolocation(Geolocation geolocation)
        {
            this._geolocation = geolocation;
        }

        public void ClearGeolocation()
        {
            this._geolocation = null;
        }
    }
}