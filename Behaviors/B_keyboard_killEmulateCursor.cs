using NITHdmis.Modules.Keyboard;
using NITHmouseController.Modules;
using RawInputProcessor;

namespace NITHmouseController.Behaviors
{
    public class BKeyboardKillEmulateCursor : IKeyboardBehavior
    {
        private const VKeyCodes KeyKill = VKeyCodes.K;
        public int ReceiveEvent(RawInputEventArgs e)
        {
            
            // Key Enable
            if (e.VirtualKey == (ushort)KeyKill && e.KeyPressState == KeyPressState.Down)
            { 
                Rack.UserSettings.EmulateCursor = false;
            }


            return 0;
        }
    }
}
