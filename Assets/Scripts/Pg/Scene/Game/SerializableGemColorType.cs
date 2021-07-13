#nullable enable
using System;
using Pg.Data.Simulation;

namespace Pg.Scene.Game
{
    [Serializable]
    public class SerializableGemColorType
    {
        public int Id;

        public GemColorType Convert()
        {
            return GemColorType.Convert(Id);
        }
    }
}
