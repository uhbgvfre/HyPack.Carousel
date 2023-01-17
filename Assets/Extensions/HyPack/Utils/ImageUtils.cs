using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace HyPack.Utils
{
    public static class ImageUtils
    {
        public static (bool isSucc, Texture2D tx2d) LoadImageAsTexture2D(string path)
        {
            Texture2D result = new Texture2D(2, 2);
            result.filterMode = FilterMode.Point;

            byte[] imgData = File.ReadAllBytes(path);
            bool isSuccess = result.LoadImage(imgData);

            return (isSuccess, result);
        }

        public static (bool isSucc, Sprite sprite) LoadImageAsSprite(string path)
        {
            (bool isLoaded, Texture2D tx) = LoadImageAsTexture2D(path);
            return (isLoaded, Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero));
        }

        public static List<Texture2D> LoadTexture2DsFromFolder(string folderPath, params string[] searchPatterns)
        {
            List<Texture2D> tx2ds = new List<Texture2D>();

            if (!Directory.Exists(folderPath)) return tx2ds;

            if (searchPatterns == null || searchPatterns.Length == 0)
            {
                var defaltSearchPatterns = new string[3] { ".jpg", ".png", ".jpeg" };
                searchPatterns = defaltSearchPatterns;
            }

            IOUtils.ForeachFilesBySuffixCaseInsensitive(folderPath, searchPatterns, file =>
            {
                (bool succ, Texture2D tx) = LoadImageAsTexture2D(file);
                if (succ) tx2ds.Add(tx);
            });

            return tx2ds;
        }

        public static List<Sprite> LoadSpritesFromFolder(string folderPath, params string[] searchPatterns)
        {
            List<Sprite> sprites = new List<Sprite>();

            if (!Directory.Exists(folderPath)) return sprites;

            if (searchPatterns == null || searchPatterns.Length == 0)
            {
                var defaltSearchPatterns = new string[3] { ".jpg", ".png", ".jpeg" };
                searchPatterns = defaltSearchPatterns;
            }

            IOUtils.ForeachFilesBySuffixCaseInsensitive(folderPath, searchPatterns, file =>
            {
                (bool succ, Sprite sprite) = LoadImageAsSprite(file);
                if (succ) sprites.Add(sprite);
            });

            return sprites;
        }
    }
}