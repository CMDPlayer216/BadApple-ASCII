using System.Diagnostics;
using static BadApple.ConsoleHelper;

namespace BadApple;

public static class Program
{
    public static void Main(string[] args)
    {
        if ((args.Contains("-h") || args.Contains("--help")))
        {
            DrawText("Uso: ");
            DrawText(" -h,      --help                   Muestra essta información");
            DrawText("          --just-splash            Solo muestra el splashtext");
            DrawText("          --no-audio               No reproduce el audio");
            DrawText(("         --concurrent-splashes    Muestra splashtexts durante la reproducción"));
            return;
        }

        if (!args.Contains("--just-splash"))
        {
            Process? audioProcess = null;

            if (!args.Contains("--no-audio"))
            {
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
            }
            // 1. Iniciar la reproducción de audio en segundo plano


            string[] frames = Directory.GetFiles("txt_frames");
            Array.Sort(frames); // Asegura el orden correcto de los frames

            if (frames.Length == 0) return;

            Console.CursorVisible = false;
            Console.Clear();
            // 2. Bucle de animación (30 FPS -> ~33ms por frame)
            int delayMs = 33;
            int splashDelayMs = 200;

            bool concurrentSplashes = args.Contains("--concurrent-splashes");

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
                if (concurrentSplashes)
                {
                    Random r = new Random();
                    string[] splashes = File.ReadAllLines("splash-texts.txt");
                    if (frame % splashDelayMs == 0 || frame == 0) DrawText($"{splashes[r.Next(0, splashes.Length)]}                                                                                                      ");
                }

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
        }

        Random rand = new Random();
        string[] splashTexts = File.ReadAllLines("splash-texts.txt");

        Process? lolcat = Process.Start(new ProcessStartInfo
        {
            FileName = "/bin/bash",
            Arguments = $"-c \"toilet -t '{splashTexts[rand.Next(0, splashTexts.Length)]}' | lolcat\"",
            UseShellExecute = true,
            CreateNoWindow = false
        });
    }
}