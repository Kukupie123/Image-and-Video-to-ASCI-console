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
    public static void PerFrameImage(Stack<Bitmap> images)
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

        const double targetFrameRate = 30;
        double targetFrameTime = 1.0 / targetFrameRate;

        // If the elapsed time is less than the target frame time, wait
        if (deltaTime < targetFrameTime)
        {
            int delayMilliseconds = (int)((targetFrameTime - deltaTime) * 1000);
            Task.Delay(delayMilliseconds).Wait(); // Wait in main thread. It's fine.............
        }


        PrintImageToConsole(images.Peek());
        images.Pop();

        // Move the cursor to the top left corner to overwrite the image on the next frame

        // Display the next frame recursively
        PerFrameImage(images);
    }

    /// <summary>
    /// Prints to console starting from the top most.
    /// This is slow and needs to be improved
    /// </summary>
    /// <param name="Image"></param>
    private static void PrintImageToConsole(Bitmap Image)
    {
        //TODO: Come up with better logic to print this shit instead of looping per pixel
        Console.SetCursorPosition(0, 0);
        for (int y = 0; y < Image.Height; y++)
        {
            for (int x = 0; x < Image.Width; x++)
            {
                Color c = Image.GetPixel(x, y);
                Console.Write(GetCharRangeByColor(c));
            }

            Console.WriteLine("");
        }

    }

    /// <summary>
    /// Converts greyscal to a character. This one one converts into binary
    /// </summary>
    /// <param name="Color"></param>
    /// <param name="Threshold"></param>
    /// <returns></returns>
    private static char GetCharByColor(Color Color, int Threshold = 20)
    {
        int grayValue = (int)(Color.R * 0.3 + Color.G * 0.59 + Color.B * 0.11);
        return grayValue > Threshold ? '1' : ' ';
    }

    /// <summary>
    /// Converts greyscale into character. This one has a range
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    private static char GetCharRangeByColor(Color color)
    {
        //Some formula used to calculate Greyscale shit idk
        int grayValue = (int)(color.R * 0.3 + color.G * 0.59 + color.B * 0.11);

        int veryLightThreshold = 200;
        int lightThreshold = 160;
        int midThreshold = 120;
        int darkThreshold = 80;

        // Determine the shade of gray, Will try to use switch later on
        if (grayValue > veryLightThreshold)
        {
            return ' '; // Very light
        }
        else if (grayValue > lightThreshold)
        {
            return '@'; // Light
        }
        else if (grayValue > midThreshold)
        {
            return '#'; // Mid
        }
        else if (grayValue > darkThreshold)
        {
            return '$'; // Dark
        }
        else
        {
            return '%'; // Very dark
        }
    }
   

}

