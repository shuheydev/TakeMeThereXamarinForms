using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
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

        private double _compassNorth;
        public double CompassNorth
        {
            get => _compassNorth;
            set => SetProperty(ref _compassNorth, value);

        }


        public event EventHandler OnReadingValueChanged;

        public void Start()
        {
            Essentials.Compass.ReadingChanged += (s, e) =>
            {
                this.HeadingNorth = e.Reading.HeadingMagneticNorth;
                this.CompassNorth = 360 - e.Reading.HeadingMagneticNorth;

                OnReadingValueChanged?.Invoke(this, EventArgs.Empty);
            };

            Essentials.Compass.Start(Essentials.SensorSpeed.UI);
        }

        public void Stop()
        {
            Essentials.Compass.Stop();
        }
    }
}
