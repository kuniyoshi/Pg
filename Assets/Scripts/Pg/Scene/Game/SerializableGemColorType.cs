#nullable enable
using System;
using Pg.Data.Simulation;
using UnityEngine;

namespace Pg.Scene.Game
{
    [Serializable]
    internal class SerializableGemColorType
    {
        [SerializeField]
        internal int Id;

        internal GemColorType Convert()
        {
            return GemColorType.Convert(Id);
        }
    }
}
