using System;
using System.Runtime.InteropServices;
using WiimoteLib; // Make sure to install the WiimoteLib package from NuGet

class WiiMoteMouse
{
    private static readonly int screenWidth = GetSystemMetrics(0);
    private static readonly int screenHeight = GetSystemMetrics(1);

    [DllImport("user32.dll")]
    private static extern bool SetCursorPos(int X, int Y);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern void mouse_event(uint dwFlags, int dx, int dy, int cButtons, uint dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    static void Tick()
    {
        var wiimote = new Wiimote();
        wiimote.Connect();

        // Set the IR sensor mode and sensitivity
        wiimote.SetReportType(InputReport.IRAccel, true);
        wiimote.SetIRSensorMode(IRSensorMode.Extended);
        wiimote.SetIRSensitivity(IR sensitivity: IRSensitivity.Maximum);

        while (true)
        {
            var state = wiimote.WiimoteState;

            if (state.IRState.IRSensors[0].Found && state.IRState.IRSensors[1].Found)
            {
                // Calculate the cursor position based on the IR sensor data
                int curX = (int)(state.IRState.IRSensors[0].RawPosition.X * screenWidth);
                int curY = (int)(state.IRState.IRSensors[0].RawPosition.Y * screenHeight);

                // Set the cursor position
                SetCursorPos(curX, curY);

                // Simulate a left mouse button click
                mouse_event(dwFlags: 0x0002, dx: curX, dy: curY, cButtons: 0, dwExtraInfo: 0);
            }
        }
    }
}
