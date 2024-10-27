using System.Diagnostics;

namespace ChessGui.Stockfish;

public class StockfishProcess
{
    private readonly string _enginePath;
    private readonly Process _process;

    public StockfishProcess(string enginePath)
    {
        _enginePath = enginePath;
        _process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _enginePath,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            }
        };
    }

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
        this._process.WaitForExit(millisecond);
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