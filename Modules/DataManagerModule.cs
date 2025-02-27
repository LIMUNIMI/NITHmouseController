namespace NITHmouseController.Modules
{
    internal class DataManagerModule
    {
        private const int MaxErrorLines = 100;
        private const int MaxRawLines = 100;

        public string FaceCamArgumentsString { get; set; }
        public string FaceCamErrorsString { get; private set; } = "";
        public string FaceCamRawString { get; private set; } = "";
        public string UDPsensorName { get; set; } = "";
        public string UDPstatusCode { get; set; } = "";
        public string HeadTrackerArgumentsString { get; set; }
        public string HeadTrackerErrorsString { get; private set; } = "";
        public string HeadTrackerRawString { get; private set; } = "";
        public string HeadTrackerSensorName { get; set; } = "";
        public string HeadTrackerStatusCode { get; set; } = "";
        private Queue<string> FaceCamErrorQueue { get; set; } = new Queue<string>();
        private Queue<string> FaceCamRawQueue { get; set; } = new Queue<string>();
        private Queue<string> HeadTrackerErrorQueue { get; set; } = new Queue<string>();
        private Queue<string> HeadTrackerRawQueue { get; set; } = new Queue<string>();

        internal void AppendFaceCamErrorLine(string line)
        {
            FaceCamErrorQueue.Enqueue(line);
            if (FaceCamErrorQueue.Count > MaxErrorLines) { FaceCamErrorQueue.Dequeue(); }
            FaceCamErrorsString = string.Join("\n", FaceCamErrorQueue.Reverse().ToArray());
        }

        internal void AppendFaceCamRawLine(string line)
        {
            FaceCamRawQueue.Enqueue(line);
            if (FaceCamRawQueue.Count > MaxRawLines) { FaceCamRawQueue.Dequeue(); }
            FaceCamRawString = string.Join("\n", FaceCamRawQueue.Reverse().ToArray());
        }

        internal void AppendHeadTrackerErrorLine(string line)
        {
            HeadTrackerErrorQueue.Enqueue(line);
            if (HeadTrackerErrorQueue.Count > MaxErrorLines) { HeadTrackerErrorQueue.Dequeue(); }
            HeadTrackerErrorsString = string.Join("\n", HeadTrackerErrorQueue.Reverse().ToArray());
        }

        internal void AppendHeadTrackerRawLine(string line)
        {
            HeadTrackerRawQueue.Enqueue(line);
            if (HeadTrackerRawQueue.Count > MaxRawLines) { HeadTrackerRawQueue.Dequeue(); }
            HeadTrackerRawString = string.Join("\n", HeadTrackerRawQueue.Reverse().ToArray());
        }

        internal void ClearFaceCamErrorQueue()
        {
            FaceCamErrorQueue.Clear();
        }

        internal void ClearFaceCamRawQueue()
        {
            FaceCamRawQueue.Clear();
        }

        internal void ClearHeadTrackerErrorQueue()
        {
            HeadTrackerErrorQueue.Clear();
        }

        internal void ClearHeadTrackerRawQueue()
        {
            HeadTrackerRawQueue.Clear();
        }
    }
}