#nullable enable
namespace Pg.Rule
{
    public struct JudgeResult
    {
        public static JudgeResult Continuation { get; } = new JudgeResult(Impl.Continuation);
        public static JudgeResult Failure { get; } = new JudgeResult(Impl.Failure);
        public static JudgeResult Succeed { get; } = new JudgeResult(Impl.Succeed);
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

        enum Impl
        {
            Continuation,
            Failure,
            Succeed,
        }
    }
}
