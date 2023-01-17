using UniRx;
using System;
using UnityEngine;
using HyPack.Utils;
using RenderHeads.Media.AVProVideo;

namespace HyPack.Carousel
{
    // 幻燈片單元節

    [Serializable]
    public class CarouselSession
    {
        public CarouselResProfile carouselResProfile;
        public Sprite m_SpriteRes;
        public bool playNextOnSessionClose = true;
        public bool isResLoadSuccess;

        private CarouselManager m_CarouselManager;
        private double m_Duration;
        private double m_PassedTime;
        private bool m_Inited;

        private static double k_ImageSessionStayTime = 3d; // for AppConfig parameterize refactoring

        public CarouselSession(CarouselManager mgr, CarouselResProfile rp, bool playNextOnSessionClose = true)
        {
            m_CarouselManager = mgr;
            carouselResProfile = rp;
            this.playNextOnSessionClose = playNextOnSessionClose;

            Init();
        }

        private void Init()
        {
            var resPath = carouselResProfile.filePath;

            if (carouselResProfile.resType == CarouselResProfile.Type.Image)
            {
                (isResLoadSuccess, m_SpriteRes) = ImageUtils.LoadImageAsSprite(carouselResProfile.filePath);
                m_Duration = isResLoadSuccess ? k_ImageSessionStayTime : 2;
                m_Inited = true;
            }
            else if (carouselResProfile.resType == CarouselResProfile.Type.Video)
            {
                MediaPath mp = new MediaPath(resPath, MediaPathType.AbsolutePathOrURL);
                isResLoadSuccess = m_CarouselManager.mediaPlayer.OpenMedia(mp);

                if (isResLoadSuccess)
                {
                    Observable.Timer(TimeSpan.FromSeconds(.1f))
                        .Subscribe(_ =>
                        {
                            m_Duration = m_CarouselManager.mediaPlayer.Info.GetDuration();
                            m_Inited = true;
                        });
                }
                else
                {
                    m_Duration = 0;
                    m_Inited = true;
                }
            }
        }

        internal void OnFixedUpdate(float fixedDeltaTime)
        {
            if (!m_Inited) return;

            if (m_Duration <= 0 || m_PassedTime > m_Duration)
            {
                Close();
                return;
            }

            m_PassedTime += fixedDeltaTime;
        }

        public void Close(bool isAbsolutely = false)
        {
            if (m_CarouselManager == null) return;

            m_CarouselManager.carouselSession.Value = null;
            if (m_CarouselManager.mediaPlayer.Control.IsPlaying()) m_CarouselManager.mediaPlayer.CloseMedia();

            if (isAbsolutely) return; // 指定完全關閉就不檢測

            // 若非絕對關閉且有下一節，續播放下一節
            if (playNextOnSessionClose)
            {
                m_CarouselManager.PlayNextSession();
            }
        }
    }
}