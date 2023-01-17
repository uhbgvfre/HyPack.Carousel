using UnityEngine;
using Zenject;
using UniRx;

namespace HyPack.Carousel.Demo
{
    public class CarouselApiDemo : MonoBehaviour
    {
        public bool playOnInited = true;

        [Inject] private CarouselManager m_CarouselManager;

        private void Start()
        {
            if (playOnInited)
            {
                m_CarouselManager.isInited.Where(x => x).Subscribe(_ => Play());
            }
        }

        [ContextMenu("Play")]
        public void Play()
        {
            m_CarouselManager.PlayFromFirstSession();
        }

        [ContextMenu("Stop")]
        public void Stop()
        {
            m_CarouselManager.StopSession();
        }
    }
}