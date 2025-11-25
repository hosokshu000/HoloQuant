import os
import argparse
import cv2
from concurrent.futures import ProcessPoolExecutor
import multiprocessing

def process_image(input_folder, output_folder, filename, mode):
    print(f"Processing {filename}...")
    try:
        # Open image using OpenCV
        img = cv2.imread(os.path.join(input_folder, filename), cv2.IMREAD_UNCHANGED)

        if img is None:
            print(f"Error reading {filename}, skipping.")
            return

        # Check if the image has an alpha channel
        if img.shape[2] == 4:
            b, g, r, a = cv2.split(img)  # Extract channels
        else:
            print(f"{filename} does not have an alpha channel, skipping.")
            return

        if mode == "bgm":  # Background removal
            # Create a mask for all black pixels
            black_mask = (r == 0) & (g == 0) & (b == 0)

            # Set the alpha channel to 0 for black pixels
            a[black_mask] = 0
        elif mode == "bgc":  # Background removal
            # Create a mask for all non-white pixels
            black_mask = (r != 255) & (g != 255) & (b != 255)

            # Set the alpha channel to 0 for black pixels
            a[black_mask] = 0
        elif mode == "s":  # Shadow creation
            # Mask for all non-transparent pixels
            mask = (a != 0)

            # Set them to gray
            gray_value = 70
            shadow_transparency = 180
            b[mask] = g[mask] = r[mask] = gray_value
            a[mask] = shadow_transparency
        else:
            print(f"Invalid mode for {filename}.")
            return

        # Merge channels back and save the image
        processed_img = cv2.merge([b, g, r, a])
        cv2.imwrite(os.path.join(output_folder, filename), processed_img)

    except Exception as e:
        print(f"Error processing {filename}: {e}")

def process(input_folder, output_folder, mode):
    print(f"Process started with input: {input_folder}, output: {output_folder}, mode: {mode}")
    
    # Ensure the output folder exists
    os.makedirs(output_folder, exist_ok=True)

    # Sort files for orderly image processing
    filenames = sorted([f for f in os.listdir(input_folder) if f.endswith(".png")])

    if not filenames:
        print("No PNG files found in the input folder.")
        return  # Exit if there are no files

    print(f"Found {len(filenames)} PNG files to process.")
    
    # Use parallel processing
    num_workers = multiprocessing.cpu_count()

    with ProcessPoolExecutor(max_workers=num_workers) as executor:
        for filename in filenames:
            input_file_path = os.path.join(input_folder, filename)
            # Check if the file is a symlink
            if os.path.islink(input_file_path):
                print(f"Skipping symlink: {filename}")
                continue
            
            executor.submit(process_image, input_folder, output_folder, filename, mode)

if __name__ == "__main__":
    # Set up argument parser for input/output folders and mode
    parser = argparse.ArgumentParser(description="Process PNG images in a folder.")
    parser.add_argument("input_folder", help="Path to the input folder containing PNG images.")
    parser.add_argument("output_folder", help="Path to the output folder to save the modified images.")
    parser.add_argument("mode", choices=['bgm', 'bgc', 's'], help="Mode of the processor: 'bgm' for manim background removal, 'bgc' for canva background removal, 's' for shadow creation.")

    args = parser.parse_args()

    # Call the process function with arguments from the command line
    process(args.input_folder, args.output_folder, args.mode)