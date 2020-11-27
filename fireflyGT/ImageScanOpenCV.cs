// KAutoHelper.ImageScanOpenCV
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Media;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using fireflyGT;
namespace fireflyGT
{
    public class ImageScanOpenCV
    {
        public static Bitmap GetImage(string path)
        {
            return new Bitmap(path);
        }

        public static Bitmap Find(string main, string sub, double percent = 0.9)
        {
            Bitmap mainImg = GetImage(main);
            Bitmap subImg = GetImage(sub);
            return Find(main, sub, percent);
        }

        public static Bitmap Find(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            Image<Bgr, byte> source = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> template = new Image<Bgr, byte>(subBitmap);
            Image<Bgr, byte> imageToShow = source.Copy();
            using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
            {
                result.MinMax(out var _, out var maxValues, out var _, out var maxLocations);
                if (maxValues[0] > percent)
                {
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    imageToShow.Draw(match, new Bgr(System.Drawing.Color.Red), 2);
                }
                else
                {
                    imageToShow = null;
                }
            }
            return imageToShow?.ToBitmap();
        }

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public static Point? FindOutPoint(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            if (subBitmap == null || mainBitmap == null)
            {
                return null;
            }
            if (subBitmap.Width > mainBitmap.Width || subBitmap.Height > mainBitmap.Height)
            {
                return null;
            }
            Image<Bgr, byte> source = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> template = new Image<Bgr, byte>(subBitmap);
            Point? resPoint = null;
            using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
            {
                result.MinMax(out var _, out var maxValues, out var _, out var maxLocations);
                if (maxValues[0] > percent)
                {
                    resPoint = maxLocations[0];
                }
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return resPoint;
        }

        public static List<Point> FindOutPoints(Bitmap mainBitmap, Bitmap subBitmap, double percent = 0.9)
        {
            Image<Bgr, byte> source = new Image<Bgr, byte>(mainBitmap);
            Image<Bgr, byte> template = new Image<Bgr, byte>(subBitmap);
            List<Point> resPoint = new List<Point>();
            while (true)
            {
                using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
                {
                    result.MinMax(out var _, out var maxValues, out var _, out var maxLocations);
                    if (maxValues[0] > percent)
                    {
                        Rectangle match = new Rectangle(maxLocations[0], template.Size);
                        source.Draw(match, new Bgr(System.Drawing.Color.Blue), -1);
                        resPoint.Add(maxLocations[0]);
                        continue;
                    }
                }
                break;
            }
            return resPoint;
        }

        public static List<Point> FindColor(Bitmap mainBitmap, System.Drawing.Color color)
        {
            int searchValue = color.ToArgb();
            List<Point> result = new List<Point>();
            using (Bitmap bmp = mainBitmap)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        if (searchValue.Equals(bmp.GetPixel(x, y).ToArgb()))
                        {
                            result.Add(new Point(x, y));
                        }
                    }
                }
            }
            return result;
        }

        public static List<Point> FindColor(Bitmap mainBitmap, string color)
        {
            System.Drawing.Color Color = (System.Drawing.Color)System.Windows.Media.ColorConverter.ConvertFromString(color);
            return FindColor(mainBitmap, Color);
        }

        public static string RecolizeText(Bitmap img)
        {
            string text = "";
            return Get_Text_From_Image.Get_Text(img);
        }

        public static void SplitImageInFolder(string folderPath)
        {
            DirectoryInfo dir = new DirectoryInfo(folderPath);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo item in files)
            {
                Bitmap Bm_image_sour = new Bitmap(item.FullName);
                Bitmap image_new = Get_Text_From_Image.make_new_image(new Image<Gray, byte>(Bm_image_sour).ToBitmap());
                Bm_image_sour.Dispose();
                int cout_picture = Get_Text_From_Image.split_image(image_new, Path.GetFileNameWithoutExtension(item.Name));
            }
        }

        public static Bitmap ThreshHoldBinary(Bitmap bmp, byte threshold = 190)
        {
            Image<Gray, byte> img = new Image<Gray, byte>(bmp);
            Image<Gray, byte> bmp2 = img.ThresholdBinary(new Gray((int)threshold), new Gray(255.0));
            return bmp2.ToBitmap();
        }

        public static Bitmap NotWhiteToTransparentPixelReplacement(Bitmap bmp)
        {
            bmp = CreateNonIndexedImage(bmp);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    System.Drawing.Color pixel = bmp.GetPixel(x, y);
                    if (pixel.R > 200 && pixel.G > 200 && pixel.B > 200)
                    {
                        bmp.SetPixel(x, y, System.Drawing.Color.Transparent);
                    }
                }
            }
            return bmp;
        }

        public static Bitmap WhiteToBlackPixelReplacement(Bitmap bmp)
        {
            bmp = CreateNonIndexedImage(bmp);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    System.Drawing.Color pixel = bmp.GetPixel(x, y);
                    if (pixel.R > 20 && pixel.G > 230 && pixel.B > 230)
                    {
                        bmp.SetPixel(x, y, System.Drawing.Color.Black);
                    }
                }
            }
            return bmp;
        }

        public static Bitmap TransparentToWhitePixelReplacement(Bitmap bmp)
        {
            bmp = CreateNonIndexedImage(bmp);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    if (bmp.GetPixel(x, y).A >= 1)
                    {
                        bmp.SetPixel(x, y, System.Drawing.Color.White);
                    }
                }
            }
            return bmp;
        }

        public static Bitmap CreateNonIndexedImage(Image src)
        {
            Bitmap newBmp = new Bitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            using (Graphics gfx = Graphics.FromImage(newBmp))
            {
                gfx.DrawImage(src, 0, 0);
            }
            return newBmp;
        }
    }
}
