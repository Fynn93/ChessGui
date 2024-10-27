using System.Numerics;
using Chess;
using ImGuiNET;

namespace ChessGui.Renderers;

public class BoardRenderer
{
    private static readonly int SquareSize = Program.Config!.SquareSize;

    public static void Render(ChessBoard board)
    {
        var drawList = ImGui.GetWindowDrawList();

        for (var y = 1; y <= 8; y++)
        {
            for (var x = 1; x <= 8; x++)
            {
                var color = (y + x) % 2 == 0 ? Program.Config!.Color1 : Program.Config!.Color2;
                drawList.AddRectFilled(new Vector2(x * SquareSize, y * SquareSize), new Vector2(x * SquareSize + SquareSize, y * SquareSize + SquareSize),
                    ImGui.GetColorU32(color));
            }
        }

        for (var rank = 8; rank >= 1; rank--)
        {
            for (var file = 1; file <= 8; file++)
            {
                var piece = board[file - 1, rank - 1];
                PieceRenderer.Render(piece!, file, rank);
            }
        }
    }
}