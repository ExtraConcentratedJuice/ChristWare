using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
namespace ChristWare.UI
{
    public enum WindowMessage : uint
    {
        Null = 0x0000,
        Create = 0x0001,
        Destroy = 0x0002,
        Move = 0x0003,
        Size = 0x0005,
        Activate = 0x0006,
        Setfocus = 0x0007,
        Killfocus = 0x0008,
        Enable = 0x000A,
        Setredraw = 0x000B,
        Settext = 0x000C,
        Gettext = 0x000D,
        Gettextlength = 0x000E,
        Paint = 0x000F,
        Close = 0x0010,
        Queryendsession = 0x0011,
        Queryopen = 0x0013,
        Endsession = 0x0016,
        Quit = 0x0012,
        EraseBackground = 0x0014,
        Syscolorchange = 0x0015,
        Showwindow = 0x0018,
        Wininichange = 0x001A,
        Settingchange = Wininichange,
        Devmodechange = 0x001B,
        Activateapp = 0x001C,
        Fontchange = 0x001D,
        Timechange = 0x001E,
        Cancelmode = 0x001F,
        Setcursor = 0x0020,
        Mouseactivate = 0x0021,
        Childactivate = 0x0022,
        Queuesync = 0x0023,
        Getminmaxinfo = 0x0024,
        Painticon = 0x0026,
        Iconerasebkgnd = 0x0027,
        Nextdlgctl = 0x0028,
        Spoolerstatus = 0x002A,
        Drawitem = 0x002B,
        Measureitem = 0x002C,
        Deleteitem = 0x002D,
        Vkeytoitem = 0x002E,
        Chartoitem = 0x002F,
        Setfont = 0x0030,
        Getfont = 0x0031,
        Sethotkey = 0x0032,
        Gethotkey = 0x0033,
        Querydragicon = 0x0037,
        Compareitem = 0x0039,
        Getobject = 0x003D,
        Compacting = 0x0041,
        Commnotify = 0x0044,
        Windowposchanging = 0x0046,
        Windowposchanged = 0x0047,
        Power = 0x0048,
        Copydata = 0x004A,
        Canceljournal = 0x004B,
        Notify = 0x004E,
        Inputlangchangerequest = 0x0050,
        Inputlangchange = 0x0051,
        Tcard = 0x0052,
        Help = 0x0053,
        Userchanged = 0x0054,
        Notifyformat = 0x0055,
        Contextmenu = 0x007B,
        Stylechanging = 0x007C,
        Stylechanged = 0x007D,
        Displaychange = 0x007E,
        Geticon = 0x007F,
        Seticon = 0x0080,
        Nccreate = 0x0081,
        Ncdestroy = 0x0082,
        Nccalcsize = 0x0083,
        Nchittest = 0x0084,
        NcPaint = 0x0085,
        Ncactivate = 0x0086,
        Getdlgcode = 0x0087,
        Syncpaint = 0x0088,

        Ncmousemove = 0x00A0,
        Nclbuttondown = 0x00A1,
        Nclbuttonup = 0x00A2,
        Nclbuttondblclk = 0x00A3,
        Ncrbuttondown = 0x00A4,
        Ncrbuttonup = 0x00A5,
        Ncrbuttondblclk = 0x00A6,
        Ncmbuttondown = 0x00A7,
        Ncmbuttonup = 0x00A8,
        Ncmbuttondblclk = 0x00A9,
        Ncxbuttondown = 0x00AB,
        Ncxbuttonup = 0x00AC,
        Ncxbuttondblclk = 0x00AD,

        InputDeviceChange = 0x00FE,
        Input = 0x00FF,

        Keyfirst = 0x0100,
        Keydown = 0x0100,
        Keyup = 0x0101,
        Char = 0x0102,
        Deadchar = 0x0103,
        Syskeydown = 0x0104,
        Syskeyup = 0x0105,
        Syschar = 0x0106,
        Sysdeadchar = 0x0107,
        Unichar = 0x0109,
        Keylast = 0x0109,

        ImeStartcomposition = 0x010D,
        ImeEndcomposition = 0x010E,
        ImeComposition = 0x010F,
        ImeKeylast = 0x010F,

        Initdialog = 0x0110,
        Command = 0x0111,
        Syscommand = 0x0112,
        Timer = 0x0113,
        Hscroll = 0x0114,
        Vscroll = 0x0115,
        Initmenu = 0x0116,
        Initmenupopup = 0x0117,
        Menuselect = 0x011F,
        Menuchar = 0x0120,
        Enteridle = 0x0121,
        Menurbuttonup = 0x0122,
        Menudrag = 0x0123,
        Menugetobject = 0x0124,
        Uninitmenupopup = 0x0125,
        Menucommand = 0x0126,

        Changeuistate = 0x0127,
        Updateuistate = 0x0128,
        Queryuistate = 0x0129,

        Ctlcolormsgbox = 0x0132,
        Ctlcoloredit = 0x0133,
        Ctlcolorlistbox = 0x0134,
        Ctlcolorbtn = 0x0135,
        Ctlcolordlg = 0x0136,
        Ctlcolorscrollbar = 0x0137,
        Ctlcolorstatic = 0x0138,
        Gethmenu = 0x01E1,

        Mousefirst = 0x0200,
        Mousemove = 0x0200,
        Lbuttondown = 0x0201,
        Lbuttonup = 0x0202,
        Lbuttondblclk = 0x0203,
        Rbuttondown = 0x0204,
        Rbuttonup = 0x0205,
        Rbuttondblclk = 0x0206,
        Mbuttondown = 0x0207,
        Mbuttonup = 0x0208,
        Mbuttondblclk = 0x0209,
        Mousewheel = 0x020A,
        Xbuttondown = 0x020B,
        Xbuttonup = 0x020C,
        Xbuttondblclk = 0x020D,
        Mousehwheel = 0x020E,

        Parentnotify = 0x0210,
        Entermenuloop = 0x0211,
        Exitmenuloop = 0x0212,

        Nextmenu = 0x0213,
        Sizing = 0x0214,
        Capturechanged = 0x0215,
        Moving = 0x0216,

        Powerbroadcast = 0x0218,

        Devicechange = 0x0219,

        Mdicreate = 0x0220,
        Mdidestroy = 0x0221,
        Mdiactivate = 0x0222,
        Mdirestore = 0x0223,
        Mdinext = 0x0224,
        Mdimaximize = 0x0225,
        Mditile = 0x0226,
        Mdicascade = 0x0227,
        Mdiiconarrange = 0x0228,
        Mdigetactive = 0x0229,

        Mdisetmenu = 0x0230,
        Entersizemove = 0x0231,
        Exitsizemove = 0x0232,
        Dropfiles = 0x0233,
        Mdirefreshmenu = 0x0234,

        ImeSetcontext = 0x0281,
        ImeNotify = 0x0282,
        ImeControl = 0x0283,
        ImeCompositionfull = 0x0284,
        ImeSelect = 0x0285,
        ImeChar = 0x0286,
        ImeRequest = 0x0288,
        ImeKeydown = 0x0290,
        ImeKeyup = 0x0291,

        Mousehover = 0x02A1,
        Mouseleave = 0x02A3,
        Ncmousehover = 0x02A0,
        Ncmouseleave = 0x02A2,

        WtssessionChange = 0x02B1,

        TabletFirst = 0x02c0,
        TabletLast = 0x02df,

        DpiChanged = 0x02E0,

        Cut = 0x0300,
        Copy = 0x0301,
        Paste = 0x0302,
        Clear = 0x0303,
        Undo = 0x0304,
        Renderformat = 0x0305,
        Renderallformats = 0x0306,
        Destroyclipboard = 0x0307,
        Drawclipboard = 0x0308,
        Paintclipboard = 0x0309,
        Vscrollclipboard = 0x030A,
        Sizeclipboard = 0x030B,
        Askcbformatname = 0x030C,
        Changecbchain = 0x030D,
        Hscrollclipboard = 0x030E,
        Querynewpalette = 0x030F,
        Paletteischanging = 0x0310,
        Palettechanged = 0x0311,
        Hotkey = 0x0312,

        Print = 0x0317,
        Printclient = 0x0318,

        Appcommand = 0x0319,

        Themechanged = 0x031A,

        Clipboardupdate = 0x031D,

        DwmCompositionChanged = 0x031E,
        Dwmncrenderingchanged = 0x031F,
        Dwmcolorizationcolorchanged = 0x0320,
        Dwmwindowmaximizedchange = 0x0321,

        Gettitlebarinfoex = 0x033F,

        Handheldfirst = 0x0358,
        Handheldlast = 0x035F,

        Afxfirst = 0x0360,
        Afxlast = 0x037F,

        Penwinfirst = 0x0380,
        Penwinlast = 0x038F,

        App = 0x8000,

        User = 0x0400,

        Reflect = User + 0x1C00
    }
    [Flags]
    public enum WindowStyles : uint
    {
        WS_BORDER = 0x800000,
        WS_CAPTION = 0xc00000,
        WS_CHILD = 0x40000000,
        WS_CLIPCHILDREN = 0x2000000,
        WS_CLIPSIBLINGS = 0x4000000,
        WS_DISABLED = 0x8000000,
        WS_DLGFRAME = 0x400000,
        WS_GROUP = 0x20000,
        WS_HSCROLL = 0x100000,
        WS_MAXIMIZE = 0x1000000,
        WS_MAXIMIZEBOX = 0x10000,
        WS_MINIMIZE = 0x20000000,
        WS_MINIMIZEBOX = 0x20000,
        WS_OVERLAPPED = 0x0,
        WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
        WS_POPUP = 0x80000000u,
        WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
        WS_SIZEFRAME = 0x40000,
        WS_SYSMENU = 0x80000,
        WS_TABSTOP = 0x10000,
        WS_VISIBLE = 0x10000000,
        WS_VSCROLL = 0x200000
    }

    [Flags]
    public enum WindowStylesEx : uint
    {
        WS_EX_ACCEPTFILES = 0x00000010,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_COMPOSITED = 0x02000000,
        WS_EX_CONTEXTHELP = 0x00000400,
        WS_EX_CONTROLPARENT = 0x00010000,
        WS_EX_DLGMODALFRAME = 0x00000001,
        WS_EX_LAYERED = 0x00080000,
        WS_EX_LAYOUTRTL = 0x00400000,
        WS_EX_LEFT = 0x00000000,
        WS_EX_LEFTSCROLLBAR = 0x00004000,
        WS_EX_LTRREADING = 0x00000000,
        WS_EX_MDICHILD = 0x00000040,
        WS_EX_NOACTIVATE = 0x08000000,
        WS_EX_NOINHERITLAYOUT = 0x00100000,
        WS_EX_NOPARENTNOTIFY = 0x00000004,
        WS_EX_NOREDIRECTIONBITMAP = 0x00200000,
        WS_EX_OVERLAPPEDWINDOW = WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE,
        WS_EX_PALETTEWINDOW = WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST,
        WS_EX_RIGHT = 0x00001000,
        WS_EX_RIGHTSCROLLBAR = 0x00000000,
        WS_EX_RTLREADING = 0x00002000,
        WS_EX_STATICEDGE = 0x00020000,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_TRANSPARENT = 0x00000020,
        WS_EX_WINDOWEDGE = 0x00000100
    }
    public class ChristWareUI
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WNDCLASSEX
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public int style;
            public IntPtr lpfnWndProc; // not WndProc
            public int cbClsExtra;
            public int cbWndExtra;
            public IntPtr hInstance;
            public IntPtr hIcon;
            public IntPtr hCursor;
            public IntPtr hbrBackground;
            public string lpszMenuName;
            public string lpszClassName;
            public IntPtr hIconSm;

            //Use this function to make a new one with cbSize already filled in.
            //For example:
            //var WndClss = WNDCLASSEX.Build()
            public static WNDCLASSEX Build()
            {
                var nw = new WNDCLASSEX();
                nw.cbSize = Marshal.SizeOf(typeof(WNDCLASSEX));
                return nw;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U2)]
        static extern UInt16 RegisterClassEx([In] ref WNDCLASSEX lpwcx);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowEx(
           WindowStylesEx dwExStyle,
           UInt16 classname,
           [MarshalAs(UnmanagedType.LPStr)] string lpWindowName,
           WindowStyles dwStyle,
           int x,
           int y,
           int nWidth,
           int nHeight,
           IntPtr hWndParent,
           IntPtr hMenu,
           IntPtr hInstance,
           IntPtr lpParam);

        [DllImport("kernel32.dll")]
        static extern uint GetLastError();

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("user32.dll")]
        static extern IntPtr DefWindowProc(IntPtr hWnd, WindowMessage uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool UpdateWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);


        delegate IntPtr WindowProc(IntPtr hWnd, WindowMessage msg, IntPtr wParam, IntPtr lParam);

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_LAYERED = 0x80000;
        public const int WS_EX_TRANSPARENT = 0x20;

        private static WindowProc winprocdele = new WindowProc(WindowProcedure);

        public static IntPtr CreateWindow()
        {
            var extendedWindowStyle = WindowStylesEx.WS_EX_CLIENTEDGE | WindowStylesEx.WS_EX_TOPMOST;
            //var extendedWindowStyle = (WindowStylesEx)(0x8 | 0x20 | 0x80000 | 0x80 | 0x8000000);
            var ws = WindowStyles.WS_VISIBLE;

            var wclass = new WNDCLASSEX
            {
                cbSize = Marshal.SizeOf<WNDCLASSEX>(),
                style = 0,
                lpfnWndProc = Marshal.GetFunctionPointerForDelegate(winprocdele),
                cbClsExtra = 0,
                cbWndExtra = 0,
                hInstance = IntPtr.Zero,
                hIcon = IntPtr.Zero,
                hCursor = IntPtr.Zero,
                hbrBackground = IntPtr.Zero,
                lpszMenuName = null,
                lpszClassName = "CWWindow",
                hIconSm = IntPtr.Zero
            };

            var atom = RegisterClassEx(ref wclass);
            var handle = CreateWindowEx(extendedWindowStyle, atom, "christware", ws, 50, 50, 200, 200, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
            SetLayeredWindowAttributes(handle, 0, 0, 0x2);
            //UpdateWindow(handle);
            //ShowWindow(handle, 4);


            return handle;
        }

        private static IntPtr WindowProcedure(IntPtr hwnd, WindowMessage msg, IntPtr wParam, IntPtr lParam)
        {
            switch (msg)
            {
                case WindowMessage.EraseBackground:
                    //User32.SendMessage(hwnd, WindowMessage.Paint, (IntPtr)0, (IntPtr)0);
                    break;

                case WindowMessage.Keyup:
                case WindowMessage.Keydown:
                case WindowMessage.Syscommand:
                case WindowMessage.Syskeydown:
                case WindowMessage.Syskeyup:
                    return (IntPtr)0;

                case WindowMessage.NcPaint:
                case WindowMessage.Paint:
                    return (IntPtr)0;

                case WindowMessage.DwmCompositionChanged:
                    //WindowHelper.ExtendFrameIntoClientArea(hwnd);
                    return (IntPtr)0;

                case WindowMessage.DpiChanged:
                    return (IntPtr)0; // block DPI changed message

                default: break;
            }

            return DefWindowProc(hwnd, msg, wParam, lParam);
        }
    }
}
*/