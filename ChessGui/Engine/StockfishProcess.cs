using System.Diagnostics;

namespace ChessGui.Engine;

public class StockfishProcess(string enginePath)
{
    private readonly Process _process = new()
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = enginePath,
            UseShellExecute = false,
            RedirectStandardError = true,
            RedirectStandardInput = true,
            RedirectStandardOutput = true
        }
    };

    ~StockfishProcess()
    {
        _process.Close();
    }

    public void Start()
    {
        _process.Start();
    }

    public void Wait(int millisecond)
    {
        _process.WaitForExit(millisecond);
    }

    public void WriteLine(string command)
    {
        if (_process.StandardInput == null)
        {
            throw new NullReferenceException();
        }
        _process.StandardInput.WriteLine(command);
        _process.StandardInput.Flush();
    }

    public string ReadLine()
    {
        if (_process.StandardOutput == null)
        {
            throw new NullReferenceException();
        }
        return _process.StandardOutput.ReadLine()!;
    }
}