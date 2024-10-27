namespace ChessGui.Stockfish;

public class Stockfish
{
    public StockfishProcess Process { get; set; }

    public Stockfish(string pathToEngine)
    {
        Process = new StockfishProcess(pathToEngine);
    }

    pub
}