using System;
using System.IO;
using System.Drawing;
using OpenCvSharp;

class Program
{
    static void Main()
    {
        try
        {
            while (true)
            {
                // Display the menu options
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Load a video directly");
                Console.WriteLine("2. Extract frames from video");
                Console.WriteLine("3. Load images from folder directly");
                Console.WriteLine("4. Clear contents of exportedFrames folder");
                Console.WriteLine("5. Exit");

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
                            HandleLoadVideo();
                            break;
                        case 2:
                            HandleClearExportedFrames(false);
                            HandleExtractFrames();
                            break;
                        case 3:
                            HandleLoadImages();
                            break;
                        case 4:
                            HandleClearExportedFrames(true);
                            break;
                        case 5:
                            // Exit the program
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please enter a valid option (1-5).");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number (1-5).");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}\nStack: \n{ex.StackTrace}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    static void HandleLoadVideo()
    {
        // Load a video directly
        Console.WriteLine("Enter the path of the video file (e.g., './video.mp4'):");
        string path = Console.ReadLine();

        // Default video path
        if (string.IsNullOrEmpty(path))
        {
            path = "./video.mp4";
        }

        // Ask for the size
        System.Drawing.Size size1 = GetImageSize();

        // Generate images from the video
        var videoImages = VideoService.GenerateImagesFromVideo(path, size1);

        // Print images to console with specified frame rate and reverse option
        ConsoleService.PrintImagesToConsole(videoImages, new VideoCapture(path).Fps - 14);
    }

    static void HandleExtractFrames()
    {
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
        System.Drawing.Size size2 = GetImageSize();

        // Extract frames from the video
        VideoService.ExtractImagesFromVideo(videoFilePath, outputFolderPath, size2);
    }

    static void HandleLoadImages()
    {
        // Load images from folder directly
        Console.WriteLine("Enter the folder path containing Greyscale images (e.g., './exportedFrames'):");
        string folderPath = Console.ReadLine();

        // Default folder path
        if (string.IsNullOrEmpty(folderPath))
        {
            folderPath = "./exportedFrames";
        }

        // Load images from the specified folder
        var images = ImageService.LoadImages(folderPath);

        // Print the grayscale images to the console with the specified frame rate and reverse option
        ConsoleService.PrintImagesToConsole(images, VideoService.GetFPSofExportedFrames(folderPath));
    }

    static void HandleClearExportedFrames(bool askConfirmation)
    {
        // Ask for confirmation before clearing the folder
        if (askConfirmation)
        {
            Console.WriteLine("Are you sure you want to clear the contents of the 'exportedFrames' folder? (Y/N):");
            string confirmation = Console.ReadLine().ToUpper();

            if (confirmation != "Y")
            {
                // Clear the contents of the folder
                Console.WriteLine("'Aborting.");
                return;

            }
        }

        // Clear the contents of the folder
        string exportedFramesPath = "./exportedFrames";
        if (Directory.Exists(exportedFramesPath))
        {
            Directory.Delete(exportedFramesPath, true);
            Console.WriteLine("Contents of 'exportedFrames' folder cleared successfully.");
        }
        else
        {
            Console.WriteLine("'exportedFrames' folder not found.");
        }


    }

    static System.Drawing.Size GetImageSize()
    {
        int width = 200;
        int height = 200;

        // Ask for the size and validate input
        Console.WriteLine("Enter the width of the resized images (default: 200):");
        string widthInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(widthInput) && int.TryParse(widthInput, out int parsedWidth) && parsedWidth > 0)
        {
            width = parsedWidth;
        }

        Console.WriteLine("Enter the height of the resized images (default: 200):");
        string heightInput = Console.ReadLine();
        if (!string.IsNullOrEmpty(heightInput) && int.TryParse(heightInput, out int parsedHeight) && parsedHeight > 0)
        {
            height = parsedHeight;
        }

        return new System.Drawing.Size(width, height);
    }
}
