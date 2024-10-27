using System.Drawing;
using Chess;
using ChessGui.Engine;
using Newtonsoft.Json;
using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using StbImageSharp;
using File = System.IO.File;

namespace ChessGui;

internal abstract class Program
{
    public static readonly Dictionary<string, uint> Textures = new();
    private static GL? _gl;

    private static Position? _selectedSquare;
    public static Position? SelectedSquare
    {
        get => _selectedSquare;
        set
        {
            var moves = Board.Moves().ToList();
            var moveList = moves.ToList();
            LegalMoves = (
                from move in moveList
                where move.OriginalPosition == value
                select move).ToList();
            _selectedSquare = value;
        }
    }

    public static List<Move> LegalMoves { get; private set; } = [];

    public static ChessBoard Board { get; } = new();
    public static Config? Config { get; private set; }
    public static Stockfish? Stockfish { get; private set; }

    private static void Main()
    {
        if (!File.Exists("config.json"))
            File.WriteAllText("config.json", JsonConvert.SerializeObject(new Config(), Formatting.Indented));

        Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"))!;

        Stockfish = new Stockfish(Environment.ExpandEnvironmentVariables(Config.EnginePath));

        using var window = Window.Create(WindowOptions.Default);

        // Declare some variables
        ImGuiController controller = null!;
        IInputContext inputContext = null!;

        // Our loading function
        window.Load += () =>
        {
            controller = new ImGuiController(
                _gl = window.CreateOpenGL(), // load OpenGL
                window, // pass in our window
                inputContext = window.CreateInput() // create an input context
            );
            LoadPieceTextures();
        };

        // Handle resizes
        window.FramebufferResize += s =>
        {
            // Adjust the viewport to the new window size
            _gl!.Viewport(s);
        };

        // The render function
        window.Render += delta =>
        {
            // Make sure ImGui is up-to-date
            controller.Update((float)delta);

            // This is where you'll do any rendering beneath the ImGui context
            // Here, we just have a blank screen.
            _gl!.ClearColor(Color.FromArgb(255, (int)(.45f * 255), (int)(.55f * 255), (int)(.60f * 255)));
            _gl!.Clear((uint)ClearBufferMask.ColorBufferBit);

            // This is where you'll do all of your ImGui rendering
            Ui.Render();

            // Make sure ImGui renders too!
            controller.Render();
        };

        // The closing function
        window.Closing += () =>
        {
            // Dispose our controller first
            controller.Dispose();

            // Dispose the input context
            inputContext.Dispose();

            // Unload OpenGL
            _gl?.Dispose();
        };

        // Now that everything's defined, let's run this bad boy!
        window.Run();
    }

    private static void LoadPieceTextures()
    {
        Textures["P"] = LoadTexture("textures/pieces/wp.png");
        Textures["p"] = LoadTexture("textures/pieces/bp.png");
        Textures["R"] = LoadTexture("textures/pieces/wr.png");
        Textures["r"] = LoadTexture("textures/pieces/br.png");
        Textures["N"] = LoadTexture("textures/pieces/wn.png");
        Textures["n"] = LoadTexture("textures/pieces/bn.png");
        Textures["B"] = LoadTexture("textures/pieces/wb.png");
        Textures["b"] = LoadTexture("textures/pieces/bb.png");
        Textures["Q"] = LoadTexture("textures/pieces/wq.png");
        Textures["q"] = LoadTexture("textures/pieces/bq.png");
        Textures["K"] = LoadTexture("textures/pieces/wk.png");
        Textures["k"] = LoadTexture("textures/pieces/bk.png");
        Textures[""] = LoadTexture("textures/pieces/empty.png");
    }

    private static unsafe uint LoadTexture(string piecesWkPng)
    {
        var texture = _gl!.GenTexture();
        _gl.ActiveTexture(TextureUnit.Texture0);
        _gl.BindTexture(TextureTarget.Texture2D, texture);

        var result = ImageResult.FromMemory(File.ReadAllBytes(piecesWkPng), ColorComponents.RedGreenBlueAlpha);

        fixed (byte* ptr = result.Data)
        {
            _gl.TexImage2D(TextureTarget.Texture2D, 0, InternalFormat.Rgba, (uint)result.Width,
                (uint)result.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, ptr);
        }

        _gl.TextureParameter(texture, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        _gl.TextureParameter(texture, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        _gl.TextureParameter(texture, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        _gl.TextureParameter(texture, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

        _gl.GenerateMipmap(TextureTarget.Texture2D);

        _gl.BindTexture(TextureTarget.Texture2D, 0);

        _gl.Enable(EnableCap.Blend);
        _gl.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
        return texture;
    }
}