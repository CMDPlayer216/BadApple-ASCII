using System.Diagnostics;
using static BadApple.ConsoleHelper;

namespace BadApple;

public static class Program
{
    public static void Main()
    {
        string[] frames = Directory.GetFiles("txt_frames");
        Array.Sort(frames); // Asegura el orden correcto de los frames

        if (frames.Length == 0) return;

        Console.CursorVisible = false;
        Console.Clear();

        // 1. Iniciar la reproducción de audio en segundo plano
        Process? audioProcess = null;
        if (File.Exists("audio.wav"))
        {
            audioProcess = Process.Start(new ProcessStartInfo
            {
                FileName = "ffplay", // También puedes usar "mpv" o "pw-play"
                Arguments = "-nodisp -autoexit -loglevel quiet audio.wav",
                UseShellExecute = false,
                CreateNoWindow = true
            });
        }

        // 2. Bucle de animación (30 FPS -> ~33ms por frame)
        int delayMs = 33;

        for (int frame = 0; frame < frames.Length; frame++)
        {
            var frameStart = DateTime.UtcNow;

            // Volver cursor al origen para evitar parpadeo
            Console.SetCursorPosition(0, 0);

            // Dibujar frame
            Console.Write(File.ReadAllText(frames[frame]));

            // Dibujar barra de progreso
            int percent = (frame * 100) / frames.Length;
            string progress = "[" + new string('#', percent / 2) + new string(' ', 50 - (percent / 2)) + "]";
            DrawText($"\n{percent}% {progress}", Color.Cyan);

            // Sincronización de tiempo
            int elapsed = (int)(DateTime.UtcNow - frameStart).TotalMilliseconds;
            int sleepTime = delayMs - elapsed;

            if (sleepTime > 0)
                Thread.Sleep(sleepTime);
        }

        // Limpieza al finalizar
        if (audioProcess is { HasExited: false })
        {
            audioProcess.Kill();
        }

        Console.CursorVisible = true;

        Random rand = new Random();
        string[] splashTexts = File.ReadAllLines("splash-texts.txt");

        DrawText(splashTexts[rand.Next(0, splashTexts.Length)]);
    }
}