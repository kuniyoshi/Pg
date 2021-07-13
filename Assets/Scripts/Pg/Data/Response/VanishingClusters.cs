#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Assertions;

namespace Pg.Data.Response
{
    public class VanishingClusters
    {
        Dictionary<GemColorType, List<List<Coordinate>>> Data { get; }

        public VanishingClusters(Dictionary<GemColorType, List<List<Coordinate>>> data)
        {
            Data = data;
        }

        public IEnumerable<GemColorType> NewGemColorTypes => Data.Keys;

        public bool Any()
        {
            return Data.Values.Any(listList => listList.Any(list => list.Any()));
        }

        public IEnumerable<IEnumerable<Coordinate>> GetVanishingCoordinatesOf(GemColorType gemColorType)
        {
            Assert.IsTrue(Data.ContainsKey(gemColorType), "Data2.ContainsKey(gemColorType)");
            return Data[gemColorType];
        }

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
