using System.Numerics;
using ChessGui.Renderers;
using ImGuiNET;

namespace ChessGui;

public class Ui
{
    public static void Render()
    {
        ImGui.SetNextWindowSize(ImGui.GetIO().DisplaySize);
        ImGui.SetNextWindowPos(new Vector2(0, 0));

        ImGui.Begin("Board", ImGuiWindowFlags.NoTitleBar
                             | ImGuiWindowFlags.NoResize
                             | ImGuiWindowFlags.NoCollapse
                             | ImGuiWindowFlags.NoScrollbar
                             | ImGuiWindowFlags.NoScrollWithMouse);
        DisplayChessBoard();
        ImGui.End();
    }

    private static void DisplayChessBoard()
    {
        BoardRenderer.Render(Program.Board);
    }
}