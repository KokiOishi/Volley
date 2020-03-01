using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Reactive.Bindings;
using ReactiveUI;
using Volley.Players;
using Volley.Team;
using Xamarin.Forms;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Volley.Mobile.Forms.Controls
{
    public partial class PlayerSelectingButton : ContentView
    {
        #region BindableProperty

        public static readonly BindableProperty TextProperty
            = BindableProperty.Create(nameof(Text), typeof(string), typeof(PlayerSelectingButton));

        public static readonly BindableProperty TeamProperty
            = BindableProperty.Create(nameof(Team), typeof(ITeamInMatch), typeof(PlayerSelectingButton));

        public static readonly BindableProperty CommandProperty
            = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(PlayerSelectingButton));

        #endregion BindableProperty

        #region Properties

        public string Text
        {
            get => (string)GetValue(TextProperty); set
            {
                SetValue(TextProperty, value);
                CenterButtonText.Value = value;
            }
        }

        public ITeamInMatch Team
        {
            get => (ITeamInMatch)GetValue(TeamProperty); set
            {
                SetValue(TeamProperty, value);
                TeamInternal.Value = value;
                if (value.PlayerCount > 1)
                {
                    CVMain.IsVisible = true;
                    SinglesButton.IsVisible = false;
                }
                else
                {
                    CVMain.IsVisible = false;
                    SinglesButton.IsVisible = true;
                }
            }
        }

        public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }

        public ObservableCollection<(int Index, ReadOnlyReactiveProperty<string> Name)> Items { get; }

        protected ReactiveProperty<ITeamInMatch> TeamInternal { get; }

        protected ReactiveProperty<string> CenterButtonText { get; }

        #endregion Properties

        #region Commands

        public ICommand CommandOnDown { get; }

        #endregion Commands

        public PlayerSelectingButton()
        {
            try
            {
                Task.Delay(1000).Wait();
                TeamInternal = new ReactiveProperty<ITeamInMatch>();
                CenterButtonText = new ReactiveProperty<string>();
                Items = new ObservableCollection<(int Index, ReadOnlyReactiveProperty<string> Name)>()
                {
                    (-1,TeamInternal.Select(a=>a?[0]?.Name ?? "").ToReadOnlyReactiveProperty()),
                    (0, CenterButtonText.ToReadOnlyReactiveProperty()),
                    (1,TeamInternal.Select(a=>a?[1]?.Name ?? "").ToReadOnlyReactiveProperty()),
                };
                Command = ReactiveCommand.Create<Player>(a =>
                {
                    //Test command
                    Console.WriteLine(a.Name);
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
            BindingContext = this;
            InitializeComponent();
            CVMain.CurrentItem = Items.Where(a => a.Index == 0).FirstOrDefault();
            //CVMain.ItemsSource = Items;
            BindingContext = this;
        }

        private void ExecuteCommand(int player)
        {
            if (Team is null) return;

#pragma warning disable S2583 // Conditionally executed blocks should be reachable
            if (Command?.CanExecute(Team[player]) ?? false)
#pragma warning restore S2583 // Conditionally executed blocks should be reachable
            {
                Command.Execute(Team[player]);
            }
        }

        private void CVMain_CurrentItemChanged(object sender, CurrentItemChangedEventArgs e)
        {
            if (e.CurrentItem is ValueTuple<int, ReadOnlyReactiveProperty<string>> values)
            {
                switch (values.Item1)
                {
                    case -1:
                        ExecuteCommand(0);
                        CVMain.ScrollTo(1);
                        break;

                    case 0:
                        break;

                    case 1:
                        ExecuteCommand(1);
                        CVMain.ScrollTo(1);
                        break;

                    default:
                        break;
                }
            }
        }

        private void SinglesButton_Clicked(object sender, EventArgs e)
        {
            ExecuteCommand(0);
        }

        private void CVMain_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            Console.WriteLine($"Pos:{e.CurrentPosition}");
            switch (e.CurrentPosition)
            {
                case 0:
                    ExecuteCommand(0);
                    CVMain.Position = 1;
                    break;

                case 1:
                    break;

                case 2:
                    ExecuteCommand(1);
                    CVMain.Position = 1;
                    break;

                default:
                    break;
            }
        }
    }
}