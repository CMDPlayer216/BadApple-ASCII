# BadApple-ASCII
### Simple reproductor de BadApple
---
## Uso
Para usarlo, deberás poner el video de **BadApple!!** en la carpeta del proyecto posterior a clonarlo con el nombre **BadApple.mp4**
### Ejecución sencilla (primera ejecución)
```bash
chmod +x execute.sh
./execute.sh
```
### Ejecución con argumentos
#### a. Si ya has ejecutado `execute.sh`:
```bash
dotnet run -- [Argumentos]
```
### b. Si no lo has ejecutado:
Preparación:
```bash
# Instalar dependencias
# Usar sudo apt install en Debian/Ubuntu o derivados
# Usar sudo dnf install en Fedora o derivados
sudo pacman -S ffmpeg dotnet-sdk python lolcat toilet --needed
# Crear carpeta de imágenes
mkdir -p frames
# Procesar video a imágenes (puede tardar dependiendo de tu dispositivo)
ffmpeg -i BadApple.mp4 -vf "fps=30,scale=80:30:flags=neighbor" -q:v 2 frames/frame_%04d.png
# Crear carpeta de imágenes en ASCII
mkdir -p txt_frames
# Procesar imágenes a ASCII
python FramesToASCII.py
# Extraer audio de BadApple.mp4
ffmpeg -i BadApple.mp4 audio.wav
```
Ejecución:
```bash
dotnet run -- [Argumentos]
```
---
## Opciones disponibles
```text
-h,         --help                    Muestra ayuda
            --just-splash             Solo muestra un splashtext
            --no-audio                No reproduce el audio
            --concurrent-splashes     Muestra splashtexts durante la reproducción
```
