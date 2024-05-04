using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

public class VideoService
{

    //Generates images from videos and prints them directly
    public static Stack<Bitmap> GenerateImagesFromVideo(string videoFilePath, OpenCvSharp.Size size)
    {


        return LoadImagesFromVideo(videoFilePath, size);
    }

    //Extracts images of video per frame in given output with given size
    public static void ExtractImagesFromVideo(string videoFilePath, string outputFolderPath, OpenCvSharp.Size size)
    {

        // Call the common function to load images from video
        var images = GenerateImagesFromVideo(videoFilePath, size);

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
                    return MathF.Max((float)(fps - 14), 10);
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
            Console.WriteLine("Saving Frame number " + frameNumber);
            // Save the bitmap image as a file
            string outputFilePath = Path.Combine(outputFolderPath, $"frame_{frameNumber:D6}.jpg");
            bitmap.Save(outputFilePath);
            frameNumber++;
        }
    }

    private static Stack<Bitmap> LoadImagesFromVideo(string videoFilePath, OpenCvSharp.Size newSize)
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

            int frameCount = 0;

            // Loop through each frame of the video
            Mat frame = new Mat();
            while (videoCapture.Read(frame))
            {
                // Resize the frame
                Cv2.Resize(frame, frame, newSize);

                // Convert the frame to greyscale
                Cv2.CvtColor(frame, frame, ColorConversionCodes.RGBA2GRAY);

                // Convert the OpenCV Mat to a bitmap
                Bitmap bitmap = BitmapConverter.ToBitmap(frame);

                // Insert the bitmap image at the beginning of the stack
                images.Push(bitmap);
                Console.WriteLine("Added Bitmap " + frameCount);

                frameCount++;
            }
        }

        // Reversing the stack to maintain the original order of frames
        Stack<Bitmap> reversedImages = new Stack<Bitmap>(images.Count);
        while (images.Count > 0)
        {
            reversedImages.Push(images.Pop());
        }

        return reversedImages;
    }






}
