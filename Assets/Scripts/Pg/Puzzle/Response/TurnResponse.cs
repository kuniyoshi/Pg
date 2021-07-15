#nullable enable
using System.Collections.Generic;
using Pg.Data.Response;
using Pg.Rule;

namespace Pg.Puzzle.Response
{
    public class TurnResponse
    {
        public JudgeResult JudgeResult { get; }
        public Score Score { get; }
        public IEnumerable<SimulationStepResponse> SimulationStepResponses { get; }

        public TurnResponse(IEnumerable<SimulationStepResponse> simulationStepResponses,
                            Score score,
                            JudgeResult judgeResult)
        {
            SimulationStepResponses = simulationStepResponses;
            Score = score;
            JudgeResult = judgeResult;
        }
    }
}
