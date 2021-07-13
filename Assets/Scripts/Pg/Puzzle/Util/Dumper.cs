#nullable enable
using System.Text;
using Pg.Data;

namespace Pg.Puzzle.Util
{
    public static class Dumper
    {
        public static string Dump(TileStatus[,] tileStatuses)
        {
            var builder = new StringBuilder();

            for (var rowIndex = 0; rowIndex < tileStatuses.GetLength(dimension: 1); ++rowIndex)
            {
                for (var colIndex = 0; colIndex < tileStatuses.GetLength(dimension: 0); ++colIndex)
                {
                    var tileStatus = tileStatuses[colIndex, rowIndex];
                    builder.Append(tileStatus.Sigil);
                }

                builder.Append("\n");
            }

            return builder.ToString();
        }
    }
}
