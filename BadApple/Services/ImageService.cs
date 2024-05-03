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
        Bitmap imageGreyScale = new Bitmap(image.Width, image.Height);

        // Iterate over every pixel position
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                Color originalColor = image.GetPixel(x, y);
                int r = originalColor.R;
                int g = originalColor.G;
                int b = originalColor.B;

                // Calculate greyscale value using weighted average
                int avg = (int)(0.299 * r + 0.587 * g + 0.114 * b);

                imageGreyScale.SetPixel(x, y, Color.FromArgb(avg, avg, avg)); // Update the pixel color of greyScaleImage 
            }
        }

        return imageGreyScale;
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
    public static Stack<Bitmap> LoadImages(string FolderPath)
    {
        Stack<Bitmap> images = new();
        string[] ImagesRAW = Directory.GetFiles(FolderPath);
        foreach (var imgRaw in ImagesRAW) 
        {
            Bitmap image = new(imgRaw);
            images.Push(image);

        }
        return images;
    }


}

