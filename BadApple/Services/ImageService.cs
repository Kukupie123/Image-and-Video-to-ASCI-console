using OpenCvSharp.Extensions;
using OpenCvSharp;
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
    /// THIS IS SOOOO SLOWWWWW
    /// </summary>
    /// <param name="Image"></param>
    /// <returns></returns>
    public static Bitmap ConvertImageToGreyscale(Bitmap image)
    {
        // Convert Bitmap to Mat (OpenCV format)
        Mat src = BitmapConverter.ToMat(image);

        // Convert the image to grayscale
        Mat gray = new Mat();
        Cv2.CvtColor(src, gray, ColorConversionCodes.BGR2GRAY);

        // Convert Mat back to Bitmap
        Bitmap result = BitmapConverter.ToBitmap(gray);

        return result;
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

