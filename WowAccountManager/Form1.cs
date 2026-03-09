using Magic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WowAccountManager
{

    public partial class Form1 : Form
    {
        private const uint pGameState = 0x00B6A9E0;//pointer to a string. "login","charselect","charcreate",
        private const uint pIsLoadingOrConnecting = 0x00B6AA38; //pointer to byte .gamestate. IsLoadingOrConnecting = 0x00B6AA38  14- client ready;10- LOADING WORLD;4-LOADING REALMLIST;3-RETRIEVEING CHARACTER LIST;1-CONECTING;2-SUCCESS ON CONNECTING;
        private const uint pLocale = 0x00B2FE48;//string "enUS" "enGB"
        private const uint pCurrentAccount = 0x00B6AA40; //string 
        private const uint pWorldLoaded = 0x00BEBA40;
        private const uint pSelectedCharacter = 0xAC436C;//byte 0..9 selected character index
        private const string pname = "Wow";
        private const int requiredWidth = 1440;
        private const int requiredHeight = 900;
        private const uint scanStartAddr = 0x02000000;
        private const uint scanEndAddr = 0x05000000;
        private static readonly Random _rng = new Random();

        private bool stopping = false;
        private int gridSelectedIndex = -1;
        private string rootDir, settingsPath, grid1Path;
        private bool switchPending = false;
        private int lastSelectedLine = -1;
        private bool busyLaunching = false;
        private int sendingIt = 0;
        private int threadCount = 0;

        private struct WowWindow
        {
            public int count;
            public IntPtr handle;
            public int PID;
            public string title;
            public bool isVacant;
            public bool isLoggedIn;
            public string accountName;
            public byte isLoadingOrConnecting;
            public bool isWorldLoaded;
            public string serverName;
            public string locale;
            public string baseDir;
            public string realmListPath;
            public BlackMagic wow;
        }
        private List<WowWindow> WowWindows = new List<WowWindow>();

        private struct PointData
        {
            public string tag;
            public Point p1;
            public uint color1;
            public Point p2;
            public uint color2;
            public int color_logic;//1 -AND; 2-OR;
        }
        private readonly PointData pdDisconnected = new PointData //tested
        {
            tag = "pdDisconnected",
            p1 = new Point(808, 440),
            color1 = 14923,
            p2 = new Point(926, 422),
            color2 = 51199,
            color_logic = 1
        };
        private readonly PointData pdRealmSelectWindow = new PointData
        {
            tag = "pdRealmSelectWindow",
            p1 = new Point(1029, 214),
            color1 = 51199
        };
        private readonly PointData pdRealmScreen = new PointData //tested
        {
            tag = "pdRealmScreen",
            p1 = new Point(134, 847),
            color1 = 51199,
            p2 = new Point(1246, 60),
            color2 = 34218,
            color_logic = 2
        };
        private readonly PointData pdLoginScreen = new PointData //tested
        {
            tag = "pdLoginScreen",
            p1 = new Point(783, 445),
            color1 = 51199,
            p2 = new Point(152, 671),
            color2 = 20582,
            color_logic = 1
        };
        public Form1()
        {
            InitializeComponent();
        }
        private string GetStringFromGrid(int rowIndex, string columnName)
        {
            var value = dataGridView1.Rows[rowIndex].Cells[columnName].Value;
            string s = value?.ToString() ?? string.Empty;
            s = s.ToLower();
            return s;
        }
        private bool GetIntFromGrid(int rowIndex, string columnName, out int buffer)
        {
            buffer = 0;
            var value = dataGridView1.Rows[rowIndex].Cells[columnName].Value;
            if (value == null)
            {
                return false;
            }
            bool r = int.TryParse(value.ToString(), out buffer);
            if (r)
            {
                return true;
            }
            return false;
        }

        private void Switch_button_Click(object sender, EventArgs e)
        {
            if (busyLaunching)
            {
                return;
            }
            if (switchPending)
            {
                return;
            }
            if (gridSelectedIndex == -1)
            {
                return;
            }
            WowWindow w1 = default;
            if (IsGridIndexLoggedIn(ref w1))
            {
                NativeMethods.SetForegroundWindow(w1.handle);
                System.Threading.Thread.Sleep(150);
                return;
            }

            WowWindow w = default;
            switchPending = true;
            if (!GetFreeWindowWithRightServer(ref w))
            {
                string r = GetGridRealm();
                if (r == null)
                {
                    return;
                }
                WOW_WriteRealmList(r);
                WOW_StartNew();
            }
            else
            {

            }
            return;
        }
        private bool LoginAndEnterWorld(ref WowWindow w)
        {
            if (WOW_LogIn(w))
            {
                if (CharSelect(w, true))
                {
                    return true;
                }
            }
            return false;
        }
        private bool CharSelect(WowWindow w, bool enterTheWorld)
        {
            if (gridSelectedIndex == -1)
            {
                return false;
            }
            //"position" > 1..10 
            bool r = GetIntFromGrid(gridSelectedIndex, "char_position", out int position);
            if (!r || position < 1 || position > 10)
            {
                Log("ERROR wrong character position. index out of bounds");
                return false;
            }

            position--;
            byte sc = w.wow.ReadByte(pSelectedCharacter);

            Log("Selecting character...");
            int i = 0;
            int maxCharacters = sc;
            while (sc != position)
            {
                i++;
                DoEventsWait(50);
                PostMessageWrap(w.wow.WindowHandle, NativeMethods.WM_KEYDOWN, NativeMethods.VK_DOWN);
                DoEventsWait(10);
                PostMessageWrap(w.wow.WindowHandle, NativeMethods.WM_KEYUP, NativeMethods.VK_DOWN);
                DoEventsWait(100);
                byte sc1 = w.wow.ReadByte(pSelectedCharacter);
                sc = sc1;
                if (i <= 10)//finding out character count
                {
                    if (sc > maxCharacters)
                    {
                        maxCharacters = sc;
                    }
                    else//checking if requested position greater than position count
                    {
                        if (position > maxCharacters)
                        {
                            Log("ERROR wrong character position. index too big");
                            return false;
                        }
                    }
                }
            }
            Log("OK");

            if (enterTheWorld)
            {
                Log("Entering the world...");
                DoEventsWait(100);
                PostMessageWrap(w.wow.WindowHandle, NativeMethods.WM_KEYDOWN, NativeMethods.VK_RETURN);
                PostMessageWrap(w.wow.WindowHandle, NativeMethods.WM_KEYUP, NativeMethods.VK_RETURN);
            }

            return true;
        }
        private bool IsValidPath(string path)
        {
            try
            {
                // This checks for invalid characters and illegal formats
                string fullPath = Path.GetFullPath(path);

                // Optional: Ensure it's not just a relative path (e.g., "temp.txt")
                return Path.IsPathRooted(fullPath);
            }
            catch
            {
                return false;
            }
        }
        private bool IsPathExists(string input)
        {
            if (string.IsNullOrEmpty(input) || input.Trim().Length == 0)
            {
                return false;
            }

            if (IsValidPath(input))
            {
                if (Directory.Exists(input))
                {
                    return true;
                }
                else
                {
                }
            }
            return false;
        }
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (stopping)
                return;
            if (busyLaunching)
                return;
            gridSelectedIndex = GetGridSelectedIndex();

            Process_WOWWindows();

            if (switchPending)
            {
                WowWindow w = default;
                if (GetFreeWindowWithRightServer(ref w))
                {
                    switchPending = false;
                    LoginAndEnterWorld(ref w);
                }
            }
        }
        public uint GetColorAt(IntPtr hwnd, int x, int y)
        {
            IntPtr hdc = NativeMethods.GetDC(hwnd);
            uint pixel = NativeMethods.GetPixel(hdc, x, y);
            NativeMethods.ReleaseDC(hwnd, hdc);
            return pixel;
        }
        private void ShutDown()
        {
            foreach (WowWindow w in WowWindows)
            {
                w.wow.Close();
            }
        }
        private bool IsWOWProcessInTheList(int PID)
        {
            foreach (WowWindow ww in WowWindows)
            {
                if (ww.PID == PID)
                {
                    return true;
                }
            }
            return false;
        }
        private void Update_WOWWindows()
        {
            for (int i = 0; i < WowWindows.Count; i++)
            {
                WowWindow w = WowWindows[i];
                if (!IsProcessRunning(w.PID))
                {
                    WowWindows.RemoveAt(i);
                    continue;
                }
                NativeMethods.GetClientRect(w.handle, out NativeMethods.RECT rect);

                int width = rect.Right - rect.Left;
                int height = rect.Bottom - rect.Top;

                if (width != requiredWidth ||
                            height != requiredHeight)
                {
                    Log($"{w.title} window must be of required size {requiredWidth}x{requiredHeight} to work properly");
                    WowWindows.RemoveAt(i);
                    continue;
                }

                string is2 = w.wow.ReadASCIIString(pGameState, 10);
                if (is2 == "login")
                {
                    w.isVacant = true;
                    w.isLoggedIn = false;
                    w.accountName = default;
                }
                else
                {
                    w.isVacant = false;
                    w.isLoggedIn = true;
                    w.accountName = w.wow.ReadASCIIString(pCurrentAccount, 20);
                    w.accountName = w.accountName.ToLower();
                }
                w.isLoadingOrConnecting = w.wow.ReadByte(pIsLoadingOrConnecting);
                if (w.wow.ReadByte(pWorldLoaded) == 1)
                {
                    w.isWorldLoaded = true;
                }
                else
                {
                    w.isWorldLoaded = false;
                }
                WowWindows[i] = w;
            }
        }
        public bool IsProcessRunning(int pid)
        {
            try
            {
                Process p = Process.GetProcessById(pid);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }
        private void WOW_SendKey(WowWindow w, int key)
        {
            NativeMethods.SetForegroundWindow(w.handle);
            System.Threading.Thread.Sleep(150);
            NativeMethods.PostMessage(w.handle, NativeMethods.WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, NativeMethods.LoadKeyboardLayout(NativeMethods.en_US, NativeMethods.KLF_ACTIVATE));
            System.Threading.Thread.Sleep(100);

            PostMessageWrap(w.handle, NativeMethods.WM_KEYDOWN, key);
            System.Threading.Thread.Sleep(10);
            PostMessageWrap(w.handle, NativeMethods.WM_KEYUP, key);
            System.Threading.Thread.Sleep(100);
        }
        private void PostMessageWrap(IntPtr hWnd, int wMsg, int key)
        {
            NativeMethods.PostMessage(hWnd, wMsg, (IntPtr)key, (IntPtr)0);
        }
        private bool WOW_LogIn(WowWindow w)
        {
            if (gridSelectedIndex == -1)
            {
                return false;
            }
            string login = GetStringFromGrid(gridSelectedIndex, "login");
            string pass = GetStringFromGrid(gridSelectedIndex, "pass");
            if (login != default && pass != default)
            {
                if (!w.isVacant)
                {
                    return false;
                }
                if (IsDisconnectedScreen(w))
                {
                    WOW_SendKey(w, NativeMethods.VK_RETURN);
                }
                if (IsLoginScreen(w))
                {
                    NativeMethods.SetForegroundWindow(w.handle);
                    DoEventsWait(150);
                    Log("Entering login data...");
                    NativeMethods.PostMessage(w.handle, NativeMethods.WM_INPUTLANGCHANGEREQUEST, IntPtr.Zero, NativeMethods.LoadKeyboardLayout(NativeMethods.en_US, NativeMethods.KLF_ACTIVATE));
                    DoEventsWait(100);
                    for (int i = 0; i < login.Length; i++)
                    {
                        NativeMethods.PostMessage(w.handle, NativeMethods.WM_KEYDOWN, (IntPtr)NativeMethods.VkKeyScan(login[i]), (IntPtr)0);
                    }
                    DoEventsWait(50);
                    PostMessageWrap(w.handle, NativeMethods.WM_KEYDOWN, NativeMethods.VK_TAB);
                    DoEventsWait(10);
                    PostMessageWrap(w.handle, NativeMethods.WM_KEYUP, NativeMethods.VK_TAB);
                    DoEventsWait(100);
                    for (int i = 0; i < pass.Length; i++)
                    {
                        NativeMethods.PostMessage(w.handle, NativeMethods.WM_KEYDOWN, (IntPtr)NativeMethods.VkKeyScan(pass[i]), (IntPtr)0);
                    }
                    Log("OK");
                    DoEventsWait(250);

                    Log("Logging in...");

                    PostMessageWrap(w.handle, NativeMethods.WM_KEYDOWN, NativeMethods.VK_RETURN);
                    PostMessageWrap(w.handle, NativeMethods.WM_KEYUP, NativeMethods.VK_RETURN);

                    int timeout = 5000;
                    DateTime wait_start = DateTime.Now;
                    while (!IsRealmScreen(w))
                    {
                        if (DateTime.Now.Subtract(wait_start).TotalMilliseconds > timeout)
                        {
                            Log("FAILED to log in");
                            break;
                        }
                        DoEventsWait(200);
                    }
                    if ((IsRealmScreen(w)))
                    {
                        Log("Successfully logged in");
                        return true;
                    }

                }
            }
            return false;
        }
        private bool IsGridIndexLoggedIn(ref WowWindow w)
        {
            w = default;
            if (gridSelectedIndex == -1)
            {
                return false;
            }
            string login = GetStringFromGrid(gridSelectedIndex, "login");
            if (login == default)
            {
                return false; ;
            }

            foreach (WowWindow ww in WowWindows)
            {
                if (login == ww.accountName)
                {
                    w = ww;
                    return true;
                }
            }
            return false;
        }
        private int GetWOWWindowCount()
        {
            int c = 0;
            foreach (WowWindow ww in WowWindows)
            {
                if (ww.count > c)
                {
                    c = ww.count;
                }
            }
            return c;
        }
        private void Process_WOWWindows()
        {
            if (busyLaunching)
                return;
            Process[] processes = Process.GetProcessesByName(pname);

            int count = GetWOWWindowCount();
            foreach (Process p in processes)
            {
                if (!IsWOWProcessInTheList(p.Id))
                {
                    IntPtr handle = p.MainWindowHandle;
                    if (handle != IntPtr.Zero)
                    {
                        count++;
                        var w = new WowWindow();
                        w.count = count;
                        w.handle = handle;
                        w.PID = p.Id;
                        w.title = $"WOW_{count}";

                        if (w.wow == null)
                        {
                            w.wow = new BlackMagic();
                            w.wow.OpenProcessAndThread(p.Id);
                        }
                        if (w.wow.IsProcessOpen && w.wow.IsThreadOpen)
                        { }
                        else
                        {
                            w.wow.Close();
                            w.wow.OpenProcessAndThread(p.Id);
                        }
                        w.locale = w.wow.ReadASCIIString(pLocale, 10);
                        w.baseDir = Path.GetDirectoryName(p.MainModule.FileName);

                        w.realmListPath = Path.Combine(w.baseDir, @"Data\" + w.locale + @"\realmlist.wtf");

                        w.serverName = FindServerName(ref w);

                        WowWindows.Add(w);
                        NativeMethods.SetWindowText(handle, w.title);
                    }
                }
            }
            Update_WOWWindows();
            FillListBox_WOWWindows();
        }
        private void AppendColorText(string text, Color color, bool newLine = false)
        {
            richTextBox1.SelectionStart = richTextBox1.TextLength;
            richTextBox1.SelectionLength = 0;

            richTextBox1.SelectionColor = color;
            richTextBox1.AppendText(text + (newLine ? Environment.NewLine : ""));
            richTextBox1.SelectionColor = richTextBox1.ForeColor; // Reset to default
        }
        private void FillListBox_WOWWindows()
        {
            SuspendDrawing();

            int scrollPos = richTextBox1.SelectionStart;
            richTextBox1.Clear();

            foreach (WowWindow w in WowWindows)
            {
                string s;
                if (w.accountName == default)
                {
                    s = "{not logged in}";
                }
                else
                {
                    s = w.accountName;
                }
                AppendColorText($"account: {s} ", Color.Red);
                AppendColorText($"window: {w.title} ", Color.Green);
                AppendColorText($"server: {w.serverName}", Color.Blue, true);
            }
            HighlightLine(lastSelectedLine, Color.LightBlue);

            richTextBox1.SelectionStart = scrollPos;

            ResumeDrawing();
        }
        private void SuspendDrawing() => NativeMethods.SendMessage(richTextBox1.Handle, NativeMethods.WM_SETREDRAW, (IntPtr)0, IntPtr.Zero);
        private void ResumeDrawing()
        {
            NativeMethods.SendMessage(richTextBox1.Handle, NativeMethods.WM_SETREDRAW, (IntPtr)1, IntPtr.Zero);
            richTextBox1.Invalidate();
        }
        private string GetGridRealm()
        {
            string gridRealm;
            int rowIndex = dataGridView1.CurrentRow?.Index ?? -1;
            if (rowIndex >= 0 && !dataGridView1.Rows[rowIndex].IsNewRow)
            {
                var value = dataGridView1.Rows[rowIndex].Cells["server"].Value;
                gridRealm = value?.ToString() ?? string.Empty;
                gridRealm = gridRealm.ToLower();
                return gridRealm;
            }

            return default;
        }
        private bool GetFreeWindowWithRightServer(ref WowWindow w)
        {
            string r = GetGridRealm();
            if (r == null)
            {
                return false;
            }
            foreach (WowWindow ww in WowWindows)
            {
                if (ww.isVacant)
                {
                    if (ww.serverName == r)
                    {
                        w = ww;
                        return true;
                    }
                }
            }
            return false;
        }
        private string FindServerName(ref WowWindow w)
        {
            string newRealm = default;
            string prevRealm = default;
            string fileRealm = default;

            if (WOW_ReadNewRealm(ref w, ref newRealm))
            {
            }
            if (WOW_ReadPrevRealm(ref w, ref prevRealm))
            {
                //Debug.WriteLine($"prevRealm \"{prevRealm}\" newRealm \"{newRealm}\" ");
            }
            if (WOW_ReadRealmList(ref w, ref fileRealm))
            {
            }

            if (newRealm != default)
            {
                return newRealm;
            }

            if (newRealm == default && prevRealm == default)
            {
                return "localhost";
            }

            if (newRealm == default && prevRealm != default)
            {
                if (fileRealm == "localhost")
                {
                    return fileRealm;
                }
                else
                {
                    return prevRealm;
                }
            }

            return default;
        }
        private bool WOW_ReadPrevRealm(ref WowWindow w, ref string buffer)
        {

            byte[] var1 = { 0x00, 0x00, 0x00, 0x00, 0x76, 0x65, 0x72, 0x73, 0x69, 0x6F, 0x6E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
            uint var1offset = 52;
            byte[] pat = var1;
            int byteCount = pat.Length;
            string msk = new string('x', byteCount);
            uint found = ScanRegion32(ref w, scanStartAddr, scanEndAddr, pat, msk);
            if (found == 0)
            {
                Log($"cant find WOW_ReadPrevRealm {w.title}");
                buffer = default;
                return false;
            }
            uint p1 = found + var1offset;
            string sn = w.wow.ReadASCIIString(p1, 50);
            string s1 = "localhost";
            if (sn == s1)
            {
                buffer = default;
                return true;
            }
            buffer = sn;
            return true;
        }
        private bool WOW_ReadNewRealm(ref WowWindow w, ref string buffer)
        {
            byte[] var1 = { 0x77, 0x6F, 0x72, 0x6C, 0x64, 0x6F, 0x66, 0x77, 0x61, 0x72, 0x63, 0x72, 0x61, 0x66, 0x74, 0x2E, 0x63, 0x6F, 0x6D, 0x3A, 0x33, 0x37, 0x32, 0x34 };
            uint var1offset = 9 + 17 + 5;
            byte[] pat = var1;
            int byteCount = pat.Length;
            string msk = new string('x', byteCount);
            uint found = ScanRegion32(ref w, scanStartAddr, scanEndAddr, pat, msk);
            if (found == 0)
            {
                Log($"cant find WOW_ReadNewRealm {w.title}");
                buffer = default;
                return false;
            }
            uint p1 = found + var1offset;
            string sn = w.wow.ReadASCIIString(p1, 50);
            string s1 = "Desired method for game timing";
            if (sn == s1)
            {
                buffer = default;
                return true;
            }

            buffer = sn;
            return true;
        }
        private bool WOW_ReadRealmList(ref WowWindow w, ref string buffer)
        {
            string rldata = default;
            buffer = default;
            using (StreamReader reader = new StreamReader(w.realmListPath))
            {
                rldata = reader.ReadLine();
            }
            if (rldata == null)
            {
                Log("Failed ro read realmlist.wtf");
                return false;
            }
            else
            {
                string prefix = "set realmlist ";
                buffer = rldata.Replace(prefix, "");
                return true;
            }
        }
        private void WOW_WriteRealmList(string server)
        {
            string wowpath = textBox2.Text;
            string locale = textBox3.Text;
            if (!IsPathExists(wowpath))
            {
                Log($"Cant find {wowpath}");
                return;
            }
            string localePath = Path.Combine(wowpath, @"Data\" + locale);
            if (!IsPathExists(localePath))
            {
                Log($"Cant find {localePath}");
                return;
            }

            string rl = Path.Combine(localePath, @"realmlist.wtf");


            File.WriteAllText(rl, $"set realmlist {server}");
        }

        private bool IfWOWWindowAvailable(out WowWindow w)
        {
            w = WowWindows.FirstOrDefault();
            if (w.handle == IntPtr.Zero)
            {
                return false;
            }
            return true;
        }
        private bool IsLoginScreen(WowWindow w)
        {
            return IsPointDataMatching(w.handle, pdLoginScreen);
        }
        private bool IsDisconnectedScreen(WowWindow w)
        {
            return IsPointDataMatching(w.handle, pdDisconnected);
        }
        private bool IsPointDataMatching(IntPtr handle, PointData pd)
        {
            if (pd.color_logic != 1 && pd.color_logic != 2)
            {
                return false;
            }
            bool match1 = false;
            bool match2 = false;
            if (pd.p1.X != 0 && pd.p1.Y != 0)
            {
                uint color = GetColorAt(handle, pd.p1.X, pd.p1.Y);
                if (color == pd.color1)
                {
                    match1 = true;
                }
            }
            if (pd.p2.X != 0 && pd.p2.Y != 0)
            {
                uint color = GetColorAt(handle, pd.p2.X, pd.p2.Y);
                if (color == pd.color2)
                {
                    match2 = true;
                }
            }
            else
            {
                if (pd.color_logic == 2) //OR
                {
                    match2 = false;
                }
                else if (pd.color_logic == 1) //AND
                {
                    match2 = true;
                }
            }

            if (pd.color_logic == 2) //OR
            {
                if (match1 || match2)
                {
                    return true;
                }
            }
            else if (pd.color_logic == 1) //AND
            {
                if (match1 && match2)
                {
                    return true;
                }
            }


            return false;
        }
        private bool IsRealmScreen(WowWindow w)
        {
            return IsPointDataMatching(w.handle, pdRealmScreen);
        }
        public byte GetRandomByte(byte[] source)
        {
            return source[_rng.Next(source.Length)];
        }
        public int GetRandomDelay()
        {
            return _rng.Next(500, 3000);
        }
        private void SendAFKKey(WowWindow w, byte keyCode, int delay, int pushtime)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Interlocked.Increment(ref threadCount);
                Thread.Sleep(delay);
                NativeMethods.PostMessage(w.handle, NativeMethods.WM_KEYDOWN, (IntPtr)keyCode, (IntPtr)0);
                if (pushtime == 0)
                {
                    Thread.Sleep(GetRandomDelay());
                }
                else
                {
                    Thread.Sleep(pushtime);
                }
                NativeMethods.PostMessage(w.handle, NativeMethods.WM_KEYUP, (IntPtr)keyCode, (IntPtr)0);
                Interlocked.Decrement(ref threadCount);
            });
        }
        private void SendSpamKeys(WowWindow w)
        {
            if (sendingIt == 1)
            {
                return;
            }
            char[] allChars = textBox1.Text.ToCharArray();
            List<short> keys = new List<short>();
            foreach (char c in allChars)
            {
                short key = NativeMethods.VkKeyScan(c);
                if (key != -1)
                {
                    keys.Add(key);
                }
            }

            Interlocked.Exchange(ref sendingIt, 1);
            ThreadPool.QueueUserWorkItem(delegate
        {
            Interlocked.Increment(ref threadCount);
            foreach (short k in keys)
            {
                Thread.Sleep(150);
                NativeMethods.PostMessage(w.handle, NativeMethods.WM_KEYDOWN, (IntPtr)k, (IntPtr)0);
                Thread.Sleep(50);
                NativeMethods.PostMessage(w.handle, NativeMethods.WM_KEYUP, (IntPtr)k, (IntPtr)0);
            }

            Interlocked.Decrement(ref threadCount);
            Interlocked.Exchange(ref sendingIt, 0);
        });
        }
        private void AntiAFK_tick(WowWindow w)
        {
            byte[] keys = { 0x57, 0x53, 0x41, 0x44 };
            byte[] arrowkeys = { 0x25, 0x27 };
            byte randomKey = GetRandomByte(keys);
            byte randomTurn = GetRandomByte(arrowkeys);

            int jump = 100;
            int sendJump = _rng.Next(1, jump);
            if (sendJump > 30)
            {
                SendAFKKey(w, 0x20, GetRandomDelay(), 0);
            }

            int turn = 100;
            int sendTurn = _rng.Next(1, turn);
            int turnTime = _rng.Next(300, 1500);
            if (sendTurn > 30)
            {
                SendAFKKey(w, randomTurn, GetRandomDelay(), turnTime);
            }

            SendAFKKey(w, randomKey, 0, 0);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopping = true;
            Log("Waiting for threads to finish...");
            while (threadCount != 0)
            {
                DoEventsWait(100);
            }
            SaveSettings();

            ShutDown();
        }
        public static void DoEventsWait(int mil)
        {
            DateTime ls = DateTime.Now;
            while (DateTime.Now.Subtract(ls).TotalMilliseconds < mil)
                Application.DoEvents();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //shows or hides account data grid
            if (e.KeyCode == Keys.F3) { dataGridView1.Visible = true; DoEventsWait(10000); dataGridView1.Visible = false; }
            if (e.KeyCode == Keys.F4) { dataGridView1.Visible = false; }
        }
        private int GetGridSelectedIndex()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                return dataGridView1.CurrentCell.RowIndex;
            }
            return -1;
        }
        private void ChecKReGion32(ref WowWindow w, uint targetAddr)
        {
            IntPtr ta = (IntPtr)targetAddr;

            int result = NativeMethods.VirtualQueryEx(w.wow.ProcessHandle, ta, out NativeMethods.MEMORY_BASIC_INFORMATION mbi, (uint)Marshal.SizeOf(typeof(NativeMethods.MEMORY_BASIC_INFORMATION)));

            if (result != 0)
            {
                uint start = (uint)mbi.BaseAddress;
                uint end = start + (uint)mbi.RegionSize;

                Console.WriteLine($"Region Start: 0x{start:X}");
                Console.WriteLine($"Region End:   0x{end:X}");
                Console.WriteLine($"Region Size:  {mbi.RegionSize} bytes");

            }
        }
        private uint ScanRegion32(ref WowWindow w, uint startAddr, uint endAddr, byte[] Pattern, string Mask)
        {
            uint currentAddr = startAddr;
            while (currentAddr < endAddr)
            {
                uint sizeOfMbi = (uint)Marshal.SizeOf(typeof(NativeMethods.MEMORY_BASIC_INFORMATION));

                int result = NativeMethods.VirtualQueryEx(w.wow.ProcessHandle, (IntPtr)currentAddr, out NativeMethods.MEMORY_BASIC_INFORMATION mbi, sizeOfMbi);

                if (result == 0) break;

                uint regionEnd = (uint)mbi.BaseAddress + (uint)mbi.RegionSize;

                bool isGuardPage = (mbi.Protect & 0x100) != 0;
                if (!isGuardPage)
                    if (mbi.State == 0x1000 && mbi.Protect != 0x01)
                    {
                        uint scanEnd = Math.Min(regionEnd, endAddr);
                        int scanSize = (int)(scanEnd - currentAddr);

                        uint found1 = w.wow.FindPattern(currentAddr, scanSize, Pattern, Mask);
                        if (found1 != currentAddr)
                        {
                            return found1;
                            // Console.WriteLine($"Region {cc}: currentAddr: {currentAddr:X}, found1:0x{found1:X}");
                        }
                    }
                //move to next region
                currentAddr = regionEnd;
            }
            return 0;
        }

        private void WOW_StartNew()
        {
            if (busyLaunching)
                return;
            string wowpath = textBox2.Text;
            if (!IsPathExists(wowpath))
            {
                Log($"Cant find {wowpath}");
                return;
            }

            string wowexe = Path.Combine(wowpath, @"wow.exe");

            Process p = Process.Start(wowexe);
            busyLaunching = true;
            Log($"Launching new WOW client..");
            bool isReady = false;
            int timeout = 0;

            while (!isReady && timeout < 20)
            {
                p.Refresh();

                bool rrr = IsPointDataMatching(p.MainWindowHandle, pdLoginScreen);
                if (rrr)
                {
                    Log($"Done.");
                    busyLaunching = false;
                    isReady = true;
                }
                else
                {
                    DoEventsWait(300);
                    timeout++;
                }
            }
            if (timeout >= 20)
            {
                Log($"FAIL");
                busyLaunching = false;
                switchPending = false;
            }
        }
        private void Log(string s)
        {
            if (s == default)
                return;
            listBox1.Items.Add(s);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            listBox1.SelectedIndex = -1;
        }
        private void Test_button2_Click(object sender, EventArgs e)
        {
#if DEBUG
            SaveControlStates();
            LoadControlStates();
            if (!IfWOWWindowAvailable(out WowWindow w))
            {
                return;
            }
            //CharSelect(w, 2, true);

            return;
            //EnumerateWOWWindows();
            //Debug.WriteLine($"selected {GetGridSelectedIndex()}");
            char[] allChars = textBox1.Text.ToCharArray();
            List<short> keys = new List<short>();
            foreach (char c in allChars)
            {
                short key = NativeMethods.VkKeyScan(c);
                if (key != -1)
                {
                    keys.Add(key);
                }


            }
            return;
#endif
        }
        private void Button3_Click(object sender, EventArgs e)
        {
#if DEBUG
            DEBUGColorTimer.Enabled = true;
#endif
        }
        private void DEBUGColorTimer_Tick(object sender, EventArgs e)
        {
#if DEBUG
            Process_WOWWindows();
            if (!IfWOWWindowAvailable(out WowWindow w))
            {
                return;
            }
            Point cursor = new Point();
            NativeMethods.GetCursorPos(ref cursor);

            NativeMethods.ScreenToClient(w.handle, ref cursor);


            //uint disc_color = GetColorAt(w.handle, pdDisconnected.p.X, pdDisconnected.p.Y);
            PointData wd = pdRealmScreen;
            Point ptest = new Point(wd.p1.X, wd.p1.Y);
            uint ctest = wd.color1;

            uint color = GetColorAt(w.handle, ptest.X, ptest.Y);

            bool cmatch = false;
            if (color == ctest)
            {
                cmatch = true;
            }
           ;
            //Debug.WriteLine($"IsLoginScreen {IsLoginScreen()} IsRealmScreen {IsRealmScreen()} IsDisconnectedScreen {IsDisconnectedScreen()}");
            //Debug.WriteLine($"pdLoginScreen= {IsPointDataMatching(w.handle, pdLoginScreen)} pdRealmScreen= {IsPointDataMatching(w.handle, pdRealmScreen)} pdDisconnected= {IsPointDataMatching(w.handle, pdDisconnected)} ");
            Debug.WriteLine($" pdLoginScreen= {IsPointDataMatching(w.handle, pdLoginScreen)} ");

            //uint pixel = GetPixel(hdc, cursor.X, cursor.Y);
            uint cursor_color = GetColorAt(w.handle, cursor.X, cursor.Y);
            //Debug.WriteLine($"{w.title} X={cursor.X}  Y={cursor.Y}  pixel={cursor_color} match={cmatch} color={color} vs. {ctest} @ {ptest.X}x{ptest.Y}");

            uint realm1 = GetColorAt(w.handle, 152, 671);
            uint realm2 = GetColorAt(w.handle, 134, 847);
            uint realm3 = GetColorAt(w.handle, 1246, 60);
            //Debug.WriteLine($"{w.title} r1 {realm1} r2 {realm2}r3 {realm3}");
            //label1.Text = cursor.X + "." + cursor.Y + " " + (pixel.ToString());
#endif
        }

        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Label lb = label3;
            if (cb.Checked)
            {
                lb.Text = "SPAM IS ON";
                lb.ForeColor = Color.Red;
                if (int.TryParse(textBox4.Text, out int ms))
                {
                    spamtimer.Interval = ms;
                }
                else
                {
                    spamtimer.Interval = 1000;
                }
                spamtimer.Enabled = true;
            }
            else
            {
                lb.Text = "SPAM IS OFF";
                lb.ForeColor = Color.Green;
                spamtimer.Enabled = false;
            }
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cb = (CheckBox)sender;
            Label lb = label6;
            if (cb.Checked)
            {
                lb.Text = "ANTIAFK IS ON";
                lb.ForeColor = Color.Red;
                antiafktimer.Enabled = true;
            }
            else
            {
                lb.Text = "ANTIAFK IS OFF";
                lb.ForeColor = Color.Green;
                antiafktimer.Enabled = false;
            }
        }
        private void HighlightLine(int lineIndex, Color color)
        {
            if (lineIndex < 0 || lineIndex >= richTextBox1.Lines.Length) return;

            int start = richTextBox1.GetFirstCharIndexFromLine(lineIndex);
            int length = richTextBox1.Lines[lineIndex].Length;

            richTextBox1.SelectionStart = start;
            richTextBox1.SelectionLength = length;
            richTextBox1.SelectionBackColor = color;

            richTextBox1.SelectionLength = 0;
        }
        private void RichTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (WowWindows.Count == 0)
                return;
            int charIndex = richTextBox1.GetCharIndexFromPosition(e.Location);
            int lineIndex = richTextBox1.GetLineFromCharIndex(charIndex);

            if (lastSelectedLine != -1)
            {
                HighlightLine(lastSelectedLine, richTextBox1.BackColor);
            }

            HighlightLine(lineIndex, Color.LightBlue);
            lastSelectedLine = lineIndex;
            NativeMethods.SetForegroundWindow(WowWindows[lastSelectedLine].handle);
        }
        private string GetRootDir()
        {
            DirectoryInfo directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            return directory.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
#if !DEBUG
            this.Controls.Remove(button2);
            this.Controls.Remove(button3);

            button2.Dispose();
            button3.Dispose();
#endif
            rootDir = GetRootDir();
            settingsPath = Path.Combine(rootDir, @"settings.xml");
            grid1Path = Path.Combine(rootDir, @"grid1.xml");

            LoadSettings();
            listBox1.Items.Clear();


            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            CheckBox2_CheckedChanged(checkBox2, EventArgs.Empty);
            CheckBox1_CheckedChanged(checkBox1, EventArgs.Empty);
        }
        private void AntiAFKTimer_Tick(object sender, EventArgs e)
        {
            if (!stopping)
                foreach (WowWindow w in WowWindows)
                {
                    if (w.isLoggedIn)
                    {
                        AntiAFK_tick(w);
                    }
                }
        }
        private void SpamTimer_Tick(object sender, EventArgs e)
        {
            if (stopping)
            { return; }

            string[] parts = textBox5.Text.Split(',');

            List<int> numbers = new List<int>();

            foreach (string s in parts)
            {
                if (int.TryParse(s.Trim(), out int result))
                {
                    numbers.Add(result);
                }
            }
            foreach (WowWindow w in WowWindows)
            {
                if (numbers.Contains(w.count))
                {
                    if (w.isLoggedIn)
                    {
                        SendSpamKeys(w);
                    }
                }
            }
        }
        public void AddControlsToXml(Control parent, XElement xmlRoot)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is TextBox)
                {
                    xmlRoot.Add(new XElement(c.Name, c.Text));
                }
                if (c is CheckBox cb)
                {
                    xmlRoot.Add(new XElement(c.Name, cb.Checked));
                }
                if (c.HasChildren)
                {
                    AddControlsToXml(c, xmlRoot);
                }
            }
        }
        private void SaveSettings()
        {
            dataSet1.WriteXml(grid1Path);
            SaveControlStates();
        }
        private void LoadSettings()
        {
            LoadControlStates();
            if (System.IO.File.Exists(grid1Path))
            {
                dataGridView1.AutoGenerateColumns = true;
                dataSet1.Clear();
                dataSet1.ReadXml(grid1Path);
                dataGridView1.DataSource = dataSet1.Tables["Table1"];
            }
        }

        private void SaveControlStates()
        {
            var root =
                new XElement("Settings");
            AddControlsToXml(this, root);

            root.Save(settingsPath);
        }
        public void LoadControlStates()
        {
            if (!System.IO.File.Exists(settingsPath)) return;

            XElement root = XElement.Load(settingsPath);

            foreach (XElement el in root.Elements())
            {
                string controlName = el.Name.LocalName;
                string savedValue = el.Value;

                Control[] foundControls = this.Controls.Find(controlName, true);
                if (foundControls.Length > 1)
                {
                    System.Diagnostics.Debug.Fail("Duplicate control name found: " + controlName);
                }

                if (foundControls.Length > 0)
                {
                    if (foundControls[0] is TextBox box)
                    {
                        box.Text = savedValue;
                    }
                    else
                        if (foundControls[0] is CheckBox cb)
                        {
                            if (bool.TryParse(savedValue, out bool isChecked))
                            {
                                cb.Checked = isChecked;
                            }
                        }
                }
            }
        }

    }
    internal static class NativeMethods
    {

        public const int WM_SETREDRAW = 0x0b;
        public const int WM_KEYDOWN = 0x100;
        public const int WM_KEYUP = 0x101;
        public const int WM_CHAR = 0x102;
        public const int WM_INPUTLANGCHANGEREQUEST = 0x0050;

        public const int VK_RETURN = 0x0D;
        public const int VK_TAB = 0x0009;
        public const int VK_DOWN = 0x0028;

        public const string en_US = "00000409";
        public const uint KLF_ACTIVATE = 1;

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public IntPtr RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }

        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, uint dwLength);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetWindowText(IntPtr hWnd, string lpString);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(ref Point lpPoint);
        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);
        [DllImport("gdi32.dll")]
        public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern short VkKeyScan(char ch);
        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
    }
}