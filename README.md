# BadApple-ASCII
### Simple reproductor de BadApple
---
## Uso
Para usarlo, deberás poner el video de **BadApple!!** en la carpeta del proyecto posterior a clonarlo con el nombre **BadApple.mp4**
### Ejecución sencilla
```bash
chmod +x prepare.sh
./prepare.sh --execute
```
El script instala todas las dependencias (incluyendo el SDK de .NET) y crea todos los archivos y carpetas necesarios en base al archivo `BadApple.mp4`.
### Ejecución con argumentos
Primero preparemos el entorno:
```bash
chmod +x prepare.sh
./prepare.sh
```

Ahora ejecutamos:
```bash
dotnet run -- [argumentos]
```
## Opciones disponibles
```text
-h,         --help                    Muestra ayuda
            --just-splash             Solo muestra un splashtext
            --no-audio                No reproduce el audio
            --concurrent-splashes     Muestra splashtexts durante la reproducción
```
