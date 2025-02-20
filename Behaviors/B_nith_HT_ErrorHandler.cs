using NITHmouseController.Modules;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Nith.Module;
using NITHlibrary.Nith.BehaviorTemplates;

namespace NITHmouseController.Behaviors
{
    internal class BNithHtErrorHandler : ANithErrorToStringBehavior
    {
        private string _lastErrorString = "";

        public BNithHtErrorHandler(NithModule nithModule) : base(nithModule)
        {
        }

        protected override void ErrorActions(string errorString, NithErrors error)
        {

            if(errorString != _lastErrorString)
            {
                Rack.DataManagerModule.AppendHeadTrackerErrorLine("[" + DateTime.Now.ToString("HH:mm ss:fff") + "] " + errorString);
            }

            _lastErrorString = errorString;
        }
    }
}
