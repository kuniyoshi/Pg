#nullable enable
using Pg.Data.Response;
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

        public void SetScore(Data.Response.Score newValue)
        {
            _score = newValue;
            UpdateView();
        }

        internal void AddScore(AcquisitionScore acquisitionScore)
        {
            _score = _score.Add(acquisitionScore);
            UpdateView();
        }

        internal void Initialize(Data.Response.Score score)
        {
            _score = score;
            UpdateView();
        }

        void UpdateView()
        {
            Text!.text = _score.GetText();
        }
    }
}
