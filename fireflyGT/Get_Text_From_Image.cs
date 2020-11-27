// KAutoHelper.Get_Text_From_Image
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;

internal class Get_Text_From_Image
{
    private static int saisot = 5;

    private static int red = 217;

    private static int collor_Byte_Start = 160;

    private static string path_langue = "C:\\";

    private static string TempFolder = "image_temp";

    private static string StandarFolder = "image_standand";

    private static List<Color> TemplateColors = new List<Color>
    {
        Color.FromArgb(255, 0, 0, 0)
    };

    public static void information(string Path_Langue)
    {
        path_langue = Path_Langue;
    }

    public static string Get_Text(Bitmap Bm_image_sour)
    {
        string text = "";
        Bitmap image_new = make_new_image(new Image<Gray, byte>(Bm_image_sour).ToBitmap());
        Bm_image_sour.Dispose();
        int cout_picture = split_image(image_new);
        return Get_Text(cout_picture);
    }

    public static Bitmap make_new_image(Bitmap Bm_image_sour)
    {
        int _width = Bm_image_sour.Width;
        int _height = Bm_image_sour.Height;
        Bitmap Bm_image = new Bitmap(_width, _height);
        int collor_Byte_Stop = 230;
        for (int i = collor_Byte_Start; i < collor_Byte_Stop; i++)
        {
            red = i;
            Get_List_Point();
        }
        return Bm_image;
        bool Check_sailenh_Color(Color indexColor, List<Color> templateColor, int sailech)
        {
            bool result = false;
            foreach (Color item in templateColor)
            {
                if (indexColor.R + sailech >= item.R && indexColor.R - sailech <= item.R && indexColor.G + sailech >= item.G && indexColor.G - sailech <= item.G && indexColor.B + sailech >= item.B && indexColor.B - sailech <= item.B)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        void Get_List_Point()
        {
            for (int j = 0; j < _width; j++)
            {
                for (int k = 0; k < _height; k++)
                {
                    Color color = Bm_image_sour.GetPixel(j, k);
                    if (Check_sailenh_Color(color, TemplateColors, saisot))
                    {
                        try
                        {
                            Bm_image.SetPixel(j, k, Color.Black);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
            }
        }
    }

    public static int split_image(Bitmap image, string name = "")
    {
        int cout_picture = 0;
        bool is_start = false;
        int width_start = 0;
        int width_stop = 0;
        int _height_top = 200;
        int _height_bottom = 0;
        int _width = image.Width;
        int _height = image.Height;
        for (int i = 0; i < _width; i++)
        {
            int cout_Black = 0;
            for (int j = 0; j < _height; j++)
            {
                if (image.GetPixel(i, j).Name != "0")
                {
                    cout_Black++;
                    if (_height_top > j)
                    {
                        _height_top = j;
                    }
                    if (_height_bottom < j)
                    {
                        _height_bottom = j;
                    }
                }
            }
            if (cout_Black > 1 && !is_start)
            {
                width_start = i - 1;
                is_start = true;
            }
            if (cout_Black < 1 && is_start)
            {
                width_stop = i + 1;
                is_start = false;
                save_image_splip();
                cout_picture++;
                _height_top = 200;
                _height_bottom = 0;
            }
        }
        return cout_picture;
        void save_image_splip()
        {
            int _width_image_slip = width_stop - width_start;
            int _height_image_split = _height_bottom - _height_top;
            Bitmap image_split = new Bitmap(_width_image_slip, _height_image_split);
            for (int k = 0; k < _width_image_slip; k++)
            {
                for (int l = 0; l < _height_image_split; l++)
                {
                    try
                    {
                        Color color_image_split = image.GetPixel(width_start + k, _height_top + l);
                        image_split.SetPixel(k, l, color_image_split);
                    }
                    catch
                    {
                    }
                }
            }
            string path_folder = TempFolder;
            check_folder_exists(path_folder);
            string output = path_folder + "\\" + name + cout_picture + ".jpg";
            image_split.Save(output);
            image_split.Dispose();
        }
    }

    protected static string Get_Text(int cout_picture)
    {
        string text = "";
        List<string> character = new List<string>
        {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"
        };
        for (int i = 0; i < cout_picture; i++)
        {
            List<double> ketqua = new List<double>();
            for (int k = 0; k < character.Count; k++)
            {
                try
                {
                    string name_text = character[k];
                    double max = 0.0;
                    double currentMax = 0.0;
                    string folderPath = StandarFolder + "\\" + name_text;
                    DirectoryInfo dir = new DirectoryInfo(folderPath);
                    FileInfo[] files = dir.GetFiles();
                    foreach (FileInfo item in files)
                    {
                        string path_image_standate = item.FullName;
                        Bitmap standand = new Bitmap(path_image_standate);
                        string path_image = TempFolder + "\\" + i + ".jpg";
                        Bitmap main = new Bitmap(path_image);
                        currentMax = Image_Equal(main, standand);
                        standand.Dispose();
                        main.Dispose();
                        if (currentMax > max)
                        {
                            max = currentMax;
                        }
                    }
                    ketqua.Add(max);
                }
                catch
                {
                }
            }
            int index_max_trung = 0;
            double _ketqua = 0.0;
            for (int j = 0; j < character.Count; j++)
            {
                if (_ketqua < ketqua[j])
                {
                    _ketqua = ketqua[j];
                    index_max_trung = j;
                }
            }
            text += character[index_max_trung];
        }
        return text;
    }

    public static double Image_Equal(Bitmap main, Bitmap standand)
    {
        double count = 0.0;
        double trung = 0.0;
        Bitmap sub = new Bitmap(main, new Size(standand.Width, standand.Height));
        for (int i = 0; i < standand.Width; i++)
        {
            for (int j = 0; j < standand.Height; j++)
            {
                count += 1.0;
                if (sub.GetPixel(i, j).Equals(standand.GetPixel(i, j)))
                {
                    trung += 1.0;
                }
            }
        }
        return trung / count;
    }

    protected static void check_folder_exists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}
