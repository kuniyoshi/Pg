#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pg.Puzzle
{
    public class Clusters
    {
        public Clusters(Dictionary<TileStatus, List<List<Coordinate>>> data)
        {
            Data = data;
        }

        Dictionary<TileStatus, List<List<Coordinate>>> Data { get; }

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

            return $"{nameof(Clusters)}{{"
                   + $"{string.Join(", ", list)}"
                   + "}";
        }
    }
}
