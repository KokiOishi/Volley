using System;
using Volley.Pointing;
using Volley.Team;

namespace Volley.UI.ViewModels
{
    public interface IGameViewModel
    {
        ITeamInMatch TeamA { get; }
        ITeamInMatch TeamB { get; }
        PointNumber PointA { get; }
        PointNumber PointB { get; }


    }
}
