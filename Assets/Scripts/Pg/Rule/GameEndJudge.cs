#nullable enable
using Pg.Data.Response;

namespace Pg.Rule
{
    public class GameEndJudge
    {
        PassedTurn MaxTurnCount { get; }
        Score TargetScore { get; }

        public GameEndJudge(int maxTurnCount, int targetScore)
        {
            MaxTurnCount = new PassedTurn(maxTurnCount);
            TargetScore = new Score(targetScore);
        }

        public JudgeResult Judge(PassedTurn passedTurn, Score currentScore)
        {
            if (currentScore.IsGreaterThanEqual(TargetScore))
            {
                return JudgeResult.Succeed;
            }

            if (passedTurn.IsGreaterThanEqual(MaxTurnCount))
            {
                return JudgeResult.Failure;
            }

            return JudgeResult.Continuation;
        }
    }
}
