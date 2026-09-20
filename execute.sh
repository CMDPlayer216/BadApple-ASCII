#!/bin/bash

echo "====== Instalando dependencias ======"
command -v ffmpeg >/dev/null 2>&1 || sudo pacman -S ffmpeg --no-confirm
command -v dotnet >/dev/null 2>&1 || sudo pacman -S dotnet-sdk --no-confirm
command -v python >/dev/null 2>&1 || sudo pacman -S python --no-confirm
command -v lolcat >/dev/null 2>&1 || sudo pacman -S lolcat --no-confirm
command -v toilet >/dev/null 2>&1 || sudo pacman -S toilet --no-confirm
echo "====== Separando frames ======"
mkdir -p frames
ffmpeg -i BadApple.mp4 -vf "fps=30,scale=80:30:flags=neighbor" -q:v 2 frames/frame_%04d.png
mkdir -p txt_frames
python FramesToASCII.py
echo "====== Procesando audio ======"
ffmpeg -i BadApple.mp4 audio.wav
echo "====== Ejecutando ======"
dotnet run