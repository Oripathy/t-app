using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Clicker.Presentation
{
    public class Coin : MonoBehaviour, IPoolable<IMemoryPool>
    {
        private Tween _tween;
        
        public void OnDespawned()
        {
            _tween?.Kill();
        }

        public void OnSpawned(IMemoryPool p1)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _tween = transform.DORotate(new Vector3(0, 0, -360), 2f)
                .SetRelative()
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Incremental);
        }

        public UniTask MoveTo(Vector3 position, float duration, CancellationToken token)
        {
            var flipped = Random.value > 0.5f;
            gameObject.SetActive(true);
            return DOTween.Sequence()
                .Append(transform.DOMoveX(position.x, duration)
                    .SetEase(flipped ? Ease.InSine : Ease.OutSine))
                .Join(transform.DOMoveY(position.y, duration)
                    .SetEase(flipped ? Ease.OutSine : Ease.InSine))
                .AppendCallback(() => gameObject.SetActive(false))
                .ToUniTask(cancellationToken: token);
        }

        public class Pool : MonoMemoryPool<Coin>
        {
        }
    }
}