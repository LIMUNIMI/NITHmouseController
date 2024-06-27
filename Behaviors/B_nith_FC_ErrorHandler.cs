using NITHmouseController.Modules;
using NITHlibrary.Nith.Behaviors;
using NITHlibrary.Nith.Internals;
using NITHlibrary.Nith.Module;

namespace NITHmouseController.Behaviors
{
    internal class BNithFcErrorHandler : AStandardNithErrorStringBehavior
    {
        private string _lastErrorString = "";

        public BNithFcErrorHandler(INithModule nithModule) : base(nithModule)
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
