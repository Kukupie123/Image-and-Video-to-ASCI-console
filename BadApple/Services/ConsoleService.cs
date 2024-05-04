using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

public class ConsoleService
{
    static long LastFrameTime = -1; // Used to keep track of the last time an image was printed

    /// <summary>
    /// Iterates and prints Images one by one until all images have been shown.
    /// It uses delta time to keep time consistent
    /// </summary>
    /// <param name="frames">A stack containing the frames to be printed</param>
    /// <param name="targetFrameRate">The target frame rate in frames per second</param>
    /// <param name="reverse">Boolean indicating whether frames should be printed in reverse order</param>
    public static void PrintImagesToConsole(Stack<Bitmap> frames, double targetFrameRate)
    {

        if (frames.Count == 0) // If Stack is empty, return
        {
            return;
        }

        // Calculate delta time
        long currentTime = Stopwatch.GetTimestamp();
        double deltaTime = (currentTime - LastFrameTime) / (double)Stopwatch.Frequency;
        LastFrameTime = currentTime;

        double targetFrameTime = 1.0 / targetFrameRate;

        // If the elapsed time is less than the target frame time, wait
        if (deltaTime < targetFrameTime)
        {
            int delayMilliseconds = (int)((targetFrameTime - deltaTime) * 1000);
            Task.Delay(delayMilliseconds).Wait(); // Wait in main thread
        }

        Bitmap frame = frames.Peek();
        frames.Pop();
        PrintImageToConsole(frame);

        // Display the next frame recursively
        PrintImagesToConsole(frames, targetFrameRate);
    }

    /// <summary>
    /// Prints a single frame to the console
    /// </summary>
    /// <param name="frame">The frame to be printed</param>
    public static void PrintImageToConsole(Bitmap frame)
    {
        // Calculate aspect ratio to adjust image height
        double aspectRatio = (double)frame.Width / frame.Height;

        // Calculate target width and height to fit the console window
        int targetWidth = Console.WindowWidth;
        int targetHeight = (int)(targetWidth / aspectRatio);

        // If the calculated height is greater than the console height, resize based on console height
        if (targetHeight > Console.WindowHeight)
        {
            targetHeight = Console.WindowHeight;
            targetWidth = (int)(targetHeight * aspectRatio);
        }

        // Resize image to fit console window
        Bitmap resizedImage = new Bitmap(frame, new Size(targetWidth, targetHeight));

        // Instead of printing every single pixel, append string for each pixel and print at the end
        StringBuilder sb = new StringBuilder();

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

        Console.SetCursorPosition(0, 0);
        Console.Write(sb.ToString());
    }


    /// <summary>
    /// Converts greyscale into character using a range
    /// </summary>
    /// <param name="color">The color to be converted</param>
    /// <returns>Character representation of the color</returns>
    private static char GetCharRangeByColor(Color color)
    {
        // Calculate grayscale value
        int grayValue = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);

        // Define symbols for different shades of gray (from dark to light)
        char[] symbols = { ' ', '.', ',', ':', '!', 'i', 'I', 'o', 'O', '*', '#', '@' };

        // Calculate the index based on brightness
        int index = (grayValue * (symbols.Length - 1)) / 255;

        // Return the symbol for the corresponding brightness level
        return symbols[index];
    }
}
