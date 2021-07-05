#nullable enable
using System;
using Pg.Puzzle;

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
