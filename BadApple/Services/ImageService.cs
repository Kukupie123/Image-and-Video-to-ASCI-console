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
    public static Bitmap ConvertImageToGreyscale(Bitmap Image)
    {
        Bitmap imageGreyScale = new(Image.Width, Image.Height);

        //Iterate every pixel position
        for (int x = 0; x < Image.Width; x++)
        {
            for (int y = 0; y < Image.Height; y++)
            {
                Color originalColor = Image.GetPixel(x, y);
                int r = originalColor.R;
                int g = originalColor.G;
                int b = originalColor.B;
                int avg = (r + g + b) / 3;
                imageGreyScale.SetPixel(x, y, color: Color.FromArgb(avg, avg, avg)); //Update the pixel color of greyScaleImage 
            }
        }

        return imageGreyScale;

    }

    /// <summary>
    /// Loads all the images in the given path. Make sure that the folder has ONLY images
    /// </summary>
    /// <param name="FolderPath"></param>
    /// <returns></returns>
    public static Stack<Bitmap> LoadImages(string FolderPath)
    {
        Stack<Bitmap> images = new Stack<Bitmap>();
        string[] ImagesRAW = Directory.GetFiles(FolderPath);


        foreach (var imgRaw in ImagesRAW.Reverse()) //Need to reverse because we want the First frame to be on top and last frame on top
        {
            Bitmap image = new Bitmap(imgRaw);
            images.Push(image);

        }
        return images;
    }


}

