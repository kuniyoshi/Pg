#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pg.Puzzle.Response
{
    public class SlidingGems
    {
        public enum EventType
        {
            Take,
            NewGem,
        }

        public IEnumerable<EventType> EventTypes { get; }

        public IEnumerable<SlidingGem> Items { get; }
        public IEnumerable<NewGem> NewGems { get; }

        SlidingGems(IEnumerable<SlidingGem> slidingGems,
                    IEnumerable<NewGem> newGems,
                    IEnumerable<EventType> eventTypes)
        {
            Items = slidingGems.ToArray();
            NewGems = newGems.ToArray();
            EventTypes = eventTypes.ToArray();
        }

        public string DebugDescribeHistory()
        {
            var builder = new StringBuilder();
            var x = Items.GetEnumerator();
            var y = NewGems.GetEnumerator();

            foreach (var eventType in EventTypes)
            {
                switch (eventType)
                {
                    case EventType.Take:
                        x.MoveNext();
                        builder.Append($"{eventType}: {x.Current}");
                        break;

                    case EventType.NewGem:
                        y.MoveNext();
                        builder.Append($"{eventType}: {y.Current}");
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            x.Dispose();
            y.Dispose();

            return builder.ToString();
        }

        public override string ToString()
        {
            return $"{nameof(SlidingGem)}{{"
                   + $"{nameof(Items)}: [{string.Join(", ", Items.Select(item => item.ToString()))}]"
                   + $", {nameof(NewGems)}: [{string.Join(", ", NewGems.Select(item => item.ToString()))}]"
                   + $", {nameof(EventTypes)}: [{string.Join(", ", EventTypes.Select(item => item.ToString()))}]"
                   + "}";
        }

        public readonly struct NewGem
        {
            public Coordinate Coordinate { get; }
            public GemColorType GemColorType { get; }

            public NewGem(Coordinate coordinate, GemColorType gemColorType)
            {
                Coordinate = coordinate;
                GemColorType = gemColorType;
            }

            public override string ToString()
            {
                return $"{nameof(NewGem)}{{"
                       + $"{nameof(Coordinate)}: {Coordinate}"
                       + $", {nameof(GemColorType)}: {GemColorType}"
                       + "}";
            }
        }

        public readonly struct SlidingGem
        {
            public Coordinate From { get; }

            public Coordinate To { get; }

            public GemColorType GemColorType { get; }

            public SlidingGem(GemColorType gemColorType, Coordinate from, Coordinate to)
            {
                GemColorType = gemColorType;
                From = from;
                To = to;
            }

            public override string ToString()
            {
                return $"{nameof(SlidingGem)}{{"
                       + $"{nameof(GemColorType)}: {GemColorType}"
                       + $", {nameof(From)}: {From}"
                       + $", {nameof(To)}: {To}"
                       + "}";
            }
        }

        internal class Builder
        {
            Queue<EventType> Events { get; }
            Queue<NewGem> NewGems { get; }
            Queue<SlidingGem> SlidingGems { get; }

            internal Builder()
            {
                Events = new Queue<EventType>();
                SlidingGems = new Queue<SlidingGem>();
                NewGems = new Queue<NewGem>();
            }

            internal void AddNewGem(NewGem newGem)
            {
                Events.Enqueue(EventType.NewGem);
                NewGems.Enqueue(newGem);
            }

            internal void AddSlidingGem(SlidingGem slidingGem)
            {
                Events.Enqueue(EventType.Take);
                SlidingGems.Enqueue(slidingGem);
            }

            internal SlidingGems Build()
            {
                return new SlidingGems(SlidingGems, NewGems, Events);
            }
        }
    }
}
