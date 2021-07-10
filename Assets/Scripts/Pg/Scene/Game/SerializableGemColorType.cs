#nullable enable
using System;
using Pg.Data;

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
