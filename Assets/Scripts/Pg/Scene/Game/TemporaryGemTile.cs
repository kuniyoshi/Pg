#nullable enable
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Pg.Scene.Game
{
    internal static class TemporaryGemTile
    {
        internal static void SetEvents(UserPlayer userPlayer, Tile tile, Gem gem, GameObject o)
        {
            gem.Image!.OnPointerDownAsObservable()
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
            gem.Image!.OnPointerExitAsObservable()
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
            gem.Image!.OnPointerEnterAsObservable()
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
            gem.Image!.OnPointerUpAsObservable()
                .Subscribe(data => userPlayer.CompleteTransaction())
                .AddTo(o);
        }
    }
}
