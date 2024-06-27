using System.IO;
using System.Text;
using System.Windows.Controls;

public class ConsoleTextToTextBox : TextWriter
{
    private TextBlock _textBlock;
    private ScrollViewer _scrollViewer;
    private Queue<string> _lines = new Queue<string>();
    private readonly int _maxLines = 100;

    public ConsoleTextToTextBox(TextBlock textBlock, ScrollViewer scrollViewer)
    {
        _textBlock = textBlock;
        _scrollViewer = scrollViewer;
        textBlock.Text = string.Empty;
    }

    public override Encoding Encoding => Encoding.UTF8;

    public override void Write(char value)
    {
        Write(value.ToString());
    }

    public override void WriteLine(string value)
    {
        _textBlock.Dispatcher.Invoke(() =>
        {
            if (_lines.Count >= _maxLines)
            {
                _lines.Dequeue();
            }

            _lines.Enqueue(value);
            _textBlock.Text = string.Join("\n", _lines);

            // Ensure the ScrollViewer scrolls to the bottom
            _scrollViewer.ScrollToEnd();
        });
    }
}