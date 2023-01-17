using HyPack.Utils;

namespace HyPack.Carousel
{
    // 幻燈片資源快照

    [System.Serializable]
    public class CarouselResProfile
    {
        public string resName;
        public string resExt;
        public string filePath;
        public Type resType;

        public CarouselResProfile(string resName, string resExt, string filePath)
        {
            this.resName = resName;
            this.resExt = resExt;
            this.filePath = filePath;
            resType = IOUtils.ContainsImageExt(resExt) ? Type.Image : IOUtils.ContainsVideoExt(resExt) ? Type.Video : default;
        }

        [System.Serializable]
        public enum Type { Unknow, Image, Video };
    }
}