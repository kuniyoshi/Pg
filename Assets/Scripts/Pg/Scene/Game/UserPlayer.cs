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
        Coordinates Coordinates = null!;

        Sequence? _sequence;

        Subject<GemOperation[]> TransactionSubject { get; } = new Subject<GemOperation[]>();

        internal IObservable<GemOperation[]> OnTransaction => TransactionSubject;

        void Awake()
        {
            Assert.IsNotNull(Coordinates, "Coordinates != null");
        }

        internal void CompleteTransaction()
        {
            Assert.IsNotNull(_sequence, "_sequence != null");
            Coordinates.ClearSelections();
            var operations = _sequence!.DumpOperations();
            _sequence = null;

            TransactionSubject.OnNext(operations);
        }

        internal bool IsSelected(Gem2 gem)
        {
            return _sequence?.IsSelected(gem.GetStableCoordinate()) ?? false;
        }

        internal bool StartTransactionIfNotAlready(Gem2 gem)
        {
            if (_sequence != null)
            {
                return false;
            }

            _sequence = new Sequence(gem);

            return true;
        }

        internal bool TryAddTransaction(Gem2 gem)
        {
            if (_sequence is null)
            {
                return false;
            }

            if (_sequence.IsSelected(gem.GetStableCoordinate()))
            {
                return false;
            }

            if (!_sequence.CanSwap(gem))
            {
                return false;
            }

            Coordinates.Swap(_sequence.Swap(gem));

            return true;
        }

        class Sequence
        {
            Queue<GemOperation> Histories { get; }
            Gem2 FirstSelection { get; }

            internal Sequence(Gem2 gem)
            {
                FirstSelection = gem;
                Histories = new Queue<GemOperation>();
            }

            internal bool CanSwap(Gem2 gem)
            {
                return GameRule.CanSwap(FirstSelection.GetStableCoordinate(), gem.GetStableCoordinate());
            }

            internal GemOperation[] DumpOperations()
            {
                return Histories.ToArray();
            }

            internal bool IsSelected(Coordinate coordinate)
            {
                return coordinate.Equals(FirstSelection.GetStableCoordinate());
            }

            internal GemOperation Swap(Gem2 gem)
            {
                var operation = new GemOperation(FirstSelection.GetStableCoordinate(), gem.GetStableCoordinate());
                Histories.Enqueue(operation);
                FirstSelection.Swap(gem);

                return operation;
            }
        }
    }
}
