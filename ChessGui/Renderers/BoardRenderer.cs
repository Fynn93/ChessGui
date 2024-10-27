using System.Numerics;
using ChessGui.Chess;
using ImGuiNET;

namespace ChessGui.Components;

public class BoardRenderer
{
    public static void Render(Board board)
    {
        var drawList = ImGui.GetWindowDrawList();
        for (var y = 1; y <= 8; y++)
        {
            for (var x = 1; x <= 8; x++)
            {
                var color = (y + x) % 2 == 0 ? new Vector4(0xeb / 255.0f, 0xec / 255.0f, 0xd0 / 255.0f, 1.0f) : new Vector4(0x73 / 255.0f, 0x95 / 255.0f, 0x52 / 255.0f, 1.0f);
                drawList.AddRectFilled(new Vector2(y * 50, x * 50), new Vector2(y * 50 + 50, x * 50 + 50), ImGui.GetColorU32(color));
            }
        }
        for (var y = 1; y <= 8; y++)
        {
            for (var x = 1; x <= 8; x++)
            {
                var piece = board[x - 1, y - 1];
                PieceRenderer.Render(piece, x, y);
            }
        }
    }
}