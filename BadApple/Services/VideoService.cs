using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class VideoService
{

    //Generates images from videos and prints them directly
    public static Stack<Bitmap> GenerateImagesFromVideo(string videoFilePath)
    {


        return LoadImagesFromVideo(videoFilePath);
    }

    //Extracts images of video per frame in given output with given size
    public static void ExtractImagesFromVideo(string videoFilePath, string outputFolderPath)
    {

        // Call the common function to load images from video
        var images = GenerateImagesFromVideo(videoFilePath);

        // Save the images to the output folder
        SaveImages(images, outputFolderPath);

        // Construct the file path for the FPS file
        string parentFolderPath = Directory.GetParent(outputFolderPath)?.FullName ?? throw new ArgumentNullException("Parent folder path cannot be null.");
        string fpsFilePath = Path.Combine(parentFolderPath, "fps.txt");

        // Save the FPS information to the FPS file
        File.WriteAllText(fpsFilePath, (new VideoCapture(videoFilePath).Fps - 14).ToString());

    }

    public static double GetFPSofExportedFrames(string outputFolderPath)
    {
        double fps = 0.0;

        // Get the parent folder path
        string parentFolderPath = Directory.GetParent(outputFolderPath).FullName;

        if (parentFolderPath != null)
        {
            // Construct the file path for the FPS file in the parent folder
            string fpsFilePath = Path.Combine(parentFolderPath, "fps.txt");

            // Check if the FPS file exists
            if (File.Exists(fpsFilePath))
            {
                // Read the FPS file content
                string content = File.ReadAllText(fpsFilePath);

                // Attempt to parse the FPS value
                if (double.TryParse(content, out fps))
                {
                    return fps;
                }
            }
        }

        return fps;
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

    private static Stack<Bitmap> LoadImagesFromVideo(string videoFilePath)
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

            // Adjust decoding parameters for speed (optional)
            videoCapture.Set(VideoCaptureProperties.Fps, 30); // Set desired frame rate
            videoCapture.Set(VideoCaptureProperties.FrameHeight, 100); // Set desired frame height
            videoCapture.Set(VideoCaptureProperties.FrameWidth, 100); // Set desired frame width

            // Frame skipping (optional)
            int frameSkip = 0; // Skip every nth frame
            int frameCount = 0;

            // Loop through each frame of the video
            Mat frame = new Mat();
            while (videoCapture.Read(frame))
            {
                // Apply frame skipping
                

                // Convert the OpenCV Mat to a bitmap
                Bitmap bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame);

                var greyImg = ImageService.ConvertImageToGreyscale(bitmap);

                // Push the bitmap image onto the stack
                images.Push(bitmap);
                Console.WriteLine("Added Bitmap " + frameCount);

                frameCount++;
            }
        }

        return images;
    }



}
