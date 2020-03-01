using System;
using Volley.Team;

namespace Volley.UI.ViewModels
{
    public interface IMatchViewModel
    {
        ITeamInMatch TeamA{get;}
        ITeamInMatch TeamB{get;}
        int SetCountA { get; }
        int SetCountB { get; }

    }
}
