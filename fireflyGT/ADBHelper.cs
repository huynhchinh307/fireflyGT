using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using fireflyGT;
using static System.Net.Mime.MediaTypeNames;

namespace fireflyGT
{
    public class ADBHelper
    {
        private static string CLEAR_SESSION_MARO = "memuc -i {0} adb shell pm clear com.ttc.wallet";
        private static string CLEAR_SESSION_UHIVE = "memuc -i {0} adb shell pm clear uhiveapp.uhive.com.uhive";
        private static string CLEAR_SESSION_TATA = "memuc -i {0} adb shell pm clear com.tata.tataufo";
        private static string CLEAR_SESSION_ACORN = "memuc -i {0} adb shell pm clear eco.acorn";
        private static string REBOOT = "memuc reboot -i {0}";
        private static string STOPALL = "memuc stopall";
        private static string CHECK_APP = "memuc -i {0} adb shell pidof com.tata.tataufo";
        private static string CHECK_APP_UHIVE = "memuc -i {0} adb shell pidof uhiveapp.uhive.com.uhive";
        private static string LIST_DEVICES_RUNNING = "memuc  listvms --running";
        private static string LIST_MEMUS = "memuc listvms --running";
        private static string CLEAR_LOGCAT = "memuc -i {0} adb logcat -c";
        private static string CHECK_RUNNING = "memuc isvmrunning -i {0}";
        private static string CHECK_RUNNING_WITH_NAME = "memuc listvms -r";
        private static string OPEN_APP = "memuc startapp -i {0} {1}";
        private static string CLOSE_APP = "memuc stopapp -i {0} {1}";
        private static string START_MEMU = "memuc start -i {0} -t";
        private static string START_MEMU_WITH_TASK = "memuc start -i {0}";
        private static string STOP_MEMU = "memuc stop -i {0} -t";
        private static string SORT_WIN = "memuc sortwin";
        private static string RENAME = "memuc rename -i {0} \"{1}\"";
        private static string TAP_DEVICES = "memuc -i {0} adb shell input tap {1} {2}";

        private static string SWIPE_DEVICES = "memuc -i {0} adb shell input swipe {1} {2} {3} {4} {5}";

        private static string KEY_DEVICES = "memuc -i {0} adb shell input keyevent {1}";

        private static string INPUT_TEXT_DEVICES = "memuc -i {0} input \"{1}\"";

        private static string CAPTURE_SCREEN_TO_DEVICES = "memuc -i {0} adb shell screencap -p \"{1}\"";

        private static string PULL_SCREEN_FROM_DEVICES = "memuc -i {0} adb pull \"{1}\"";

        private static string REMOVE_SCREEN_FROM_DEVICES = "memuc -i {0} adb shell rm \"{1}\"";

        private static string GET_SCREEN_RESOLUTION = "memuc -i {0} adb shell dumpsys display | Find \"mCurrentDisplayRect\"";

        private const int DEFAULT_SWIPE_DURATION = 100;

        private static string ADB_FOLDER_PATH = @"D:\TataLa\Microvirt\MEmu";

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
        public static bool Reboot(string deviceID)
        {
            string cmd = string.Format(REBOOT, deviceID);
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
        public static bool OpenMemuWithTask(string deviceID)
        {
            string cmd = string.Format(START_MEMU_WITH_TASK, deviceID);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }
        public static bool StopAll()
        {
            string input = fireflyGT.ADBHelper.ExecuteCMD(STOPALL);
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
        public static bool ClearSessionMaro(string deviceID)
        {
            string cmd = string.Format(CLEAR_SESSION_MARO, deviceID);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }
        public static bool ClearSessionUhive(string deviceID)
        {
            string cmd = string.Format(CLEAR_SESSION_UHIVE, deviceID);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }
        public static bool ClearSessionTata(string deviceID)
        {
            string cmd = string.Format(CLEAR_SESSION_TATA, deviceID);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }
        public static bool ClearSessionAcorn(string deviceID)
        {
            string cmd = string.Format(CLEAR_SESSION_ACORN, deviceID);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }
        public static bool CloseApp(string deviceID, string pakage)
        {
            string cmd = string.Format(CLOSE_APP, deviceID, pakage);
            string input = fireflyGT.ADBHelper.ExecuteCMD(cmd);
            return true;
        }
        public static bool CheckRunningWithName(string deviceID)
        {
            try
            {
                string cmd = string.Format(CHECK_RUNNING_WITH_NAME, deviceID);
                string input = ExecuteCMD(cmd);
                string pattern = "(?<=" + cmd + ")([^\\n]*\\n+)+";
                MatchCollection matchCollection = Regex.Matches(input, pattern, RegexOptions.Singleline);
                string status = matchCollection[0].Groups[0].Value;
                string[] lines = Regex.Split(status, "\r\n");
                for (int i = 1; i < lines.Length - 3; i++)
                {
                    string[] code = lines[i].Split(',');
                    if (code[1].Equals(deviceID))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool RenameMemu(string deviceID, string name)
        {
            string cmd = string.Format(RENAME, deviceID, name);
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
                try 
                {
                    if (cmdCommand.Split(' ')[1].Equals("start") || cmdCommand.Split(' ')[1].Equals("reboot"))
                    {
                        cmd.WaitForExit();
                    }
                    else
                    {
                        cmd.WaitForExit(5000);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Message: " + ex.Message + " - Details: " + ex.ToString());
                    cmd.WaitForExit(5000);
                }
                StringBuilder output = new StringBuilder();
                while (cmd.StandardOutput.Peek() > 0)
                {
                    if (output == null)
                    {
                        output.AppendFormat(cmd.StandardOutput.ReadLine());
                    }
                    else
                    {
                        output.AppendFormat("{0}\r\n", cmd.StandardOutput.ReadLine());
                    }
                }
                return output.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ExecuteCMD:" + ex.StackTrace);
                return null;
            }
        }
        public static string CheckRunning(string deviceID)
        {
                string cmd = string.Format(CHECK_RUNNING, deviceID);
                string input = ExecuteCMD(cmd);
                if(input == null)
                {
                    return "Not Running";
                }
                string pattern = "(?<=" + cmd + ")([^\\n]*\\n+)+";
                MatchCollection matchCollection = Regex.Matches(input, pattern, RegexOptions.Singleline);
                string status = matchCollection[0].Groups[0].Value;
                string[] lines = Regex.Split(status, "\r\n");
                return lines[1];
        }
        public static string CheckApp(string deviceID)
        {
            try
            {
                Point point = GetScreenResolution(deviceID);
                if (point.X.Equals(0) && point.Y.Equals(0))
                {
                    return "E";
                }
                string cmd = string.Format(CHECK_APP, deviceID);
                string input = ExecuteCMD(cmd);
                string pattern = "(?<=" + cmd + ")([^\\n]*\\n+)+";
                MatchCollection matchCollection = Regex.Matches(input, pattern, RegexOptions.Singleline);
                string status = matchCollection[0].Groups[0].Value;
                string[] lines = Regex.Split(status, "\r\n");
                return lines[3];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Check App error: " + ex.ToString());
                return "E";
            }
        }
        public static string CheckAppUhive(string deviceID)
        {
            try
            {
                string cmd = string.Format(CHECK_APP_UHIVE, deviceID);
                string input = ExecuteCMD(cmd);
                string pattern = "(?<=" + cmd + ")([^\\n]*\\n+)+";
                MatchCollection matchCollection = Regex.Matches(input, pattern, RegexOptions.Singleline);
                string status = matchCollection[0].Groups[0].Value;
                string[] lines = Regex.Split(status, "\r\n");
                return lines[3];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Check App error: " + ex.ToString());
                return "E";
            }
        }
        public static List<deviceInfo> GetDevices()
        {
            List<deviceInfo> ListDevices = new List<deviceInfo>();
            string input = ExecuteCMD(LIST_DEVICES_RUNNING);
            string pattern = "(?<=" + LIST_DEVICES_RUNNING + ")([^\\n]*\\n+)+";
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
                            return ListDevices;
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
                            return ListDevices;
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

        public static bool TapByPercent(string deviceID, double x, double y, int count = 1)
        {
            try
            {
                Point resolution = GetScreenResolution(deviceID);
                if (resolution.X.Equals(0) && resolution.Y.Equals(0))
                {
                    return false;
                }
                int X = (int)(x * ((double)resolution.X * 1.0 / 100.0));
                int Y = (int)(y * ((double)resolution.Y * 1.0 / 100.0));
                string cmdCommand = string.Format(TAP_DEVICES, deviceID, X, Y);
                for (int i = 1; i < count; i++)
                {
                    cmdCommand = cmdCommand + " && " + string.Format(TAP_DEVICES, deviceID, x, y);
                }
                string result = ExecuteCMD(cmdCommand);
                if (result == null)
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
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
            string cmdCommand = string.Format(INPUT_TEXT_DEVICES, deviceID, text);
            string result = ExecuteCMD(cmdCommand);
            Console.WriteLine(result);
        }

        public static bool SwipeByPercent(string deviceID, double x1, double y1, double x2, double y2, int duration = 100)
        {
            try
            {
                Point resolution = GetScreenResolution(deviceID);
                if (resolution.X.Equals(0) && resolution.Y.Equals(0))
                {
                    return false;
                }
                int X1 = (int)(x1 * ((double)resolution.X * 1.0 / 100.0));
                int Y1 = (int)(y1 * ((double)resolution.Y * 1.0 / 100.0));
                int X2 = (int)(x2 * ((double)resolution.X * 1.0 / 100.0));
                int Y2 = (int)(y2 * ((double)resolution.Y * 1.0 / 100.0));
                string cmdCommand = string.Format(SWIPE_DEVICES, deviceID, X1, Y1, X2, Y2, duration);
                string result = ExecuteCMD(cmdCommand);
                if (result == null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Swipe error fix " + ex.Message);
                return false;
            }
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
            try
            {
                string cmdCommand = string.Format(GET_SCREEN_RESOLUTION, deviceID);
                string result = ExecuteCMD(cmdCommand);
                if (result == null)
                {
                    return new Point();
                }
                try
                {
                    result = result.Substring(result.IndexOf("- "));
                }
                catch
                {
                    return new Point();
                }
                result = result.Substring(result.IndexOf(' '), result.IndexOf(')') - result.IndexOf(' '));
                string[] temp = result.Split(',');
                try
                {
                    int x = Convert.ToInt32(temp[0].Trim());
                    int y = Convert.ToInt32(temp[1].Trim());
                    return new Point(x, y);
                }
                catch
                {
                    return new Point();
                }
            }
            catch
            {
                return new Point();
            }
        }

        public static Bitmap ScreenShoot(string deviceID = null, bool isDeleteImageAfterCapture = true, string fileName = "screenShoot.png")
        {
            Bitmap bitmap;
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
            cmdCommand = cmdCommand + "&&" + string.Format(PULL_SCREEN_FROM_DEVICES, deviceID, "/sdcard/" + nameToSave);
            cmdCommand = cmdCommand + "&&" + string.Format(REMOVE_SCREEN_FROM_DEVICES, deviceID, "/sdcard/" + nameToSave) + Environment.NewLine;
            string path = ADB_FOLDER_PATH + "\\" + nameToSave;
            try
            {
                File.Delete(path);
            }
            catch
            {
                Delay(1);
            }
            bool check = File.Exists(path);

            string result = ExecuteCMD(cmdCommand);
            if(result == null)
            {
                bitmap = null;
                return bitmap;
            }
            try
            {
                using (Bitmap bmp = new Bitmap(ADB_FOLDER_PATH + "/" + nameToSave))
                {
                    bitmap = new Bitmap(bmp);
                }
                if (isDeleteImageAfterCapture)
                {
                    try
                    {
                        File.Delete(nameToSave);
                    }
                    catch
                    {
                        Delay(1);
                    }
                }
            }
            catch (Exception ex)
            {
                bitmap = null;
                Console.WriteLine("ScreenShoot Error: " + ex.ToString());
            }
            return bitmap;
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
