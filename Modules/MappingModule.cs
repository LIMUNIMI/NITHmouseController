namespace NITHmouseController
{
    public class MappingModule
    {
        public double StanceThreshold { get; set; } = 15f;
        public Stances CurrentStance { get; set; } = Stances.Center;
        public ClickButton CurrentClickButton { get; set; } = ClickButton.Left;
    }

    public enum Stances
    {
        Left,
        Center,
        Right
    }

    public enum ClickButton
    {
        Left,
        Right
    }
}