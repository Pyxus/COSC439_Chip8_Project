using SFML.Graphics;
using SFML.Window;
using SFML.System;

public class Debugger
{
    private RenderWindow _window;
    private Text _registerText;
    private Text _memoryText;
    private Text _vramText;
    private int _memoryLineSkip;
    private int _vramLineSkip;

    public Debugger()
    {
        var sFile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\src\Chip8\fonts\JetBrainsMono-Regular.ttf");
        var sFilePath = Path.GetFullPath(sFile);
        var font = new Font(sFilePath);

        _window = new RenderWindow(new VideoMode(1280, 720), "Chip8 - Debugger");
        _window.Closed += OnWindowClose;
        _window.MouseWheelScrolled += OnWindowMouseWheelScrolled;       

        _registerText = new Text()
        {
            Font = font,
            CharacterSize = 12,
            FillColor = Color.White,
            Position = new Vector2f(0, 0),
        };

        _memoryText = new Text()
        {
            Font = font,
            CharacterSize = 12,
            FillColor = Color.White,
            Position = new Vector2f(125, 0),
        };

        _vramText = new Text()
        {
            Font = font,
            CharacterSize = 12,
            FillColor = Color.White,
            Position = new Vector2f(800, 0),
        };
    }

    public void Update(string registerText, string memoryText, string vramText)
    {
        _window.DispatchEvents();
        _window.Clear();

        var memoryLines = memoryText.Replace("\r", "").Split('\n');
        var memoryColumns = memoryLines[..1];
        var memoryRows = memoryLines[2..];
        var memString = "[RAM]\n";
        _memoryLineSkip = Math.Clamp(_memoryLineSkip, 0, memoryRows.Length);

        for (var i = 0; i < memoryColumns.Length; i++)
        {
            memString += memoryColumns[i] + "\n\n";
        }

        for (var i = _memoryLineSkip; i < memoryRows.Length; i++)
        {
            memString += memoryRows[i] + "\n";
        }

        var vramLines = vramText.Replace("\r", "").Split('\n');
        var vramColumns = vramLines[..1];
        var vramRows = vramLines[2..];
        var vramString = "[VRAM]\n";
        _vramLineSkip = Math.Clamp(_vramLineSkip, 0, vramRows.Length);

        for (var i = 0; i < vramColumns.Length; i++)
        {
            vramString += vramColumns[i] + "\n\n";
        }

        for (var i = _memoryLineSkip; i < vramRows.Length; i++)
        {
            vramString += vramRows[i] + "\n";
        }

        _registerText.DisplayedString = registerText;
        _memoryText.DisplayedString = memString;
        _vramText.DisplayedString = vramString;
        
        _window.Draw(_registerText);
        _window.Draw(_memoryText);
        _window.Draw(_vramText);
        _window.Display();
    }

    public bool IsWindowOpen()
    {
        return _window.IsOpen;
    }

    private void OnWindowClose(object? sender, EventArgs args)
    {
        _window.Close();
    }

    private void OnWindowMouseWheelScrolled(object? sender, MouseWheelScrollEventArgs args)
    {
        if (args.Delta >= 1)
        {
            _memoryLineSkip--;
            _vramLineSkip--;
        }
        else if (args.Delta <= -1)
        {
            _memoryLineSkip++;
            _vramLineSkip++;
        }
    }
}