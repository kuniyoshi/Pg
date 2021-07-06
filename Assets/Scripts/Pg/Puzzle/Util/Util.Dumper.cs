#nullable enable
using System.Collections.Generic;
using System.Text;

namespace Pg.Puzzle.Util
{
    public static class Dumper
    {
        public static string Dump(TileStatus[,] tileStatuses)
        {
            var x = EnumX.Foo;

            switch (x)
            {
            }

            var builder = new StringBuilder();

            for (var rowIndex = 0; rowIndex < tileStatuses.GetLength(dimension: 1); ++rowIndex)
            {
                var items = new List<string>();

                for (var colIndex = 0; colIndex < tileStatuses.GetLength(dimension: 0); ++colIndex)
                {
                    var tileStatus = tileStatuses[colIndex, rowIndex];
                    items.Add($"{tileStatus.TileStatusType}");
                    builder.Append("");
                }
            }

            return builder.ToString();
        }

        readonly struct EnumX
        {
            public static EnumX Foo { get; } = new EnumX(id: 1, nameof(Foo));
            public static EnumX Bar { get; } = new EnumX(id: 1, nameof(Bar));

            public int Id { get; }

            public string Name { get; }

            EnumX(int id, string name)
            {
                (Id, Name) = (id, name);
            }
        }
    }
}
