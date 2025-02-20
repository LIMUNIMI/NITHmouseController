using NITHmouseController.Modules;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Nith.Module;
using NITHlibrary.Nith.BehaviorTemplates;

namespace NITHmouseController.Behaviors
{
    internal class BNithFcErrorHandler : ANithErrorToStringBehavior
    {
        private string _lastErrorString = "";

        public BNithFcErrorHandler(NithModule nithModule) : base(nithModule)
        {
        }

        protected override void ErrorActions(string errorString, NithErrors error)
        {

            if(errorString != _lastErrorString)
            {
                Rack.DataManagerModule.AppendFaceCamErrorLine("[" + DateTime.Now.ToString("HH:mm ss:fff") + "] " + errorString);
            }

            _lastErrorString = errorString;
        }
    }
}
