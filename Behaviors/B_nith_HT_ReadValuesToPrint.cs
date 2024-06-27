using NITHmouseController.Modules;
using NITHlibrary.Nith.Internals;

namespace NITHmouseController.Behaviors
{
    internal class BNithHtReadValuesToPrint : INithSensorBehavior
    {
        private const int MinArgLength = 15;
        private string _argumentStr = "";

        public void HandleData(NithSensorData nithData)
        {
            Rack.DataManagerModule.HeadTrackerSensorName = nithData.SensorName + "-" + nithData.Version;
            Rack.DataManagerModule.HeadTrackerStatusCode = nithData.StatusCode.ToString();

            _argumentStr = "";
            if(nithData.Values != null)
            {
                foreach (NithArgumentValue val in nithData.Values)
                {
                    _argumentStr += AddWhiteSpaces(val.Argument.ToString());
                    _argumentStr += "v: ";
                    if (val.Type == NithDataTypes.OnlyBase)
                    {
                        _argumentStr += val.Base;
                    }
                    else if (val.Type == NithDataTypes.BaseAndMax)
                    {
                        _argumentStr += val.Base + " / " + val.Max + "\tp: " + val.Proportional.ToString("F2");
                    }
                    _argumentStr += "\n";
                }
            }

            // Update argumentStr and raw lines
            Rack.DataManagerModule.HeadTrackerArgumentsString = _argumentStr;
            Rack.DataManagerModule.AppendHeadTrackerRawLine("[" + DateTime.Now.ToString("HH:mm ss:fff") + "] " + nithData.RawLine);
        }

        private string AddWhiteSpaces(string input)
        {
            if (input.Length < MinArgLength)
            {
                int numberOfSpaces = MinArgLength - input.Length;
                string whiteSpaces = new string(' ', numberOfSpaces);
                return input + whiteSpaces;
            }
            else
            {
                return input;
            }
        }
    }
}