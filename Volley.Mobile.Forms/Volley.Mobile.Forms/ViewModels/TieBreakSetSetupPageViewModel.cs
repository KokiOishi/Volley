using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Volley.Mobile.Forms.ViewModels.Parameters;
using Volley.Mobile.Forms.Views;
using Volley.Rules;

namespace Volley.Mobile.Forms.ViewModels
{
    public class TieBreakSetSetupPageViewModel : BindableBase, INavigationAware
    {
        public MatchSetupParameters MatchSetupParameters { get; private set; }

        public bool HasDeuceOnNormalGames { get; set; } = true;

        public int TieBreakGameLeastWinningPoint { get; set; } = 7;

        public int TieBreakGameLeastWinningPointDifference { get; set; } = 2;

        public int LeastWinningGameDifference { get; set; } = 2;

        public ICommand CommandNext { get; }

        public TieBreakSetSetupPageViewModel(INavigationService navigationService)
        {
            CommandNext = ReactiveCommand.Create(async () =>
            {
                try
                {
                    var fpc = FifteenBasedPointCounter.CreateFactory(HasDeuceOnNormalGames);
                    var ftpc = TieBrakingPointCounter.CreateFactory(TieBreakGameLeastWinningPoint, TieBreakGameLeastWinningPointDifference);
                    var fgc = TieBreakSetGameCounter.CreateFactory(fpc, ftpc, MatchSetupParameters.GamesPerSet, LeastWinningGameDifference);
                    var sc = MatchSetupParameters.SetCounterFactory.Create(fgc);
                    var n = new RuleSetupParameters(MatchSetupParameters, sc);
                    var p = new NavigationParameters
                    {
                        { "P", n }
                    };
                    _ = await navigationService.NavigateAsync(nameof(PlayerSetupPage), p);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.ToString());
                    throw;
                }
            });
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        => MatchSetupParameters = parameters.GetValue<MatchSetupParameters>("P");
    }
}