#nullable enable
using System.Collections.Generic;
using Pg.Puzzle;
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

        public bool IsSelected(Tile tile)
        {
            return _sequence?.IsSelected(tile.Coordinate) ?? false;
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

        public bool TryAddTransaction(Tile tile)
        {
            if (_sequence?.IsSelected(tile.Coordinate) ?? false)
            {
                return false;
            }

            if (!_sequence?.CanSwap(tile) ?? false)
            {
                return false;
            }

            return _sequence?.Swap(tile) ?? false;
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

            public bool CanSwap(Tile tile)
            {
                if (_lastSelection == null)
                {
                    return false;
                }

                return GameController.CanSwap(tile.TileData, _lastSelection!.TileData);
            }

            public bool IsSelected(Coordinate coordinate)
            {
                return _lastSelection != null
                       && coordinate.Equals(_lastSelection.Coordinate);
            }

            public bool Swap(Tile tile)
            {
                Assert.IsNotNull(_lastSelection, "_lastSelection != null");
                var operation = new Operation(_lastSelection!, tile);
                operation.DoSwap();
                Histories.Enqueue(operation);
                _lastSelection = tile;

                return true;
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
