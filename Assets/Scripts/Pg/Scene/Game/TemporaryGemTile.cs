#nullable enable
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Pg.Scene.Game
{
    internal static class TemporaryGemTile
    {
        internal static void SetEvents(UserPlayer userPlayer, Image image, Gem gem, GameObject o, Tile tile)
        {
            image.OnPointerDownAsObservable()
                .Subscribe(data =>
                    {
                        var didStart = userPlayer.StartTransactionIfNotAlready(tile);

                        if (didStart)
                        {
                            gem.SelectFirstTime();
                            gem.MakeWorking();
                        }
                    }
                )
                .AddTo(o);
            image.OnPointerExitAsObservable()
                .Subscribe(data =>
                    {
                        var isSelected = userPlayer.IsSelected(tile);

                        if (!isSelected)
                        {
                            gem.QuitWorking();
                        }
                    }
                )
                .AddTo(o);
            image.OnPointerEnterAsObservable()
                .Subscribe(data =>
                    {
                        var didAdd = userPlayer.TryAddTransaction(tile);

                        if (didAdd)
                        {
                            gem.MakeWorking();
                        }
                    }
                )
                .AddTo(o);
            image.OnPointerUpAsObservable()
                .Subscribe(data => userPlayer.CompleteTransaction())
                .AddTo(o);
        }
    }
}
