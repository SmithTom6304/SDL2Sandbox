// See https://aka.ms/new-console-template for more information

using SDL2_Test;
using SDL2;

internal class Program
{
    private struct Config
    {
        public bool AllowHighDpi { get; init; }
        public bool PrintKeyEvents { get; init; }
        public bool CreateWindowReport { get; init; }
        public string WindowReportPath { get; init; }
    }
    
    public static void Main(string[] args)
    {
        Console.WriteLine("Starting SDL2 sandbox");
        
        var config = new Config()
        {
            AllowHighDpi = true,
            PrintKeyEvents = false,
            CreateWindowReport = true,
            WindowReportPath = "" //"../../../../sdl2-window-report.txt" // Repo directory
        };

        if (SDL.SDL_Init((uint)SDL.SDL_INIT_VIDEO) != 0)
        {
            Console.WriteLine("Failed to initialize SDL!");
            return;
        }

        var windowFlags = SDL.SDL_WindowFlags.SDL_WINDOW_RESIZABLE;
        if(config.AllowHighDpi)
            windowFlags |= SDL.SDL_WindowFlags.SDL_WINDOW_ALLOW_HIGHDPI;
        var window = SDL.SDL_CreateWindow("SDL2 Sandbox", 100, 100, 600, 400, windowFlags);
        var renderer = SDL.SDL_CreateRenderer(window, 0, SDL.SDL_RendererFlags.SDL_RENDERER_SOFTWARE);

        bool isRunning = true;
        while (isRunning)
        {
            while (SDL.SDL_PollEvent(out var evt) != 0)
            {
                switch (evt.type)
                {
                    case SDL.SDL_EventType.SDL_QUIT:
                        isRunning = false;
                        break;
                    case SDL.SDL_EventType.SDL_KEYDOWN:
                        if (config.CreateWindowReport && evt.key.keysym.sym == SDL.SDL_Keycode.SDLK_w)
                            ReportWindowValues(window, config);
                        if(config.PrintKeyEvents)
                            PrintKeyEvent(evt.key);
                        break;
                    case SDL.SDL_EventType.SDL_KEYUP:
                        if(config.PrintKeyEvents)
                            PrintKeyEvent(evt.key);
                        break;
                }
            }
    
            Render();
        }

        return;

        void Render()
        {
            SDL.SDL_RenderClear(renderer);
            SDL.SDL_RenderPresent(renderer);
        }

        void PrintKeyEvent(SDL.SDL_KeyboardEvent keyEvent)
        {
            Console.WriteLine($"KeyEvent: {keyEvent.type}");
            Console.WriteLine($"\tKey: {keyEvent.keysym.sym.ToString()}");
            Console.WriteLine($"\tRetrievedKey: {SDL.SDL_GetKeyFromScancode(keyEvent.keysym.scancode, SDL.SDL_bool.SDL_FALSE).ToString()}");
            Console.WriteLine($"\tScancode: {keyEvent.keysym.scancode}");
            Console.WriteLine($"\tKeyName: {SDL.SDL_GetKeyName(keyEvent.keysym.sym)}");
        }

        void ReportWindowValues(IntPtr window, Config config)
        {
            var report = WindowReporting.CreateWindowReport(window, WindowReporting.Config.ReportAll);
            
            if(!string.IsNullOrEmpty(config.WindowReportPath))
                File.WriteAllLines(config.WindowReportPath, report);
            
            foreach (var line in report)
            {
                Console.WriteLine(line);
            }
        }
    }
}