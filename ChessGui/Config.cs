using System.Numerics;

namespace ChessGui;

public class Config
{
    // colors
    public Vector4 Color1 = new(0xf0 / 255f, 0xf1 / 255f, 0xf0 / 255f, 1);
    public Vector4 Color2 = new(0x84 / 255f, 0x76 / 255f, 0xba / 255f, 1);
    public Vector4 SelectColor = new(0xb7 / 255f, 0xcf / 255f, 0xdd / 255f, 1f);
    public Vector4 CheckedColor = new(0xff / 255f, 0x00 / 255f, 0x00 / 255f, 1f);

    // board
    public int SquareSize { get; set; } = 101;

    // stockfish
    public string EnginePath { get; set; } = "";
    public int MoveTime { get; set; } = 2000;
    public int Strength { get; set; } = 20;
}