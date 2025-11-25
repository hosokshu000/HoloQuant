from PIL import Image, ImageFilter
import os
import argparse

def sharpen_images(input_folder, output_folder):
    # Create the output folder if it doesn't exist
    if not os.path.exists(output_folder):
        os.makedirs(output_folder)

    # List all files in the input folder
    for filename in os.listdir(input_folder):
        if filename.lower().endswith('.png'):
            # Open an image file
            with Image.open(os.path.join(input_folder, filename)) as img:
                # Apply sharpening filter
                sharpened_img = img.filter(ImageFilter.SHARPEN)
                
                # Save the sharpened image to the output folder
                sharpened_img.save(os.path.join(output_folder, filename))
                print(f"Processed and saved {filename}")

def main():
    parser = argparse.ArgumentParser(description='Sharpen PNG images in a folder.')
    parser.add_argument('input_folder', type=str, help='Path to the input folder containing PNG images')
    parser.add_argument('output_folder', type=str, help='Path to the output folder where sharpened images will be saved')
    
    args = parser.parse_args()
    
    sharpen_images(args.input_folder, args.output_folder)

if __name__ == '__main__':
    main()
