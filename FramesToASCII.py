import os
from PIL import Image

# Rampa de caracteres de oscuro a claro (o simple para B&N)
# Para Bad Apple un simple espacio ' ' y '#' o '@' suele bastar
ASCII_CHARS = " .:-=+*#%@"

def image_to_ascii(image_path):
    img = Image.open(image_path).convert('L') # Convertir a escala de grises
    pixels = img.getdata()
    
    ascii_str = ""
    for i, pixel in enumerate(pixels):
        # Mapea el valor de 0-255 al índice de la rampa ASCII
        ascii_str += ASCII_CHARS[pixel * len(ASCII_CHARS) // 256]
        if (i + 1) % img.width == 0:
            ascii_str += "\n"
            
    return ascii_str

def main():
    input_dir = "frames"
    output_dir = "txt_frames"
    os.makedirs(output_dir, exist_ok=True)

    files = sorted([f for f in os.listdir(input_dir) if f.endswith('.png')])
    print(f"Procesando {len(files)} frames...")

    for file in files:
        img_path = os.path.join(input_dir, file)
        ascii_frame = image_to_ascii(img_path)
        
        txt_name = os.path.splitext(file)[0] + ".txt"
        with open(os.path.join(output_dir, txt_name), "w") as f:
            f.write(ascii_frame)

    print("¡Frames ASCII generados con éxito!")

if __name__ == "__main__":
    main()