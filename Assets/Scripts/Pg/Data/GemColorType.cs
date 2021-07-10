#nullable enable
using System;
using System.Collections.Generic;
using Pg.Data.Util;

namespace Pg.Data
{
    public class GemColorType
        : Enumeration
    {
        static GemColorType[]? _valuesCache;

        static Dictionary<InternalType, GemColorType> ValueOf { get; } = new Dictionary<InternalType, GemColorType>
        {
            [InternalType.Green] = new GemColorType(InternalType.Green),
            [InternalType.Red] = new GemColorType(InternalType.Red),
            [InternalType.Purple] = new GemColorType(InternalType.Purple),
            [InternalType.Blue] = new GemColorType(InternalType.Blue),
            [InternalType.Yellow] = new GemColorType(InternalType.Yellow),
            [InternalType.Orange] = new GemColorType(InternalType.Orange),
            [InternalType.Rainbow] = new GemColorType(InternalType.Rainbow),
        };

        public static GemColorType Convert(int id)
        {
            var internalType = (InternalType) id;
            return ValueOf[internalType];
        }

        static IEnumerable<GemColorType> GetValues()
        {
            return _valuesCache ??= new[]
            {
                Green,
                Red,
                Purple,
                Blue,
                Yellow,
                Orange,
                Rainbow,
            };
        }

        GemColorType(InternalType baseValue)
            : base((int) baseValue, baseValue.ToString())
        {
        }

        public static GemColorType Blue => ValueOf[InternalType.Blue];
        public static GemColorType Green => ValueOf[InternalType.Green];
        public static GemColorType Orange => ValueOf[InternalType.Orange];
        public static GemColorType Purple => ValueOf[InternalType.Purple];
        public static GemColorType Rainbow => ValueOf[InternalType.Rainbow];

        public static GemColorType Red => ValueOf[InternalType.Red];

        public string Sigil => GetSigil();

        public static IEnumerable<GemColorType> Values => GetValues();
        public static GemColorType Yellow => ValueOf[InternalType.Yellow];

        string GetSigil()
        {
            return (InternalType) Id switch
            {
                InternalType.Green => "g",
                InternalType.Red => "r",
                InternalType.Purple => "p",
                InternalType.Blue => "b",
                InternalType.Yellow => "y",
                InternalType.Orange => "o",
                InternalType.Rainbow => "a",
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        enum InternalType
        {
            Green = 1,
            Red = 2,
            Purple = 3,
            Blue = 4,
            Yellow = 5,
            Orange = 6,
            Rainbow = 7,
        }
    }
}
