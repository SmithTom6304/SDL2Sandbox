using SDL2;

namespace SDL2_Test;

public static class WindowReporting
{
    public struct Config
    {
        public bool ReportWindowPosition { get; init; }
        public bool ReportDisplayDpi { get; init; }
        public bool ReportWindowSize { get; init; }
        public bool ReportDrawableSize { get; init; }

        public static Config ReportAll { get; } = new Config()
        {
            ReportWindowPosition = true,
            ReportDisplayDpi = true,
            ReportWindowSize = true,
            ReportDrawableSize = true,
        };
    }

    public static IReadOnlyList<string> CreateWindowReport(IntPtr window, Config config)
    {
        List<string> report = new List<string>();

        if (config.ReportWindowPosition)
        {
            AddWindowPositionToReport(window, report);
        }

        if (config.ReportDisplayDpi)
        {
            var displayIndex = SDL.SDL_GetWindowDisplayIndex(window);
            AddDisplayDpiToReport(displayIndex, report);
        }

        if (config.ReportWindowSize)
        {
            AddWindowSizeToReport(window, report);
        }

        if (config.ReportDrawableSize)
        {
            AddDrawableSizeToReport(window, report);
        }
        
        return report;
    }

    private static void AddWindowPositionToReport(IntPtr window, List<string> report)
    {
        SDL.SDL_GetWindowPosition(window, out var x, out var y);
        report.Add($"{nameof(SDL.SDL_GetWindowPosition)}:");
        report.Add($"\tx: {x}");
        report.Add($"\ty: {y}");
    }
    
    private static void AddDisplayDpiToReport(int displayIndex, List<string> report)
    {
        SDL.SDL_GetDisplayDPI(displayIndex, out var dpiD, out var dpiX, out var dpiY);
        report.Add($"{nameof(SDL.SDL_GetDisplayDPI)}:");
        report.Add($"\thdpi: {dpiX}");
        report.Add($"\tvdpi: {dpiY}");
        report.Add($"\tddpi (diagonal): {dpiD}");
    }
    
    private static void AddWindowSizeToReport(IntPtr window, List<string> report)
    {
        SDL.SDL_GetWindowSize(window, out var w, out var h);
        report.Add($"{nameof(SDL.SDL_GetWindowSize)}:");
        report.Add($"\tw: {w}");
        report.Add($"\th: {h}");
    }
    
    private static void AddDrawableSizeToReport(IntPtr window, List<string> report)
    {
        SDL.SDL_GL_GetDrawableSize(window, out var dw, out var dh);
        report.Add($"{nameof(SDL.SDL_GL_GetDrawableSize)}:");
        report.Add($"\tdw: {dw}");
        report.Add($"\thh: {dh}");
    }
}