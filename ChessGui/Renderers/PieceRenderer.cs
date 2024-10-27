using System.Numerics;
using Chess;
using ImGuiNET;

namespace ChessGui.Renderers;

public class PieceRenderer
{
    private static readonly int SquareSize = Program.Config!.SquareSize;

    public static void Render(Piece? piece, int file, int rank)
    {
        var pieceString = "";
        if (piece != null)
        {
            if (piece.Color == PieceColor.Black || piece.Color == PieceColor.White)
            {
                pieceString = piece.ToFenChar().ToString();
            }
        }

        var currentPosition = new Position((short)(file - 1), (short)(rank - 1));

        ImGui.SetCursorScreenPos(new Vector2(file * SquareSize, (8 - rank + 1) * SquareSize));
        ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0, 0));

        PushButtonStyles(currentPosition);
        if (ImGui.ImageButton("##" + pieceString + rank + file, (nint)Program.Textures[pieceString], new Vector2(SquareSize, SquareSize)))
        {
            if (Program.LegalMoves.Any(move => move.NewPosition == currentPosition))
            {
                Program.Board.Move(Program.LegalMoves.First(move => move.NewPosition == currentPosition));
                var thread = new Thread(() =>
                {
                    Program.Stockfish!.SetFenPosition(Program.Board.ToFen());
                    var move = Program.Stockfish.GetBestMove();
                    Program.Board.Move(move);
                });
                thread.Start();
            }
            Program.SelectedSquare = currentPosition;
        }

        foreach (var move in Program.LegalMoves.TakeWhile(move => move.OriginalPosition == currentPosition))
        {
            if (Program.Board.Turn == PieceColor.Black) continue;
            ImGui.GetForegroundDrawList().AddCircleFilled(
                new Vector2(
                    move.NewPosition.X * SquareSize + SquareSize * 1.5f,
                    (7 - move.NewPosition.Y) * SquareSize + SquareSize * 1.5f),
                10,
                0xFF0000FF);
            // TODO: Calculate correct color for circle
        }

        ImGui.PopStyleVar();
        ImGui.PopStyleColor(3);
    }

    private static void PushButtonStyles(Position currentSquare)
    {
        if (
            (Program.Board.WhiteKingChecked && currentSquare == Program.Board.WhiteKing) ||
            (Program.Board.BlackKingChecked && currentSquare == Program.Board.BlackKing)
        )
        {
            ImGui.PushStyleColor(ImGuiCol.Button, Program.Config!.CheckedColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, Program.Config!.CheckedColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, Program.Config!.CheckedColor);
            return;
        }
        if (currentSquare == Program.SelectedSquare)
        {
            ImGui.PushStyleColor(ImGuiCol.Button, Program.Config!.SelectColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonActive, Program.Config!.SelectColor);
            ImGui.PushStyleColor(ImGuiCol.ButtonHovered, Program.Config!.SelectColor);
            return;
        }
        ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0f, 0f, 0f, 0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonActive, new Vector4(0f, 0f, 0f, 0f));
        ImGui.PushStyleColor(ImGuiCol.ButtonHovered, new Vector4(0f, 0f, 0f, 0f));
    }
}