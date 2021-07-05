#nullable enable
using System;
using System.Collections.Generic;
using Pg.Puzzle.Util;

namespace Pg.Puzzle
{
    public class TileStatusType
        : Enumeration
    {
        IEnumerable<TileStatusType> Values { get; } = new[]
        {
            Closed,
            Empty,
            Contain,
        };

        TileStatusType(int id, string name)
            : base(id, name)
        {
        }

        public static TileStatusType Closed { get; } = new TileStatusType(id: 1, nameof(Closed));
        public static TileStatusType Contain { get; } = new TileStatusType(id: 3, nameof(Contain));
        public static TileStatusType Empty { get; } = new TileStatusType(id: 2, nameof(Empty));

        public void Switch(Action ifClosed, Action ifEmpty, Action ifContain)
        {
            switch (Id)
            {
                case 1:
                    ifClosed.Invoke();
                    break;

                case 2:
                    ifEmpty.Invoke();
                    break;

                case 3:
                    ifContain.Invoke();
                    break;

                default:
                    throw new ArgumentOutOfRangeException($"{this} is out of range [{string.Join(", ", Values)}]");
            }
        }
    }
}
