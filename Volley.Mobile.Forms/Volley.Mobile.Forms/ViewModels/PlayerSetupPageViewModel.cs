using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Volley.Mobile.Forms.ViewModels.Parameters;
using Volley.Mobile.Forms.Views;
using Volley.Rules;
using Volley.Standalone.Matches;
using Volley.Standalone.Players;
using Volley.Standalone.Teams;

namespace Volley.Mobile.Forms.ViewModels
{
    public class PlayerSetupPageViewModel : BindableBase, INavigationAware
    {
        public ObservableCollection<PlayerSetupPlayerViewModel> PlayersA { get; }
        public ObservableCollection<PlayerSetupPlayerViewModel> PlayersB { get; }
        public RuleSetupParameters RuleSetupParameters { get; set; }
        private INavigationService NavigationService { get; }

        public ICommand CommandStart { get; }

        public PlayerSetupPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            PlayersA = new ObservableCollection<PlayerSetupPlayerViewModel>();
            PlayersB = new ObservableCollection<PlayerSetupPlayerViewModel>();
            CommandStart = ReactiveCommand.Create(async () =>
            {
                var ptA = PlayersA.Select(a => new PlayerStandalone(Guid.NewGuid(), a.Name, Players.Gender.Unidentified,
                    170, 65, Players.Hands.Right, a.Color.ToVolleyColor())).ToArray();
                var ptB = PlayersB.Select(a => new PlayerStandalone(Guid.NewGuid(), a.Name, Players.Gender.Unidentified,
                    170, 65, Players.Hands.Right, a.Color.ToVolleyColor())).ToArray();
                var teamA = new GenericTeam(ptA, ptA.First(), Guid.NewGuid(), "Team A");
                var teamB = new GenericTeam(ptB, ptB.First(), Guid.NewGuid(), "Team B");
                RuleSetupParameters rsp = RuleSetupParameters;
                MatchSetupParameters rp = rsp.RuleParameters;
                var rule = new MatchRuleStandalone(rp.IsDoubles,
                    rsp.SetCounter.CurrentGameCounter.CurrentPointCounter is FifteenBasedPointCounter fbpc && fbpc.HasDeuce
                    , rp.SetsPerMatch, rp.GamesPerSet, rsp.SetCounter.CurrentGameCounter is TieBreakSetGameCounter tbsgc);
                var match = new MatchStandalone<GenericTeam>(teamA, teamB, rule, Matches.EnumTeams.TeamA, rsp.SetCounter);
                var n = new MatchStartParameters(match);
                var par = new NavigationParameters() { { "P", n } };
                await navigationService.NavigateAsync(nameof(MainPage), par);
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            /**/
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters?.Any() ?? false)
            {
                RuleSetupParameters = parameters.GetValue<RuleSetupParameters>("P");
                var (cA, cB) = Utilities.GenerateColors(RuleSetupParameters.RuleParameters.IsDoubles ? 2 : 1);
                foreach (var item in (Enumerable.Range(1, cA.Count()).Zip(cA, (a, b) => (num: a, color: b))
                    .Select(a =>
                    {
                        var q = new PlayerSetupPlayerViewModel($"Player{a.num}", a.color.ToFormsColor());
                        q.CommandDetails = ReactiveCommand.Create(async () =>
                        {
                            var par = new NavigationParameters { { "P", q } };
                            _ = await NavigationService.NavigateAsync(nameof(PlayerDetailsPage), par, useModalNavigation: true);
                        });
                        return q;
                    }).ToArray()))
                {
                    PlayersA.Add(item);
                };
                foreach (var item in (Enumerable.Range(cA.Count() + 1, cB.Count()).Zip(cB, (a, b) => (num: a, color: b))
                    .Select(a =>
                    {
                        var q = new PlayerSetupPlayerViewModel($"Player{a.num}", a.color.ToFormsColor());
                        q.CommandDetails = ReactiveCommand.Create(async () =>
                        {
                            var par = new NavigationParameters { { "P", q } };
                            _ = await NavigationService.NavigateAsync(nameof(PlayerDetailsPage), par, useModalNavigation: true);
                        });
                        return q;
                    }).ToArray()))
                {
                    PlayersB.Add(item);
                }
            }
        }
    }
}