using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ConsoleService
{

    static long LastFrameTime = -1; //Used to keep track of the last time an image was Printed

    /// <summary>
    /// Iterates and prints Images one by one until all images have been shown.
    /// It uses delta time to keep time consistent
    /// </summary>
    /// <param name="images"></param>
    public static void PrintImagesToConsole(Stack<Bitmap> images)
    {
        if (images.Count == 0) //If Stack is empty return
        {
            return;
        }

        // Calculate delta.
        // Pretty sure this is wrong.
        // We do not skip frames incase of severe time difference.
        // Not  necessary for small images but  will make video with large images go out of sync fast.
        // We will have to skip images in such scenarios to stay in sync.
        long currentTime = Stopwatch.GetTimestamp();
        double deltaTime = (currentTime - LastFrameTime) / (double)Stopwatch.Frequency;
        LastFrameTime = currentTime;

        const double targetFrameRate = 15;
        double targetFrameTime = 1.0 / targetFrameRate;

        // If the elapsed time is less than the target frame time, wait
        if (deltaTime < targetFrameTime)
        {
            int delayMilliseconds = (int)((targetFrameTime - deltaTime) * 1000);
            Task.Delay(delayMilliseconds).Wait(); // Wait in main thread. It's fine.............
        }

        Bitmap Image = images.Peek();
        //Resize the Console to match the image size
        //Console.SetWindowSize(Image.Width + 5, Image.Height + 5);
        PrintImageToConsole(Image);
        images.Pop();

        // Move the cursor to the top left corner to overwrite the image on the next frame

        // Display the next frame recursively
        PrintImagesToConsole(images);
    }

    /// <summary>
    /// Prints to console starting from the top most.
    /// This is slow and needs to be improved
    /// </summary>
    /// <param name="Image"></param>

    public static void PrintImageToConsole(Bitmap image)
    {
        //Instead of printing every single pixel we instead append string every single pixel. And only print at the end
        Console.SetCursorPosition(0, 0);
        StringBuilder sb = new StringBuilder();

        // Calculate aspect ratio to adjust image height
        double aspectRatio = (double)image.Width / image.Height;
        int targetHeight = (int)(Console.WindowWidth / aspectRatio);
        targetHeight = Math.Min(targetHeight, Console.WindowHeight);

        // Resize image to fit console window
        Bitmap resizedImage = new Bitmap(image, new Size(Console.WindowWidth, targetHeight));

        // Iterate over each pixel in the resized image
        for (int y = 0; y < resizedImage.Height; y++)
        {
            for (int x = 0; x < resizedImage.Width; x++)
            {
                Color pixelColor = resizedImage.GetPixel(x, y);
                char coloredAsciiChar = GetCharRangeByColor(pixelColor);
                sb.Append(coloredAsciiChar);
            }
            sb.AppendLine();
        }

        Console.Write(sb.ToString());
    }


    /// <summary>
    /// Converts greyscale into character. This one has a range
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static char GetCharRangeByColor(Color color)
    {
        // Calculate grayscale value
        int grayValue = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);

        // Define symbols for different shades of gray
        char[] symbols = {
        ' ', '.', ',', '-', '~', '+', '*', ':', '=', 'o', 'x', '%', '#', '@', '$',
        '!', '?', '&', '(', ')', '[', ']', '{', '}', '|', '/', '\\', '<', '>', '^', ';',
        '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '_', '-', '*', '+', '=', '!', '?',
        '|', '~', '`', '"', '\'', ',', '.', '_', '(', ')', '[', ']', '{', '}', '<', '>', ':', ';', '^', '&', '$', '#', '@', '%'
    };

        // Calculate the range for each symbol
        int numSymbols = symbols.Length;
        int range = 256 / numSymbols;

        // Determine the index of the symbol based on the grayscale value
        int index = grayValue / range;
        index = Math.Min(index, numSymbols - 1);

        // Return the symbol for the corresponding grayscale intensity
        return symbols[index];
    }



}

