#nullable enable
using System;
using System.Collections.Generic;
using Pg.Etc.Util;

namespace Pg.Puzzle
{
    public class TileStatusType
        : Enumeration
    {
        static Dictionary<InternalType, TileStatusType> ValueOf { get; } = new Dictionary<InternalType, TileStatusType>
        {
            [InternalType.Closed] = new TileStatusType(InternalType.Closed),
            [InternalType.Empty] = new TileStatusType(InternalType.Empty),
            [InternalType.Contain] = new TileStatusType(InternalType.Contain),
        };

        TileStatusType(InternalType internalType)
            : base((int) internalType, internalType.ToString())
        {
        }

        public static TileStatusType Closed => ValueOf[InternalType.Closed];
        public static TileStatusType Contain => ValueOf[InternalType.Contain];
        public static TileStatusType Empty => ValueOf[InternalType.Empty];
        public string? Sigil => GetSigil();

        public void Switch(Action ifClosed, Action ifEmpty, Action ifContain)
        {
            switch ((InternalType) Id)
            {
                case InternalType.Closed:
                    ifClosed.Invoke();
                    break;

                case InternalType.Contain:
                    ifContain.Invoke();
                    break;

                case InternalType.Empty:
                    ifEmpty.Invoke();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        string? GetSigil()
        {
            return (InternalType) Id switch
            {
                InternalType.Closed => "X",
                InternalType.Empty => " ",
                InternalType.Contain => null,
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        enum InternalType
        {
            Closed = 1,
            Empty = 2,
            Contain = 3,
        }
    }
}
