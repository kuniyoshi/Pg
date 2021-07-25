#nullable enable
using UnityEngine;

namespace Pg.Scene.Result.Data
{
    [CreateAssetMenu(menuName = "Pg/Result/DebugResultData")]
    internal class DebugResultData
        : ScriptableObject
    {
        internal static DebugResultData LoadInstance()
        {
            const string debugResultDebugResultData = "Debug/Result/DebugResultData";
            return Resources.Load<DebugResultData>(debugResultDebugResultData);
        }

        [SerializeField]
        internal bool DidSucceed;

        [SerializeField]
        internal int TotalTurn;

        [SerializeField]
        internal int TurnLimit;

        [SerializeField]
        internal int TotalChain;

        [SerializeField]
        internal int TotalVanishedGem;

        [SerializeField]
        internal int TotalScore;

        [SerializeField]
        internal int TargetScore;
    }
}
