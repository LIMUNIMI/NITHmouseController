namespace NITHmouseController.Setups
{
    internal class UserSettings
    {
        private float _htSensitivity = 50f;
        public bool BlinkClicking { get; set; } = false;
        public bool EmulateCursor { get; set; } = false;

        public float HtSensitivity
        {
            get { return _htSensitivity; }
            set { _htSensitivity = value < 1 ? 1 : value; }
        }

        public bool MouthClicking { get; set; } = false;
    }
}