using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class VideoService
{

    public static Stack<Bitmap> GenerateImagesFromVideo(string videoFilePath, System.Drawing.Size resize)
    {
        // Define preprocess function to resize the frame
        Func<Mat, Mat> preprocess = (frame) =>
        {
            Mat resizedFrame = new Mat();
            Cv2.Resize(frame, resizedFrame, new OpenCvSharp.Size(resize.Width, resize.Height));
            return resizedFrame;
        };

        return LoadImagesFromVideo(videoFilePath, preprocess);
    }

    public static void ExtractImagesFromVideo(string videoFilePath, string outputFolderPath, System.Drawing.Size resizeSize)
    {
        // Define preprocess function to resize the frame
        Func<Mat, Mat> preprocess = (frame) =>
        {
            Mat resizedFrame = new Mat();
            Cv2.Resize(frame, resizedFrame, new OpenCvSharp.Size(resizeSize.Width, resizeSize.Height));
            return resizedFrame;
        };

        // Call the common function to load images from video
        var images = LoadImagesFromVideo(videoFilePath, preprocess);

        // Save the images to the output folder
        SaveImages(images, outputFolderPath);
    }

    private static void SaveImages(Stack<Bitmap> images, string outputFolderPath)
    {
        // Create the output folder if it doesn't exist
        Directory.CreateDirectory(outputFolderPath);

        // Delete all files in the output folder
        foreach (string filePath in Directory.GetFiles(outputFolderPath))
        {
            File.Delete(filePath);
        }

        // Save the images to the output folder
        int frameNumber = 0;
        foreach (var bitmap in images)
        {
            // Save the bitmap image as a file
            string outputFilePath = Path.Combine(outputFolderPath, $"frame_{frameNumber:D6}.jpg");
            bitmap.Save(outputFilePath);
            frameNumber++;
        }
    }

    private static Stack<Bitmap> LoadImagesFromVideo(string videoFilePath, Func<Mat, Mat> preprocess)
    {
        Stack<Bitmap> images = new Stack<Bitmap>();

        // Open the video file
        using (var videoCapture = new VideoCapture(videoFilePath))
        {
            if (!videoCapture.IsOpened())
            {
                Console.WriteLine("Failed to open the video file.");
                return images;
            }

            // Loop through each frame of the video
            Mat frame = new Mat();
            while (videoCapture.Read(frame))
            {
                // Preprocess the frame if needed
                if (preprocess != null)
                {
                    frame = preprocess(frame);
                }

                // Convert the OpenCV Mat to a bitmap
                Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);

                // Push the bitmap image onto the stack
                images.Push(bitmap);
            }
        }

        return images;
    }

}
