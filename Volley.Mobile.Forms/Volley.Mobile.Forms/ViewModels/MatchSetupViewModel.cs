using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using ReactiveUI;
using Volley.Matches;
using Volley.Mobile.Forms.ViewModels.Parameters;
using Volley.Mobile.Forms.Views.Rules;
using Volley.Rules;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Volley.Mobile.Forms.ViewModels
{
    public class MatchSetupViewModel : BindableBase, INavigationAware
    {
        private readonly ReactiveProperty<int> setCountTypeSelectionIndex;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Matches.Rules.MatchRule"/> is doubles.
        /// </summary>
        /// <value><c>true</c> if doubles; otherwise, <c>false</c>.</value>
        public bool IsDoubles { get; set; } = false;

        /// <summary>
        /// Gets how many sets required to win.
        /// </summary>
        /// <value>The sets.</value>
        public int SetsPerMatch { get; set; } = 3;

        /// <summary>
        /// Gets the games per set.
        /// </summary>
        /// <value>The games per set.</value>
        public int GamesPerSet { get; set; } = 6;

        public int GameCountTypeSelectionIndex { get; set; } = 0;

        public int SetCountTypeSelectionIndex { get => setCountTypeSelectionIndex.Value; set => setCountTypeSelectionIndex.Value = value; }

        public ReadOnlyReactiveProperty<bool> IsMatchTieBreakCellsEnabled { get; }

        public int MatchTieBreakLeastWinningPoints { get; set; } = 10;

        public int MatchTieBreakLeastWinningPointDifference { get; set; } = 2;

        public ICommand CommandNext { get; }

        public MatchSetupViewModel(INavigationService navigationService)
        {
            setCountTypeSelectionIndex = new ReactiveProperty<int>(0);
            IsMatchTieBreakCellsEnabled = setCountTypeSelectionIndex.Select(a => a == 1).ToReadOnlyReactiveProperty();
            CommandNext = ReactiveCommand.Create(async () =>
            {
                try
                {
                    MatchSetupParameters n;
                    switch (SetCountTypeSelectionIndex)
                    {
                        case 1:
                            {
                                n = new MatchSetupParameters(IsDoubles, SetsPerMatch, GamesPerSet,
                                        Extensions.CreateParametricFactory<ISetCounter, IParametricFactory<IGameCounter, EnumTeams>>(
                                            a => new MatchTieBreakSetCounter(a, SetsPerMatch * 2 - 1,
                                            MatchTieBreakLeastWinningPoints, MatchTieBreakLeastWinningPointDifference, EnumTeams.TeamA)));
                                break;
                            }
                        default:
                            {
                                n = new MatchSetupParameters(IsDoubles, SetsPerMatch, GamesPerSet,
                                            Extensions.CreateParametricFactory<ISetCounter, IParametricFactory<IGameCounter, EnumTeams>>(
                                                a => new StandardSetCounter(a, SetsPerMatch * 2 - 1, EnumTeams.TeamA)));
                                break;
                            }
                    }
                    var p = new NavigationParameters
                        {
                            { "P", n }
                        };
                    var h = new string[] { nameof(AdvantageSetSetupPage), nameof(TieBreakSetSetupPage) };
                    _ = await navigationService.NavigateAsync(h[GameCountTypeSelectionIndex], p);
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
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }
    }
}