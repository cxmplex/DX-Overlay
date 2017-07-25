using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SharpDX;
using SharpDX.DXGI;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

using System.Threading;
using System.Runtime.InteropServices;

namespace OverlayExample
{
    public partial class Example : Form
    {
        public Example()
        {
            InitializeComponent();
            Screen[] sc;
            sc = Screen.AllScreens;
            this.Bounds = sc[0].Bounds; //better results than FormWindowState.Maximum!
            this.BackColor = System.Drawing.Color.Black;
            this.FormBorderStyle = FormBorderStyle.None;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = false;
            this.WindowState = FormWindowState.Normal;


            Win32.SetWindowLong(this.Handle, Win32.GWL_EXSTYLE, (IntPtr)(Win32.GetWindowLong(this.Handle, Win32.GWL_EXSTYLE) ^ Win32.WS_EX_LAYERED ^ Win32.WS_EX_TRANSPARENT));
            //Clickable
            Win32.SetLayeredWindowAttributes(this.Handle, 0, 255, Win32.LWA_ALPHA);

            var targetProperties = new HwndRenderTargetProperties
            {
                Hwnd = this.Handle,
                PixelSize = new Size2(this.Bounds.Right - this.Bounds.Left, this.Bounds.Bottom - this.Bounds.Top),
                PresentOptions = PresentOptions.Immediately
            };
            var prop = new RenderTargetProperties(RenderTargetType.Hardware, new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied), 0, 0, RenderTargetUsage.None, FeatureLevel.Level_DEFAULT);

            var d3dFactory = new SharpDX.Direct2D1.Factory();

            var device = new WindowRenderTarget(d3dFactory, prop, targetProperties)
            {
                TextAntialiasMode = TextAntialiasMode.Cleartype,
                AntialiasMode = AntialiasMode.Aliased
            };

            var dxthread = new Thread(() =>
            {
                //define your text and colors here!
                while (true)
                {
                    device.BeginDraw();
                    device.Clear(null);

                    //drawing code goes here!

                    device.EndDraw();
                    Thread.Sleep(1000 / 60);
                }
            });
            dxthread.IsBackground = true;
            dxthread.Start();
        }

        private static class Win32
        {
            #region Definitions
            public const int GWL_EXSTYLE = -20;

            public const int WS_EX_LAYERED = 0x80000;

            public const int WS_EX_TRANSPARENT = 0x20;

            public const int LWA_ALPHA = 0x2;

            public const int LWA_COLORKEY = 0x1;
            #endregion

            #region Functions
            [DllImport("user32.dll", SetLastError = true)]
            public static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("user32.dll")]
            public static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

            [DllImport("user32.dll")]
            public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);
            #endregion
        }
    }
}
