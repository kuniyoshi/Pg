#nullable enable
using Pg.SceneData.ResultItem;

namespace Pg.SceneData
{
    public class ResultData
    {
        public static ResultData Create(GameResult gameResult,
                                        int totalTurn,
                                        int turnLimit,
                                        int totalChain,
                                        int totalVanishedGem,
                                        int totalScore,
                                        int targetScore)
        {
            return new ResultData(
                gameResult,
                TotalTurn.Get(totalTurn),
                TurnLimit.Get(turnLimit),
                TotalChain.Get(totalChain),
                TotalVanishedGem.Get(totalVanishedGem),
                TotalScore.Get(totalScore),
                TargetScore.Get(targetScore)
            );
        }

        public GameResult GameResult { get; }
        public TargetScore TargetScore { get; }
        public TotalChain TotalChain { get; }
        public TotalScore TotalScore { get; }
        public TotalTurn TotalTurn { get; }
        public TotalVanishedGem TotalVanishedGem { get; }
        public TurnLimit TurnLimit { get; }

        ResultData(GameResult gameResult,
                   TotalTurn totalTurn,
                   TurnLimit turnLimit,
                   TotalChain totalChain,
                   TotalVanishedGem totalVanishedGem,
                   TotalScore totalScore,
                   TargetScore targetScore)
        {
            GameResult = gameResult;
            TotalTurn = totalTurn;
            TurnLimit = turnLimit;
            TotalChain = totalChain;
            TotalVanishedGem = totalVanishedGem;
            TotalScore = totalScore;
            TargetScore = targetScore;
        }
    }
}
