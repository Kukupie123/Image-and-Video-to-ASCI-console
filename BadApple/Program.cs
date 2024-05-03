using System;
using System.Drawing;
using System.IO;

class Program
{
    static void Main()
    {
        while (true)
        {
            // Display the menu options
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Load a video directly");
            Console.WriteLine("2. Extract frames from video");
            Console.WriteLine("3. Load images from folder directly");
            Console.WriteLine("4. Exit");

            // Prompt user for input
            Console.Write("Enter your choice: ");
            string input = Console.ReadLine();

            // Parse user input
            if (int.TryParse(input, out int choice))
            {
                // Process user choice
                switch (choice)
                {
                    case 1:
                        // Load a video directly
                        Console.WriteLine("Enter the path of the video file (e.g., './video.mp4'):");
                        string path = Console.ReadLine();

                        // Default video path
                        if (string.IsNullOrEmpty(path))
                        {
                            path = "./video.mp4";
                        }

                        // Ask for the size
                        Size size1 = GetImageSize();

                        // Generate images from the video
                        var videoImages = VideoService.GenerateImagesFromVideo(path, size1);

                        ConsoleService.PrintImagesToConsole(videoImages);
                        break;

                    case 2:
                        // Extract frames from video
                        Console.WriteLine("Enter the path of the video file (e.g., './video.mp4'):");
                        string videoFilePath = Console.ReadLine();

                        // Default video path
                        if (string.IsNullOrEmpty(videoFilePath))
                        {
                            videoFilePath = "./video.mp4";
                        }

                        // Ask for the output folder path
                        Console.WriteLine("Enter the output folder path for exported frames (e.g., './exportedFrames'):");
                        string outputFolderPath = Console.ReadLine();

                        // Default output folder path
                        if (string.IsNullOrEmpty(outputFolderPath))
                        {
                            outputFolderPath = "./exportedFrames";
                        }

                        // Ask for the size
                        Size size2 = GetImageSize();

                        // Extract frames from the video
                        VideoService.ExtractImagesFromVideo(videoFilePath, outputFolderPath, size2);
                        break;

                    case 3:
                        // Load images from folder directly
                        Console.WriteLine("Enter the folder path containing the images (e.g., './images'):");
                        string folderPath = Console.ReadLine();

                        // Default folder path
                        if (string.IsNullOrEmpty(folderPath))
                        {
                            folderPath = "./images";
                        }

                        // Load images from the specified folder
                        var images = ImageService.LoadImages(folderPath);

                        // Convert images to grayscale
                        ImageService.ConvertImagesToGreyScale(images);

                        // Print the grayscale images to the console
                        ConsoleService.PrintImagesToConsole(images);
                        break;
                    case 4:
                        // Exit the program
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter a valid option (1-4).");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number (1-4).");
            }
        }
    }

    static Size GetImageSize()
    {
        // Ask for the size
        Console.WriteLine("Enter the width of the resized images (default: 200):");
        int width = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Enter the height of the resized images (default: 200):");
        int height = Convert.ToInt32(Console.ReadLine());

        // Default size if input is invalid
        if (width <= 0 || height <= 0)
        {
            width = 200;
            height = 200;
        }

        return new Size(width, height);
    }
}
