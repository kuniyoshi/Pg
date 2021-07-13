#nullable enable
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

namespace Pg.Scene.Game
{
    public class Score
        : MonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI? Text;

        Data.Response.Score _score;

        void Awake()
        {
            Assert.IsNotNull(Text, "Text != null");
            Text!.text = "0";
        }

        internal void AddScore(Data.Response.Score newValue)
        {
            _score = _score.Add(newValue);
            Text!.text = _score.GetText();
        }

        internal void Initialize(Data.Response.Score score)
        {
            _score = score;
        }
    }
}
