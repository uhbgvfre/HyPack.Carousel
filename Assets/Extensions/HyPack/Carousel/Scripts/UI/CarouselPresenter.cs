using UniRx;
using Zenject;
using UnityEngine;
using UnityEngine.UI;
using RenderHeads.Media.AVProVideo;

namespace HyPack.Carousel
{
    public class CarouselPresenter : MonoBehaviour
    {
        private Image m_ImageCache;
        private Image m_Image => m_ImageCache ??= GetComponentInChildren<Image>();
        private DisplayUGUI m_AVPDisplayCache;
        private DisplayUGUI m_AVPDisplay => m_AVPDisplayCache ??= GetComponentInChildren<DisplayUGUI>();

        private CarouselResProfile m_ResProfile;

        [Inject]
        private CarouselManager m_CarouselManager;

        private void Start()
        {
            m_AVPDisplay.CurrentMediaPlayer = m_CarouselManager.mediaPlayer;

            m_CarouselManager.carouselSession.Subscribe(s =>
            {
                if (s == null)
                {
                    SetImage(false);
                    SetVideoDisplay(false);
                    return;
                }

                if (s.carouselResProfile.resType == CarouselResProfile.Type.Image)
                {
                    print("[type:Image] resPath:" + s.carouselResProfile.filePath);
                    SetImage(true);
                    SetVideoDisplay(false);

                    m_Image.sprite = s.m_SpriteRes;
                }
                else if (s.carouselResProfile.resType == CarouselResProfile.Type.Video)
                {
                    print("[type:Video] resPath:" + s.carouselResProfile.filePath);
                    SetImage(false);
                    SetVideoDisplay(true);
                }
                else
                {
                    // Default unknow
                    print("[Warning] " + "Unknow type");
                    SetImage(false);
                    SetVideoDisplay(false);
                }
            });
        }

        private void SetImage(bool isActive)
        {
            if (isActive) m_Image.gameObject.SetActive(true);
            else
            {
                m_Image.sprite = null;
                m_Image.gameObject.SetActive(false);
            }
        }

        private void SetVideoDisplay(bool isActive)
        {
            m_AVPDisplay.gameObject.SetActive(isActive);
        }
    }
}
