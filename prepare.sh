#!/bin/bash

echo "====== Detectando gestor de paquetes e instalando dependencias ======"

check_and_install() {
    local cmd="$1"
    local pkg="$2"
    local manager="$3"

    if command -v "$cmd" >/dev/null 2>&1; then
        echo "   -> $cmd ya está instalado, no se requiere reinstalar."
    else
        echo "   -> $cmd no encontrado. Instalando $pkg..."
        case "$manager" in
            pacman) sudo pacman -S --noconfirm "$pkg" ;;
            apt)    sudo apt install -y "$pkg" ;;
            dnf)    sudo dnf install -y "$pkg" ;;
        esac
    fi
}

check_python_pillow() {
    local pkg="$1"
    local manager="$2"

    if python3 -c "import PIL" >/dev/null 2>&1; then
        echo "   -> python-pillow ya está instalado, no se requiere reinstalar."
    else
        echo "   -> python-pillow no encontrado. Instalando..."
        case "$manager" in
            pacman) sudo pacman -S --noconfirm "$pkg" ;;
            apt)    sudo apt install -y "$pkg" ;;
            dnf)    sudo dnf install -y "$pkg" ;;
        esac
    fi
}

if command -v pacman >/dev/null 2>&1; then
    echo "Detectado: Arch Linux (pacman)"
    check_and_install "ffmpeg" "ffmpeg" "pacman"
    check_and_install "dotnet" "dotnet-sdk" "pacman"
    check_and_install "python" "python" "pacman"
    check_and_install "lolcat" "lolcat" "pacman"
    check_and_install "toilet" "toilet" "pacman"
    check_python_pillow "python-pillow" "pacman"

elif command -v apt >/dev/null 2>&1; then
    echo "Detectado: Debian / Ubuntu (apt)"
    sudo apt update
    check_and_install "ffmpeg" "ffmpeg" "apt"
    check_and_install "dotnet" "dotnet-sdk-8.0" "apt"
    check_and_install "python3" "python3" "apt"
    check_and_install "lolcat" "lolcat" "apt"
    check_and_install "toilet" "toilet" "apt"
    check_python_pillow "python3-pillow" "apt"

elif command -v dnf >/dev/null 2>&1; then
    echo "Detectado: Fedora / RHEL (dnf)"
    check_and_install "ffmpeg" "ffmpeg" "dnf"
    check_and_install "dotnet" "dotnet-sdk-8.0" "dnf"
    check_and_install "python3" "python3" "dnf"
    check_and_install "lolcat" "lolcat" "dnf"
    check_and_install "toilet" "toilet" "dnf"
    check_python_pillow "python3-pillow" "dnf"

else
    echo "No se detectó un gestor compatible. Por favor instala manualmente las dependencias."
    exit 1
fi

echo "====== Proceso de dependencias finalizado ======"

echo "====== Separando frames ======"
mkdir -p frames
ffmpeg -i BadApple.mp4 -vf "fps=30,scale=80:30:flags=neighbor" -q:v 2 frames/frame_%04d.png
mkdir -p txt_frames
python FramesToASCII.py
echo "====== Procesando audio ======"
ffmpeg -i BadApple.mp4 audio.wav -y

if [ "$1" == "--execute" ]; then
    echo "====== Ejecutando ======"
    dotnet run
fi