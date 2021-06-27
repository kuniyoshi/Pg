#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pg.Puzzle.Response
{
    public class VanishingClusters
    {
        public VanishingClusters(Dictionary<GemColorType, List<List<Coordinate>>> data)
        {
            Data = data;
        }

        Dictionary<GemColorType, List<List<Coordinate>>> Data { get; }

        public override string ToString()
        {
            var list = new List<string>();

            foreach (var keyValuePair in Data)
            {
                var builder = new StringBuilder();
                builder.Append($"{keyValuePair.Key}: [");
                builder.Append(
                    string.Join(
                        ", ",
                        keyValuePair.Value.Select(values => $"[{string.Join(", ", values)}]"
                        )
                    )
                );
                builder.Append("]");
                list.Add(builder.ToString());
            }

            return $"{nameof(VanishingClusters)}{{"
                   + $"{string.Join(", ", list)}"
                   + "}";
        }
    }
}
