#nullable enable
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    public class UserPlayer
        : MonoBehaviour
    {
        [SerializeField]
        Coordinates? Coordinates;

        Sequence? _sequence;

        void Awake()
        {
            Assert.IsNotNull(Coordinates, "Coordinates != null");
        }

        public void CompleteTransaction()
        {
            Assert.IsNotNull(_sequence, "_sequence != null");
            _sequence = null;
        }

        public bool StartTransactionIfNotAlready(Tile tile)
        {
            if (_sequence != null)
            {
                return false;
            }

            _sequence = new Sequence(tile);

            return true;
        }

        public void TryAddTransaction(Tile tile)
        {
            _sequence?.Swap(tile);
        }

        class Sequence
        {
            Tile? _lastSelection;

            public Sequence(Tile tile)
            {
                _lastSelection = tile;
                Histories = new Queue<Operation>();
            }

            Queue<Operation> Histories { get; }

            public void Swap(Tile tile)
            {
                Assert.IsNotNull(_lastSelection, "MESSAGE");
                var operation = new Operation(_lastSelection!, tile);
                operation.DoSwap();
                Histories.Enqueue(operation);
                _lastSelection = tile;
            }

            class Operation
            {
                public Operation(Tile tileA, Tile tileB)
                {
                    TileA = tileA;
                    TileB = tileB;
                }

                Tile TileA { get; }
                Tile TileB { get; }

                public void DoSwap()
                {
                    var (currentA, currentB) = (TileA.TileStatus, TileB.TileStatus);
                    var (nextA, nextB) = (currentB, currentA);
                    TileA.UpdateStatus(nextA);
                    TileB.UpdateStatus(nextB);
                }
            }
        }
    }
}
