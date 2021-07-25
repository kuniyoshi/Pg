#nullable enable
using Pg.Scene.Result.Animation;
using Pg.SceneData;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Pg.Scene.Result
{
    internal class ResultInformation : MonoBehaviour
    {
        [SerializeField]
        Text? TotalScore;

        [SerializeField]
        Text? TargetScore;

        [SerializeField]
        SuccessAnimation? SuccessAnimation;

        [SerializeField]
        FailureAnimation? FailureAnimation;

        [SerializeField]
        Text? TotalTurn;

        [SerializeField]
        Text? TurnLimit;

        [SerializeField]
        Text? TotalChain;

        [SerializeField]
        Text? TotalVanishedGem;

        Player? _player;

        void Awake()
        {
            Assert.IsNotNull(TotalScore, "TotalScore != null");
            Assert.IsNotNull(TargetScore, "TargetScore != null");
            Assert.IsNotNull(SuccessAnimation, "SuccessAnimation != null");
            Assert.IsNotNull(FailureAnimation, "FailureAnimation != null");
            Assert.IsNotNull(TotalTurn, "TotalTurn != null");
            Assert.IsNotNull(TurnLimit, "TurnLimit != null");
            Assert.IsNotNull(TotalChain, "TotalChain != null");
            Assert.IsNotNull(TotalVanishedGem, "TotalVanishedGem != null");
            _player = Player.Create(
                TotalScore!,
                TargetScore!,
                SuccessAnimation!,
                FailureAnimation!,
                TotalTurn!,
                TurnLimit!,
                TotalChain!,
                TotalVanishedGem!
            );

            TotalScore!.text = "00000000";
            TargetScore!.text = "00000000";
            TotalTurn!.text = "000";
            TurnLimit!.text = "000";
            TotalChain!.text = "000";
            TotalVanishedGem!.text = "0000";
        }

        internal void Play(ResultData resultData)
        {
            _player!.Play(resultData);
        }

        class Player
        {
            internal static Player Create(Text totalScore,
                                          Text targetScore,
                                          SuccessAnimation successAnimation,
                                          FailureAnimation failureAnimation,
                                          Text totalTurn,
                                          Text turnLimit,
                                          Text chainCount,
                                          Text gemCount)
            {
                return new Player(
                    new Animation(totalScore),
                    new Animation(targetScore),
                    successAnimation,
                    failureAnimation,
                    new Animation(totalTurn),
                    new Animation(turnLimit),
                    new Animation(chainCount),
                    new Animation(gemCount)
                );
            }

            FailureAnimation FailureAnimation { get; }
            SuccessAnimation SuccessAnimation { get; }
            Animation TargetScore { get; }
            Animation TotalChain { get; }
            Animation TotalScore { get; }
            Animation TotalTurn { get; }
            Animation TotalVanishedGem { get; }
            Animation TurnLimit { get; }

            Player(Animation totalScore,
                   Animation targetScore,
                   SuccessAnimation successAnimation,
                   FailureAnimation failureAnimation,
                   Animation totalTurn,
                   Animation turnLimit,
                   Animation totalChain,
                   Animation totalVanishedGem)
            {
                TotalScore = totalScore;
                TargetScore = targetScore;
                SuccessAnimation = successAnimation;
                FailureAnimation = failureAnimation;
                TotalTurn = totalTurn;
                TurnLimit = turnLimit;
                TotalChain = totalChain;
                TotalVanishedGem = totalVanishedGem;
            }

            internal void Play(ResultData resultData)
            {
                SuccessAnimation.Play(resultData.GameResult);
                FailureAnimation.Play(resultData.GameResult);

                TotalScore.Play(resultData.TotalScore.GetValue());
                TargetScore.Play(resultData.TargetScore.GetValue());
                TotalTurn.Play(resultData.TotalTurn.GetValue());
                TurnLimit.Play(resultData.TurnLimit.GetValue());
                TotalChain.Play(resultData.TotalChain.GetValue());
                TotalVanishedGem.Play(resultData.TotalVanishedGem.GetValue());
            }
        }

        class Animation
        {
            Text Text { get; }

            internal Animation(Text text)
            {
                Text = text;
            }

            internal void Play(int value)
            {
                Text.text = value.ToString();
            }
        }
    }
}
