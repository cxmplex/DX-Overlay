# DX-Overlay
Extremely easy to use overlay base

# Overview

This is a sample base for a DX overlay using C#. The only pieces missing are the actual text and coloring variables in the dxthread.
By using window position bounds, we are easily able to set the overlay to wherever we want as well as choosing the monitor we wish to use.
Want to constantly overlay a window that may move? No worries.

# Usage

In order to use this, you need to implement both DwmExtendFrameIntoClientArea as well as SetWindowPos. This allows a lot of breathing room when it comes to the location of the overlay.
Using these, we are able to render the window transparently and avoid setting a topmost flag.
By periodically setting the window to the top (every 50ms or so), we do not cause adverse reactions with certain applications that may aim to block overlays.

Also, it is important to note that you will be doing this from a different thread. Always validate that your window handle is correct. An unrelated bug is that when another window requests top most, it has the possibility to stall the actual overlay thread. Be wary of this & implement the correct safety checks...

Example:
```
//You can dynamically set these margins. Margins is a struct that contains four integers. Left, Right, Top, Bottom. Remember that you will also need to set the form margins correctly or you'll end up with some weird bugs. 
var marg = new Win32.Margins();
marg.Left = 0;
marg.Top = 0;
marg.Right = 2560;
marg.Bottom = 1440;
Win32.DwmExtendFrameIntoClientArea(handle, ref marg);
//By setting the window position to the topmost, we avoid any basic checks for topmost windows.
//I got the definition from pinvoke!
SetWindowPos(handle, (IntPtr)Win32.SpecialWindowHandles.HWND_TOPMOST, 0, 0, 0, 0, 
Win32.SetWindowPosFlags.SWP_NOMOVE | Win32.SetWindowPosFlags.SWP_NOSIZE);
```
