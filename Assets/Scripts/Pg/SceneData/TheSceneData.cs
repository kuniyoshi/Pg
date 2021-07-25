#nullable enable
using UnityEngine;

namespace Pg.SceneData
{
    public static class TheSceneData
    {
        static Impl Instance { get; } = new Impl();

        public static ResultData? GetResultData()
        {
            return Instance.ResultData;
        }

        public static ResultData SetResultData(ResultData resultData)
        {
            return Instance.ResultData = resultData;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Initialize()
        {
            Instance.ResultData = null;
        }

        class Impl
        {
            public ResultData? ResultData { get; internal set; }
        }
    }
}
