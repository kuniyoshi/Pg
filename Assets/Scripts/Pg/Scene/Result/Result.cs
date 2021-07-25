#nullable enable
using System.Diagnostics;
using Pg.App;
using Pg.Scene.Result.Data;
using Pg.SceneData;
using Pg.SceneData.ResultItem;
using UnityEngine;
using UnityEngine.Assertions;
using static Pg.SceneData.TheSceneData;

namespace Pg.Scene.Result
{
    internal class Result
        : MonoBehaviour
    {
        [Conditional("DEBUG")]
        static void DebugSetResultData()
        {
            if (!(GetResultData() is null))
            {
                return;
            }

            var debugData = DebugResultData.LoadInstance();
            var data = ResultData.Create(
                debugData.DidSucceed ? GameResult.Success : GameResult.Failure,
                debugData.TotalTurn,
                debugData.TurnLimit,
                debugData.TotalChain,
                debugData.TotalVanishedGem,
                debugData.TotalScore,
                debugData.TargetScore
            );
            SetResultData(data);
        }

        [SerializeField]
        ResultInformation? ResultInformation;

        void Awake()
        {
            Assert.IsNotNull(ResultInformation, "ResultInformation != null");
        }

        void Start()
        {
            DebugSetResultData();
            UnityEngine.Debug.Log(GetResultData());
            ResultInformation!.Play(GetResultData()!);
        }

        internal void MoveToTitle()
        {
            SceneManager.MoveBackToTitleScene();
        }
    }
}
