using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ImageService
{

    /// <summary>
    /// Returns a Greyscale image by calculating the average
    /// </summary>
    /// <param name="Image"></param>
    /// <returns></returns>
    public static Bitmap ConvertImageToGreyscale(Bitmap image)
    {
        Bitmap imageGreyscale = new Bitmap(image.Width, image.Height);

        // Define parameters for adjusting brightness and contrast
        double brightness = 0.1; // Adjust this value to change brightness
        double contrast = 1.5;   // Adjust this value to change contrast

        // Iterate over every pixel position
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                Color originalColor = image.GetPixel(x, y);
                double r = originalColor.R / 255.0; // Normalize to range [0, 1]
                double g = originalColor.G / 255.0;
                double b = originalColor.B / 255.0;

                // Apply gamma correction for each color channel to adjust brightness
                r = Math.Pow(r, contrast) * brightness;
                g = Math.Pow(g, contrast) * brightness;
                b = Math.Pow(b, contrast) * brightness;

                // Ensure values are in valid range [0, 1]
                r = Math.Max(0, Math.Min(1, r));
                g = Math.Max(0, Math.Min(1, g));
                b = Math.Max(0, Math.Min(1, b));

                // Convert back to [0, 255] range
                int avg = (int)(255 * (0.2126 * r + 0.7152 * g + 0.0722 * b));

                imageGreyscale.SetPixel(x, y, Color.FromArgb(avg, avg, avg)); // Update the pixel color of greyScaleImage 
            }
        }

        return imageGreyscale;
    }


    public static Stack<Bitmap> ConvertImagesToGreyScale(Stack<Bitmap> Images)
    {
        var greyImg = new Stack<Bitmap>();
        foreach (Bitmap Image in Images)
        {
            greyImg.Push(ConvertImageToGreyscale(Image));
        }
        return greyImg;
    }

    /// <summary>
    /// Loads all the images in the given path. Make sure that the folder has ONLY images
    /// </summary>
    /// <param name="FolderPath"></param>
    /// <returns></returns>
    public static Stack<Bitmap> LoadImages(string folderPath)
    {
        Stack<Bitmap> images = new Stack<Bitmap>();
        string[] imagePaths = Directory.GetFiles(folderPath);

        // Sort the image file paths based on their names
        Array.Sort(imagePaths, StringComparer.InvariantCulture);

        foreach (var imagePath in imagePaths)
        {
            Bitmap image = new Bitmap(imagePath);
            images.Push(image);
        }

        return images;
    }




}

