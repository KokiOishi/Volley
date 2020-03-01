using System;

namespace Volley.Pointing
{
    public static class PointNumberUtils
    {
        public static string ToShortString(this PointNumber number)
        {
            switch (number)
            {
                case PointNumber.Love:
                    return "0";

                case PointNumber.Fifteen:
                    return "15";

                case PointNumber.Thirty:
                    return "30";

                case PointNumber.Fourty:
                    return "40";

                case PointNumber.Advantage:
                    return "Ad";

                case PointNumber.Game:
                    return "**";

                default:
                    throw new ArgumentException("");
            }
        }

        public static bool IsGameSet(this PointNumber number) => number == PointNumber.Game;
    }
}