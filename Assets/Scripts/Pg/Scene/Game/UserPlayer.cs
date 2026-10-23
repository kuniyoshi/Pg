#nullable enable
using System;
using System.Collections.Generic;
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
        bool _inputEnabled;

        internal bool CanRequestHint => _inputEnabled && _sequence == null;

        internal void SetInputEnabled(bool enabled)
        {
            _inputEnabled = enabled;
        }

        Subject<GemOperation[]> TransactionSubject { get; } = new Subject<GemOperation[]>();

        internal IObservable<GemOperation[]> OnTransaction => TransactionSubject;

        void Awake()
        {
            Assert.IsNotNull(Coordinates, "Coordinates != null");
        }

        internal void CompleteTransaction()
        {
            if (!_inputEnabled || _sequence == null) return;
            Coordinates.ClearSelections();
            var operations = _sequence!.DumpOperations();
            _sequence = null;

            if (operations.Length == 0) return;
            _inputEnabled = false;
            TransactionSubject.OnNext(operations);
        }

        internal bool StartTransactionIfNotAlready(Gem gem)
        {
            if (!_inputEnabled || _sequence != null || gem.IsMoving)
            {
                return false;
            }

            Coordinates.ClearHint();
            _sequence = new Sequence(gem);

            return true;
        }

        internal bool TryAddTransaction(Gem gem)
        {
            if (!_inputEnabled || _sequence is null || gem.IsMoving)
            {
                return false;
            }

            if (_sequence.IsSelected(gem.Coordinate))
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
            Gem FirstSelection { get; }

            internal Sequence(Gem gem)
            {
                FirstSelection = gem;
                Histories = new Queue<GemOperation>();
            }

            internal bool CanSwap(Gem gem)
            {
                return GameRule.CanSwap(FirstSelection.Coordinate, gem.Coordinate);
            }

            internal GemOperation[] DumpOperations()
            {
                return Histories.ToArray();
            }

            internal bool IsSelected(Coordinate coordinate)
            {
                return coordinate.Equals(FirstSelection.Coordinate);
            }

            internal GemOperation Swap(Gem gem)
            {
                var operation = new GemOperation(FirstSelection.Coordinate, gem.Coordinate);
                Histories.Enqueue(operation);
                FirstSelection.Swap(gem);

                return operation;
            }
        }
    }
}
