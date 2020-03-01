using System;
using Volley.Standalone.Matches;
using Volley.Standalone.Games;
using Volley.Standalone.Teams;
using Volley.Standalone.Sets;
using Volley.Standalone.Players;
using Volley.Matches;
using Volley.Sets;
using Volley.Games;
using Volley.Pointing;
using Volley.Players;
using Volley.Scoring;
using Volley.Standalone;

namespace Volley.Cocoa.Ui
{
    public class MainModel
    {
        private PlayerStandalone playerA, playerB;
        private SinglesTeam teamA, teamB;

        private MatchStandalone<SinglesTeam> match;
        Set<SinglesTeam> set => match.CurrentSet;
        Game<SinglesTeam> game => match.CurrentSet.CurrentGame;

        private MatchRuleStandalone matchRule;
        private ViewController viewController;

        public MainModel(ViewController viewController)
        {
            this.viewController = viewController;
        }

        public void OnLoad()
        {
            matchRule = new MatchRuleStandalone(false, true, 2, 7, true);

            playerA = new PlayerStandalone(Guid.NewGuid(), "Nozomi Tojo", Gender.Female, 159.0, 60, Hands.Right, new Color(Vector3.Zero));
            playerB = new PlayerStandalone(Guid.NewGuid(), "Kotori Minami", Gender.Female, 159.0, 54, Hands.Right, new Color(Vector3.Zero));
            teamA = new SinglesTeam(playerA, Guid.NewGuid());
            teamB = new SinglesTeam(playerB, Guid.NewGuid());

            match = new MatchStandalone<SinglesTeam>(teamA, teamB, matchRule, EnumTeams.TeamA,
                new StandardSetCounter(new SimpleFactory<AdvantageSetGameCounter>(() =>
                {
                    return new AdvantageSetGameCounter(new SimpleFactory<FifteenBasedPointCounter>(() =>
                    {
                        return new FifteenBasedPointCounter(true);
                    }), 6, 2);
                }), 5));
            match.StartMatch();
            match.CurrentSet.StartSet();
            match.CurrentSet.CurrentGame.StartGame();
            RefreshPoints();
        }

        public void OnAWin()
        {
            game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamA, ShotKind.Stroke, 1, playerA, HandSide.Fore));
            RefreshPoints();
        }

        private void RefreshPoints()
        {
            if (!(match.Winner is null))
            {
                viewController.PointA = "";
                viewController.PointB = "";
                viewController.GameA = "";
                viewController.GameB = "";
                viewController.SetA = match.Winner == match.TeamA ? "WON" : "LOST";
                viewController.SetB = match.Winner == match.TeamB ? "WON" : "LOST";
            }
            else
            {
                viewController.PointA = game.PointA;
                viewController.PointB = game.PointB;
                viewController.GameA = set.GameCountA.ToString();
                viewController.GameB = set.GameCountB.ToString();
                viewController.SetA = match.SetCountA.ToString();
                viewController.SetB = match.SetCountB.ToString();
            }
        }

        public void OnBWin()
        {
            game.RallyFinished(new PointWinner<SinglesTeam>(PointKinds.Winner, teamB, ShotKind.Stroke, 1, playerB, HandSide.Fore));
            RefreshPoints();
        }
    }
}