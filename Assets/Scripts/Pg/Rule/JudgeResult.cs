#nullable enable
using System;
using Cysharp.Threading.Tasks;

namespace Pg.Rule
{
    public readonly struct JudgeResult
    {
        public static JudgeResult Continuation { get; } = new JudgeResult(Impl.Continuation);
        public static JudgeResult Failure { get; } = new JudgeResult(Impl.Failure);
        public static JudgeResult Success { get; } = new JudgeResult(Impl.Success);
        Impl Value { get; }

        JudgeResult(Impl value)
        {
            Value = value;
        }

        public override bool Equals(object? obj)
        {
            return obj is JudgeResult judgeResult && Equals(judgeResult);
        }

        public bool Equals(JudgeResult? other)
        {
            return Value == other?.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public async UniTask Switch(Func<UniTask> ifContinuation,
                                    Func<UniTask> ifFailure,
                                    Func<UniTask> ifSuccess)
        {
            switch (Value)
            {
                case Impl.Continuation:
                    await ifContinuation.Invoke();
                    return;

                case Impl.Failure:
                    await ifFailure.Invoke();
                    return;

                case Impl.Success:
                    await ifSuccess.Invoke();
                    return;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        enum Impl
        {
            Continuation,
            Failure,
            Success,
        }
    }
}
