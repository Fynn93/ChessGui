using System.Numerics;
using ChessGui.Chess;
using ImGuiNET;

namespace ChessGui.Components;

public class PieceRenderer
{
    public static void Render(Piece piece, int y, int x)
    {
        var pieceString = piece.ToString();

        if (string.IsNullOrEmpty(pieceString))
        {
            return;
        }

        ImGui.GetWindowDrawList().AddImage((nint)Program.PieceTextures[pieceString], new Vector2(x * 50, y * 50), new Vector2(x * 50 + 50, y * 50 + 50));
        ImGui.SetCursorScreenPos(new Vector2(x * 50, y * 50));
        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0, 0));
        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0f, 0f, 0f, 0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0f, 0f, 0f, 0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0f, 0f, 0f, 0f));

        if (ImGui.ImageButton("##" + pieceString + x + y, (nint)Program.PieceTextures[pieceString], new Vector2(50, 50)))
        {
            Program.SelectedPiece = piece;
        }

        ImGui.PopStyleVar();
        ImGui.PopStyleColor(3);
    }
}