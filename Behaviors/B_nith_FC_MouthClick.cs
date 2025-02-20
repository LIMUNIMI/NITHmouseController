using NITHemulation.Modules.Mouse;
using NITHlibrary.Nith.Internals;
using NITHmouseController.Modules;

namespace NITHmouseController.Behaviors;

public class BNithFcMouthClick : INithSensorBehavior
{
    private readonly List<NithParameters> _requiredParams = [NithParameters.mouth_isOpen];

    private bool _isLeftDown;
    private bool _isRightDown;

    // ^^ The problem here is that i want the system to let a click only after two discordant samples are received

    public void HandleData(NithSensorData nithData)
    {
        if (nithData.ContainsParameters(_requiredParams) && Rack.UserSettings.MouthClicking)
        {
            var mouthIsOpen = nithData.GetParameterValue(NithParameters.mouth_isOpen)!.Value.BaseAsBool;
            switch (Rack.MappingModule.CurrentClickButton)
            {
                case ClickButton.Left:
                    if (mouthIsOpen && !_isLeftDown)
                    {
                        MouseSender.SendMouseButtonEvent(MouseButtonFlags.LeftDown);
                        _isLeftDown = true;
                    }
                    else if (!mouthIsOpen && _isLeftDown)
                    {
                        MouseSender.SendMouseButtonEvent(MouseButtonFlags.LeftUp);
                        _isLeftDown = false;
                    }
                    break;

                case ClickButton.Right:
                    if (mouthIsOpen && !_isRightDown)
                    {
                        MouseSender.SendMouseButtonEvent(MouseButtonFlags.RightDown);
                        _isRightDown = true;
                    }
                    else if (!mouthIsOpen && _isRightDown)
                    {
                        MouseSender.SendMouseButtonEvent(MouseButtonFlags.RightUp);
                        _isRightDown = false;
                    }
                    break;
            }
        }
    }
}