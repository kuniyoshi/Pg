#nullable enable
using System.Collections.Generic;
using System.Linq;
using Pg.Data.Simulation;
using UnityEngine;

namespace Pg.Scene.Game
{
    internal class GemColor
    {
        GemColorType GemColorType { get; }

        internal GemColor(GemColorType gemColorType)
        {
            GemColorType = gemColorType;
        }

        internal Sprite SelectSprite(IEnumerable<GemColorTypeVsSprite> items)
        {
            return items.First(item => item.First.Convert() == GemColorType).Second;
        }
    }
}
