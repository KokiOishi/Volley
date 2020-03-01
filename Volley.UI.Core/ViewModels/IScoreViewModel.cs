using System.ComponentModel;

namespace Volley.UI.ViewModels
{


    public interface IScoreViewModel : INotifyPropertyChanged
    {
        TeamScores TeamA { get; }
        TeamScores TeamB { get; }


    }
}
