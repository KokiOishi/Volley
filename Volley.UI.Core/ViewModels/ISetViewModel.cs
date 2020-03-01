using System;
using Volley.Team;

namespace Volley.UI.ViewModels
{
    public interface ISetViewModel
    {
        ITeamInMatch TeamA { get; }
        ITeamInMatch TeamB { get; }
        int GameCountA { get; }
        int GameCountB { get; }

    }
}
