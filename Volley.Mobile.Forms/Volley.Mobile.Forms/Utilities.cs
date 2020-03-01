using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Volley.Matches;
using Volley.Team;
using Volley.Standalone;
using Xamarin.Forms;
using Color = Volley.Standalone.Color;
using Volley.Standalone.Matches;
using Volley.Standalone.Players;

namespace Volley.Mobile.Forms
{
    public static class Utilities
    {
        public static (IEnumerable<PlayerModel> A, IEnumerable<PlayerModel> B) GeneratePlayerModels<TTeam>(this MatchStandalone<TTeam> match)
            where TTeam : class, ITeamInMatch
            => match.TeamA.PlayerCount != match.TeamB.PlayerCount ? throw new ArgumentException($"match must have same members in all teams!")
            : (match.TeamA.ToModels().ToArray(), match.TeamB.ToModels().ToArray());

        public static (IEnumerable<Color> A, IEnumerable<Color> B) GenerateColors(int playersPerTeam)
            => (GenerateColorsForPlayers((uint)playersPerTeam, 0), GenerateColorsForPlayers((uint)playersPerTeam, 0.5));

        private static IEnumerable<PlayerModel> ToModels(this ITeamInMatch team)
        {
            foreach (var item in team.AllPlayers.OfType<PlayerStandalone>())
            {
                yield return new PlayerModel(item);
            }
        }

        public static Xamarin.Forms.Color ToFormsColor(this Color color) => new Xamarin.Forms.Color(color.Red, color.Green, color.Blue);

        public static Color ToVolleyColor(this Xamarin.Forms.Color color) => new Color((float)color.R, (float)color.G, (float)color.B);

        public static IEnumerable<Color> GenerateColorsForPlayers(uint count, double thteaShift = 0)
        {
            double dtheta = 0.5 / count;
            for (int i = 0; i < count; i++)
            {
                var baseColor = Xamarin.Forms.Color.FromHsla(dtheta * i + thteaShift, 1, 0.75);
                Debug.WriteLine(baseColor);
                double rb = DeGamma(baseColor.R);
                double gb = DeGamma(baseColor.G);
                double bb = DeGamma(baseColor.B);
                var lb = 0.4120 * rb + 0.5239 * gb + 0.0641 * bb;
                var mb = 0.1667 * rb + 0.7204 * gb + 0.1129 * bb;
                var sb = 0.0241 * rb + 0.0755 * gb + 0.9004 * bb;
                var lpb = HybridLogGamma(lb);
                var mpb = HybridLogGamma(mb);
                var spb = HybridLogGamma(sb);
                double Luminance = 0.5 * (lpb + mpb);
                double ChromaTritanopia = lpb * 1.6137 - mpb * 3.3234 + spb * 1.7097;
                double ChromaProtanopia = lpb * 4.3781 - mpb * 4.2455 - spb * 0.1325;
                Debug.WriteLine($"I:{Luminance}, Ct:{ChromaTritanopia}, Cp:{ChromaProtanopia}");
                const double LumaRef = 0.825;
                var rr = LumaRef / Luminance;
                Luminance = LumaRef;
                ChromaTritanopia *= rr;
                ChromaProtanopia *= rr;
                double Lp = Luminance + 0.00860505 * ChromaTritanopia + 0.111034 * ChromaProtanopia;
                double Mp = Luminance + -0.00860505 * ChromaTritanopia + -0.111034 * ChromaProtanopia;
                double Sp = Luminance + 0.560049 * ChromaTritanopia + -0.320634 * ChromaProtanopia;
                double l = InverseHlg(Lp);
                double m = InverseHlg(Mp);
                double s = InverseHlg(Sp);
                double r = 3.43755689328 * l + -2.50721121251 * m + 0.0696543192281 * s;
                double g = -0.791428686656 * l + 1.98383721987 * m + -0.192408533218 * s;
                double b = -0.02564666291151 * l + -0.09924024864395 * m + 1.12488691156 * s;
                Debug.WriteLine($"R:{r}, G:{g}, B:{b}");
                var newColor = new Color((float)Cap(Gamma(r)), (float)Cap(Gamma(g)), (float)Cap(Gamma(b)));
                Debug.WriteLine(newColor);
                yield return newColor;
            }
        }

        private static double Atanh(double v)
        {
            return (Math.Log((1 + v) / (1 - v)) / 2);
        }

        private static double Cap(double raw)
        {
            return Math.Min(Math.Max(raw, 0), 1);
        }

        private static double Gamma(double u)
            => u < 0.018053968510807
            ? u * 4.5
            : 1.09929682680944 * Math.Pow(u, 0.45) - 0.09929682680944;

        private static double DeGamma(double u)
            => u < 0.0812428582986315
            ? u * 0.07739938080495356037151702786378
            : Math.Pow((u + 0.09929682680944) * 0.90967241568627503691791932283552, 2.2222222222222222222222222222222);

        private const double A = 0.17883277;
        private const double AInverse = 1.0 / A;
        private const double B = 0.28466892;
        private const double C = 0.55991073;
        private const double Sqrt3 = 1.732050807568877293527446341505872366942805253810380628055;
        private const double Sqrt3InverseSquared = 1.0 / 3;
        private const double OneOver12 = 1 / 12.0;

        private static double HybridLogGamma(double u) => u > OneOver12 ? A * Math.Log(12 * u - B) + C : Sqrt3 * Math.Sqrt(u);

        private static double InverseHlg(double u) => u > 0.5 ? OneOver12 * (B + Math.Exp(AInverse * (u - C))) : u * u * Sqrt3InverseSquared;
    }
}