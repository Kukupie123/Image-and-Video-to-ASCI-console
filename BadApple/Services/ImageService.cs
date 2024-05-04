using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class ImageService
{
    /// <summary>
    /// Converts a given image to greyscale using OpenCV.
    /// </summary>
    /// <param name="image">The input image.</param>
    /// <returns>The greyscale version of the input image.</returns>
    public static Bitmap ConvertImageToGreyscale(Bitmap image)
    {
        Mat src = BitmapConverter.ToMat(image);
        Mat gray = new Mat();
        Cv2.CvtColor(src, gray, ColorConversionCodes.RGBA2GRAY);
        return BitmapConverter.ToBitmap(gray);
    }

    /// <summary>
    /// Converts a stack of images to greyscale using OpenCV.
    /// </summary>
    /// <param name="images">The stack of input images.</param>
    /// <returns>A stack of greyscale images.</returns>
    public static Stack<Bitmap> ConvertImagesToGreyscale(Stack<Bitmap> images)
    {
        Stack<Bitmap> greyImages = new Stack<Bitmap>();
        foreach (Bitmap image in images)
        {
            greyImages.Push(ConvertImageToGreyscale(image));
        }
        return greyImages;
    }

    /// <summary>
    /// Loads all the images in the given folder path.
    /// </summary>
    /// <param name="folderPath">The path to the folder containing images.</param>
    /// <returns>A stack of loaded images.</returns>
    public static Stack<Bitmap> LoadImages(string folderPath)
    {
        Stack<Bitmap> images = new Stack<Bitmap>();
        string[] imagePaths = Directory.GetFiles(folderPath);

        // Sort the image file paths based on their names
        Array.Sort(imagePaths, StringComparer.InvariantCulture);

        // Reverse the sorted image paths to load images in correct order
        Array.Reverse(imagePaths);

        foreach (var imagePath in imagePaths)
        {
            Console.WriteLine("Loading " + imagePath);
            Bitmap image = new Bitmap(imagePath);
            images.Push(image);
        }

        return images;
    }

}
