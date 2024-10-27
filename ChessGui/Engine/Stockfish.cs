using Chess;

namespace ChessGui.Engine;

public class Stockfish
{
    private StockfishProcess Process { get; }

    public Stockfish(string pathToEngine)
    {
        Process = new StockfishProcess(pathToEngine);
        Process.Start();
        Process.WriteLine("uci");
        CheckForMessage("uciok");
        Process.WriteLine($"setoption Skill Level {Program.Config!.Strength}");
    }

    private void CheckForMessage(string message = "readyok")
    {
        while (true)
        {
            if (Process.ReadLine() == message)
                return;
        }
    }

    private string CheckForMessageStartsWith(string message = "readyok")
    {
        while (true)
        {
            var line = Process.ReadLine();
            if (line.StartsWith(message))
                return line;
        }
    }

    public void SetFenPosition(string fenPosition)
    {
        Process.WriteLine($"position fen {fenPosition}");
    }

    public Move GetBestMove()
    {
        Process.WriteLine($"go movetime {Program.Config!.MoveTime}");
        // example: bestmove b8c6 ponder f1c4
        var output = CheckForMessageStartsWith("bestmove");
        var splitted = output.Split(' ');
        var bestMove = splitted[1];

        var from = bestMove[..2];
        var to = bestMove[2..4];

        return new Move(from, to);
    }
}