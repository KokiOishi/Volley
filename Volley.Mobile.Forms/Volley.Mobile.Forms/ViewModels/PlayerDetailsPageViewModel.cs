using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveCommand = ReactiveUI.ReactiveCommand;

namespace Volley.Mobile.Forms.ViewModels
{
    public class PlayerDetailsPageViewModel : BindableBase, INavigationAware, IDisposable
    {
        private bool disposedValue = false;
        public ReactiveProperty<string> NameInternal { get; set; }
        public string Title { get; private set; } = "プレイヤーの詳細";

        public ICommand CommandClose { get; }

        public string Name { get => NameInternal.Value; set => NameInternal.Value = value; }
        private IDisposable NameObserver { get; set; }

        private PlayerSetupPlayerViewModel Player { get; set; }

        public PlayerDetailsPageViewModel(INavigationService navigationService)
        {
            NameInternal = new ReactiveProperty<string>();

            NameObserver = NameInternal.Subscribe(a =>
            {
                if (!(Player is null))
                {
                    Player.Name = a;
                }
            });
            CommandClose = ReactiveCommand.Create(async () => _ = await navigationService.GoBackAsync());
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            Player = parameters.GetValue<PlayerSetupPlayerViewModel>("P");
            Name = Player.Name;
            RaisePropertyChanged(nameof(Name));
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //
                }
                NameObserver.Dispose();
                NameInternal.Dispose();
                (NameObserver, NameInternal) = (null, null);

                disposedValue = true;
            }
        }

        ~PlayerDetailsPageViewModel()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}