using Google.OpenLocationCode;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Essentials = Xamarin.Essentials;

namespace TakeMeThereXamarinForms
{
    static class Utility
    {

        private static double CalculateTargetDirection(Essentials.Location location1, Essentials.Location location2)
        {
            if (location1 == null || location2 == null)
                return double.NaN;

            var lat1 = location1.Latitude;
            var lon1 = location1.Longitude;
            var lat2 = location2.Latitude;
            var lon2 = location2.Longitude;


            double y = Math.Cos(lon2 * Math.PI / 180) * Math.Sin(lat2 * Math.PI / 180 - lat1 * Math.PI / 180);
            double x = Math.Cos(lon1 * Math.PI / 180) * Math.Sin(lon2 * Math.PI / 180) - Math.Sin(lon1 * Math.PI / 180) * Math.Cos(lon2 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180 - lat1 * Math.PI / 180);

            double dirE0 = 18 * Math.Atan2(y, x) / Math.PI;

            if (dirE0 < 0)
            {
                dirE0 = dirE0 + 360;//0~360に保つため
            }

            var dirN0 = (dirE0 + 90) % 360;

            return dirN0;
        }

        public static double CalculateDistance(Essentials.Location location1, Essentials.Location location2)
        {
            return Essentials.Location.CalculateDistance(location1, location2, Essentials.DistanceUnits.Kilometers);
        }

        public static Essentials.Location GetLocationFromLocalCode(string localCodeWithPlaceName, Essentials.Location baseLocation)
        {
            //目的地の位置情報を計算
            OpenLocationCode recoveredOlc;

            //Open Location Codeで経緯度に変換
            //ローカルコード
            var localCode = Regex.Match(localCodeWithPlaceName, "^[23456789CFGHJMPQRVWX+]+").Value;
            var olc = new OpenLocationCode(localCode);

            recoveredOlc = olc.Recover(baseLocation.Latitude, baseLocation.Longitude);

            var decoded = recoveredOlc.Decode();

            return new Essentials.Location(decoded.CenterLatitude, decoded.CenterLongitude);
        }

    }
}
