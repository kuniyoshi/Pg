#nullable enable
using System.Collections.Generic;
using Pg.Rule;

namespace Pg.Puzzle.Response
{
    public class TurnResponse
    {
        public JudgeResult JudgeResult { get; }
        public IEnumerable<SimulationStepData> SimulationStepDataItems { get; }

        public TurnResponse(IEnumerable<SimulationStepData> simulationStepDataItems,
                            JudgeResult judgeResult)
        {
            SimulationStepDataItems = simulationStepDataItems;
            JudgeResult = judgeResult;
        }
    }
}
