using UniRx;
using System.IO;
using UnityEngine;
using HyPack.Utils;
using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;

namespace HyPack.Carousel
{
    public class CarouselManager : MonoBehaviour
    {
        public int currentIndex;
        public List<CarouselResProfile> carouselResProfiles = new List<CarouselResProfile>();
        public ReactiveProperty<CarouselSession> carouselSession = new ReactiveProperty<CarouselSession>();
        public ReactiveProperty<bool> isInited = new ReactiveProperty<bool>();

        private MediaPlayer m_MediaPlayerCache;
        public MediaPlayer mediaPlayer => m_MediaPlayerCache ??= GetComponentInChildren<MediaPlayer>();

        private static string s_CarouselResPath => Application.streamingAssetsPath + "/CarouselRes"; // for AppConfig parameterize refactoring

        private void Start()
        {
            LoadCarouselResProfiles();
        }

        // 從磁碟載入資源快照
        private void LoadCarouselResProfiles()
        {
            carouselResProfiles.Clear();

            IOUtils.ForeachFilesBySuffixCaseInsensitive(s_CarouselResPath, IOUtils.k_ImageAndVideoExtFilter, path =>
            {
                string fName = Path.GetFileNameWithoutExtension(path);
                string fExt = Path.GetExtension(path);
                var cr = new CarouselResProfile(fName, fExt, path);

                carouselResProfiles.Add(cr);
            });

            isInited.Value = true;
        }

        private void FixedUpdate()
        {
            // 當前節終止檢測
            carouselSession.Value?.OnFixedUpdate(Time.fixedDeltaTime);
        }

        // 從頭播放
        public void PlayFromFirstSession()
        {
            currentIndex = 0;
            PlaySession(currentIndex);
        }

        // 播放下一節
        public void PlayNextSession()
        {
            currentIndex = (int)Mathf.Repeat(currentIndex + 1, carouselResProfiles.Count);
            PlaySession(currentIndex);
        }

        // 播放目標節
        public void PlaySession(int idx)
        {
            if (idx >= carouselResProfiles.Count)
            {
                carouselSession.Value = null;
                return;
            }

            carouselSession.Value = new CarouselSession(this, carouselResProfiles[idx]);
        }

        // 停止當前節
        public void StopSession()
        {
            carouselSession.Value?.Close(true);
            carouselSession.Value = null;
        }
    }
}
