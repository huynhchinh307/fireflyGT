using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using fireflyGT;
using static System.Net.Mime.MediaTypeNames;

namespace fireflyGT
{
    public class ADBHelper
    {
        private static string LIST_DEVICES_RUNNING = "memuc  listvms --running";
        private static string LIST_MEMUS = "memuc listvms --running";
        private static string CLEAR_LOGCAT = "memuc -i {0} adb logcat -c";
        private static string CHECK_RUNNING = "memuc isvmrunning -i {0}";
        private static string OPEN_APP = "memuc startapp -i {0} {1}";
        private static string CLOSE_APP = "memuc stopapp -i {0} {1}";
        private static string START_MEMU = "memuc start -i {0}";
        private static string STOP_MEMU = "memuc stop -i {0}";
        private static string SORT_WIN = "memuc sortwin";
        private static string TAP_DEVICES = "memuc -i {0} adb shell input tap {1} {2}";

        private static string SWIPE_DEVICES = "memuc -i {0} adb shell input swipe {1} {2} {3} {4} {5}";

        private static string KEY_DEVICES = "memuc -i {0} adb shell input keyevent {1}";

        private static string INPUT_TEXT_DEVICES = "memuc -i {0} adb shell input text \"{1}\"";

        private static string CAPTURE_SCREEN_TO_DEVICES = "memuc -i {0} adb shell screencap -p \"{1}\"";

        private static string PULL_SCREEN_FROM_DEVICES = "memuc -i {0} adb pull \"{1}\"";

        private static string REMOVE_SCREEN_FROM_DEVICES = "memuc -i {0} adb shell rm -f \"{1}\"";

        private static string GET_SCREEN_RESOLUTION = "memuc -i {0} adb shell dumpsys display | Find \"mCurrentDisplayRect\"";

        private const int DEFAULT_SWIPE_DURATION = 100;

        private static string ADB_FOLDER_PATH = @"C:\Program Files (x86)\Microvirt\MEmu";

        private static string ADB_PATH = "";

        public static string SetADBFolderPath(string folderPath)
        {
            ADB_FOLDER_PATH = folderPath;
            ADB_PATH = folderPath + "adb.exe";
            if (File.Exists(ADB_PATH))
            {
                return null;
            }
            return "ADB Path not Exits!!!";
        }
        public static bool OpenApp(string deviceID, string pakage)
        {
            string cmd = string.Format(OPEN_APP, deviceID, pakage);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }

        public static bool SortWin()
        {
            string cmd = string.Format(SORT_WIN);
            Thread t = new Thread(() =>
            {
                string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            })
            {
                IsBackground = true
            };
            t.Start();
            while (t.IsAlive)
            {
                Delay(1);
            }
            return true;
        }
        public static bool OpenMemu(string deviceID)
        {
            string cmd = string.Format(START_MEMU, deviceID);
            Thread t = new Thread(() =>
            {
                string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            })
            {
                IsBackground = true
            };
            t.Start();
            while (t.IsAlive)
            {
                Delay(1);
            }
            return true;
        }
        public static bool StopMemu(string deviceID)
        {
            string cmd = string.Format(STOP_MEMU, deviceID);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }
        public static bool ClearLog(string deviceID)
        {
            string cmd = string.Format(CLEAR_LOGCAT, deviceID);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }
        public static bool CloseApp(string deviceID, string pakage)
        {
            string cmd = string.Format(CLOSE_APP, deviceID, pakage);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }
        public static string ExecuteCMD(string cmdCommand)
        {
            try
            {
                Process cmd = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.WorkingDirectory = ADB_FOLDER_PATH;
                startInfo.FileName = "cmd.exe";
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                startInfo.RedirectStandardInput = true;
                startInfo.RedirectStandardOutput = true;
                cmd.StartInfo = startInfo;
                cmd.Start();
                cmd.StandardInput.WriteLine(cmdCommand);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                return cmd.StandardOutput.ReadToEnd();
            }
            catch
            {
                return null;
            }
        }
        public static string CheckRunning(string deviceID)
        {
            string cmd = string.Format(CHECK_RUNNING, deviceID);
            string input = ExecuteCMD(cmd);
            string pattern = "(?<=" + cmd + ")([^\\n]*\\n+)+";
            MatchCollection matchCollection = Regex.Matches(input, pattern, RegexOptions.Singleline);
            string status = matchCollection[0].Groups[0].Value;
            string[] lines = Regex.Split(status, "\r\n");
            return lines[1];
        }
        public static List<deviceInfo> GetDevices()
        {
            List<deviceInfo> ListDevices = new List<deviceInfo>();
            string input = ExecuteCMD(LIST_DEVICES_RUNNING);
            string pattern = "(?<="+ LIST_DEVICES_RUNNING + ")([^\\n]*\\n+)+";
            MatchCollection matchCollection = Regex.Matches(input, pattern, RegexOptions.Singleline);
            if (matchCollection.Count > 0)
            {
                string AllDevices = matchCollection[0].Groups[0].Value;
                string[] lines = Regex.Split(AllDevices, "\r\n");
                string[] array = lines;
                foreach (string device in array)
                {
                    if (!string.IsNullOrEmpty(device) && device != " ")
                    {
                        try
                        {
                            string[] deviceI = device.Split(',');
                            deviceInfo info = new deviceInfo
                            {
                                index = deviceI[0],
                                name = deviceI[1],
                                status = "Running"
                            };
                            ListDevices.Add(info);
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }
            return ListDevices;
        }

        public static List<deviceInfo> GetMemus()
        {
            List<deviceInfo> ListDevices = new List<deviceInfo>();
            string input = ExecuteCMD(LIST_MEMUS);
            string pattern = "(?<=" + LIST_MEMUS + ")([^\\n]*\\n+)+";
            MatchCollection matchCollection = Regex.Matches(input, pattern, RegexOptions.Singleline);
            if (matchCollection.Count > 0)
            {
                string AllDevices = matchCollection[0].Groups[0].Value;
                string[] lines = Regex.Split(AllDevices, "\r\n");
                string[] array = lines;
                foreach (string device in array)
                {
                    if (!string.IsNullOrEmpty(device) && device != " ")
                    {
                        try
                        {
                            string[] deviceI = device.Split(',');
                            deviceInfo info = new deviceInfo
                            {
                                index = deviceI[0],
                                name = deviceI[1],
                                status = "Running"
                            };
                            ListDevices.Add(info);
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }
            return ListDevices;
        }
        public static string GetDeviceName(string deviceID)
        {
            string name = "";
            string cmd = "";
            string res = ExecuteCMD(cmd);
            return name;
        }

        public static void TapByPercent(string deviceID, double x, double y, int count = 1)
        {
            Point resolution = GetScreenResolution(deviceID);
            int X = (int)(x * ((double)resolution.X * 1.0 / 100.0));
            int Y = (int)(y * ((double)resolution.Y * 1.0 / 100.0));
            string cmdCommand = string.Format(TAP_DEVICES, deviceID, X, Y);
            for (int i = 1; i < count; i++)
            {
                cmdCommand = cmdCommand + " && " + string.Format(TAP_DEVICES, deviceID, x, y);
            }
            string result = ExecuteCMD(cmdCommand);
        }

        public static void Tap(string deviceID, int x, int y, int count = 1)
        {
            string cmdCommand = string.Format(TAP_DEVICES, deviceID, x, y);
            for (int i = 1; i < count; i++)
            {
                cmdCommand = cmdCommand + " && " + string.Format(TAP_DEVICES, deviceID, x, y);
            }
            string result = ExecuteCMD(cmdCommand);
        }

        public static void Key(string deviceID, ADBKeyEvent key)
        {
            string cmdCommand = string.Format(KEY_DEVICES, deviceID, key);
            string result = ExecuteCMD(cmdCommand);
        }

        public static void InputText(string deviceID, string text)
        {
            string cmdCommand = string.Format(INPUT_TEXT_DEVICES, deviceID, text.Replace(" ", "%s").Replace("&", "\\&").Replace("<", "\\<")
                .Replace(">", "\\>")
                .Replace("?", "\\?")
                .Replace(":", "\\:")
                .Replace("{", "\\{")
                .Replace("}", "\\}")
                .Replace("[", "\\[")
                .Replace("]", "\\]")
                .Replace("|", "\\|"));
            string result = ExecuteCMD(cmdCommand);
        }

        public static void SwipeByPercent(string deviceID, double x1, double y1, double x2, double y2, int duration = 100)
        {
            Point resolution = GetScreenResolution(deviceID);
            int X1 = (int)(x1 * ((double)resolution.X * 1.0 / 100.0));
            int Y1 = (int)(y1 * ((double)resolution.Y * 1.0 / 100.0));
            int X2 = (int)(x2 * ((double)resolution.X * 1.0 / 100.0));
            int Y2 = (int)(y2 * ((double)resolution.Y * 1.0 / 100.0));
            string cmdCommand = string.Format(SWIPE_DEVICES, deviceID, X1, Y1, X2, Y2, duration);
            string result = ExecuteCMD(cmdCommand);
        }

        public static void Swipe(string deviceID, int x1, int y1, int x2, int y2, int duration = 100)
        {
            string cmdCommand = string.Format(SWIPE_DEVICES, deviceID, x1, y1, x2, y2, duration);
            string result = ExecuteCMD(cmdCommand);
        }

        public static void LongPress(string deviceID, int x, int y, int duration = 100)
        {
            string cmdCommand = string.Format(SWIPE_DEVICES, deviceID, x, y, x, y, duration);
            string result = ExecuteCMD(cmdCommand);
        }

        public static Point GetScreenResolution(string deviceID)
        {
            GetServer:
            string cmdCommand = string.Format(GET_SCREEN_RESOLUTION, deviceID);
            string result = ExecuteCMD(cmdCommand);
            try
            {
                result = result.Substring(result.IndexOf("- "));
            }
            catch
            {
                ExecuteCMD("adb kill-server");
                ExecuteCMD("adb start-server");
                Delay(10);
                goto GetServer;
            }
            result = result.Substring(result.IndexOf(' '), result.IndexOf(')') - result.IndexOf(' '));
            string[] temp = result.Split(',');
            int x = Convert.ToInt32(temp[0].Trim());
            int y = Convert.ToInt32(temp[1].Trim());
            return new Point(x, y);
        }

        public static Bitmap ScreenShoot(string deviceID = null, bool isDeleteImageAfterCapture = true, string fileName = "screenShoot.png")
        {
            if (string.IsNullOrEmpty(deviceID))
            {
                List<deviceInfo> listDevice = GetDevices();
                if (listDevice == null || listDevice.Count <= 0)
                {
                    return null;
                }
            }
            string screenShotCount = deviceID;
            string nameToSave = Path.GetFileNameWithoutExtension(fileName) + screenShotCount + Path.GetExtension(fileName);
            while (File.Exists(nameToSave))
            {
                try
                {
                    File.Delete(nameToSave);
                }
                catch
                {
                    continue;
                }
                break;
            }
            string cmdCommand = string.Format(CAPTURE_SCREEN_TO_DEVICES, deviceID, "/sdcard/" + nameToSave);
            cmdCommand = cmdCommand + Environment.NewLine + string.Format(PULL_SCREEN_FROM_DEVICES, deviceID, "/sdcard/" + nameToSave);
            cmdCommand = cmdCommand + Environment.NewLine + string.Format(REMOVE_SCREEN_FROM_DEVICES, deviceID, "/sdcard/" + nameToSave) + Environment.NewLine;
            string result = ExecuteCMD(cmdCommand);
            Bitmap cloneBMP;
            using (Bitmap bmp = new Bitmap(ADB_FOLDER_PATH+"/"+ nameToSave))
            {
                cloneBMP = new Bitmap(bmp);
            }
            if (isDeleteImageAfterCapture)
            {
                try
                {
                    File.Delete(nameToSave);
                }
                catch
                {
                }
            }
            return cloneBMP;
        }

        public static void PlanModeON(string deviceID, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                string cmdClearShoppe = "adb -s " + deviceID + " settings put global airplane_mode_on 1";
                cmdClearShoppe = cmdClearShoppe + Environment.NewLine + "adb -s " + deviceID + " am broadcast -a android.intent.action.AIRPLANE_MODE";
                ExecuteCMD(cmdClearShoppe);
            }
        }

        public static void PlanModeOFF(string deviceID, CancellationToken cancellationToken)
        {
            if (!cancellationToken.IsCancellationRequested)
            {
                string cmdClearShoppe = "adb -s " + deviceID + " settings put global airplane_mode_on 0";
                cmdClearShoppe = cmdClearShoppe + Environment.NewLine + "adb -s " + deviceID + " am broadcast -a android.intent.action.AIRPLANE_MODE";
                ExecuteCMD(cmdClearShoppe);
            }
        }

        public static void Delay(double delayTime)
        {
            for (double count = 0.0; count < delayTime; count += 100.0)
            {
                Thread.Sleep(TimeSpan.FromMilliseconds(100.0));
            }
        }

        public static Point? FindImage(string deviceID, string ImagePath, int delayPerCheck = 2000, int count = 5)
        {
            DirectoryInfo dir = new DirectoryInfo(ImagePath);
            FileInfo[] icons = dir.GetFiles();
            do
            {
                Bitmap screen = null;
                int countScreenShot = 3;
                do
                {
                    try
                    {
                        screen = ScreenShoot(deviceID);
                    }
                    catch (Exception)
                    {
                        countScreenShot--;
                        Delay(1000.0);
                        continue;
                    }
                    break;
                }
                while (countScreenShot > 0);
                if (screen == null)
                {
                    return null;
                }
                Point? point = null;
                FileInfo[] array = icons;
                foreach (FileInfo item in array)
                {
                    Bitmap icon = (Bitmap)System.Drawing.Image.FromFile(item.FullName);
                    point = ImageScanOpenCV.FindOutPoint(screen, icon);
                    if (point.HasValue)
                    {
                        break;
                    }
                }
                if (point.HasValue)
                {
                    return point;
                }
                Delay(2000.0);
                count--;
            }
            while (count > 0);
            return null;
        }

        public static bool FindImageAndClick(string deviceID, string ImagePath, int delayPerCheck = 2000, int count = 5)
        {
            DirectoryInfo dir = new DirectoryInfo(ImagePath);
            FileInfo[] icons = dir.GetFiles();
            do
            {
                Bitmap screen = null;
                int countScreenShot = 3;
                do
                {
                    try
                    {
                        screen = ScreenShoot(deviceID);
                    }
                    catch (Exception)
                    {
                        countScreenShot--;
                        Delay(1000.0);
                        continue;
                    }
                    break;
                }
                while (countScreenShot > 0);
                if (screen == null)
                {
                    return false;
                }
                Point? point = null;
                FileInfo[] array = icons;
                foreach (FileInfo item in array)
                {
                    Bitmap icon = (Bitmap)System.Drawing.Image.FromFile(item.FullName);
                    point = ImageScanOpenCV.FindOutPoint(screen, icon);
                    if (point.HasValue)
                    {
                        break;
                    }
                }
                if (point.HasValue)
                {
                    Tap(deviceID, point.Value.X, point.Value.Y);
                    return true;
                }
                Delay(delayPerCheck);
                count--;
            }
            while (count > 0);
            return false;
        }
    }
}
