#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Pg.Data.Simulation;
using Pg.Puzzle;
using Pg.Puzzle.Request;
using UniRx;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    internal class UserPlayer
        : MonoBehaviour
    {
        [SerializeField]
        Coordinates? Coordinates;

        Sequence? _sequence;

        Subject<TileOperation[]> TransactionSubject { get; } = new Subject<TileOperation[]>();

        internal IObservable<TileOperation[]> OnTransaction => TransactionSubject;

        void Awake()
        {
            Assert.IsNotNull(Coordinates, "Coordinates != null");
        }

        internal void CompleteTransaction()
        {
            Assert.IsNotNull(_sequence, "_sequence != null");
            Coordinates!.ClearSelections();
            var operations = _sequence!.DumpOperations();
            _sequence = null;

            TransactionSubject.OnNext(operations);
        }

        internal bool IsSelected(Tile tile)
        {
            return _sequence?.IsSelected(tile.Coordinate) ?? false;
        }

        internal bool StartTransactionIfNotAlready(Tile tile)
        {
            if (_sequence != null)
            {
                return false;
            }

            _sequence = new Sequence(tile);

            return true;
        }

        internal bool TryAddTransaction(Tile tile)
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
            Queue<Operation> Histories { get; }
            Tile? _lastSelection;

            internal Sequence(Tile tile)
            {
                _lastSelection = tile;
                Histories = new Queue<Operation>();
            }

            internal bool CanSwap(Tile tile)
            {
                if (_lastSelection == null)
                {
                    return false;
                }

                return GameRule.CanSwap(tile.TileData, _lastSelection!.TileData);
            }

            internal TileOperation[] DumpOperations()
            {
                return Histories.Select(operation => operation.CreateTileOperation())
                    .ToArray();
            }

            internal bool IsSelected(Coordinate coordinate)
            {
                return _lastSelection != null
                       && coordinate.Equals(_lastSelection.Coordinate);
            }

            internal bool Swap(Tile tile)
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
                Tile TileA { get; }
                Tile TileB { get; }

                internal Operation(Tile tileA, Tile tileB)
                {
                    TileA = tileA;
                    TileB = tileB;
                }

                internal TileOperation CreateTileOperation()
                {
                    return new TileOperation(TileA.Coordinate, TileB.Coordinate);
                }

                internal void DoSwap()
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
