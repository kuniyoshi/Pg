#nullable enable
using System.Collections.Generic;
using System.Linq;
using Pg.Puzzle.Util;

namespace Pg.Puzzle
{
    public class GemColorType
        : Enumeration
    {
        public static GemColorType Convert(int id)
        {
            return Values.Single(value => value.Id == id);
        }

        GemColorType(int id, string name)
            : base(id, name)
        {
        }

        public static GemColorType Blue { get; } = new GemColorType(id: 4, nameof(Blue));
        public static GemColorType Green { get; } = new GemColorType(id: 1, nameof(Green));
        public static GemColorType Orange { get; } = new GemColorType(id: 6, nameof(Orange));
        public static GemColorType Purple { get; } = new GemColorType(id: 3, nameof(Purple));
        public static GemColorType Rainbow { get; } = new GemColorType(id: 7, nameof(Rainbow));
        public static GemColorType Red { get; } = new GemColorType(id: 2, nameof(Red));

        public static IEnumerable<GemColorType> Values { get; } = new[]
        {
            Green,
            Red,
            Purple,
            Blue,
            Yellow,
            Orange,
            Rainbow,
        };

        public static GemColorType Yellow { get; } = new GemColorType(id: 5, nameof(Yellow));
    }
}
