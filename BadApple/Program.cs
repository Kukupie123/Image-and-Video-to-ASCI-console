using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        /* HIGH LEVEL OVERVIEW
         * 
         * 1.Store Images in Stack
         * 2.Iterate the stack
         * 3.Per iteration print the Top most element and Pop it, thus making the next frame the Top most frame now.
         * 
         * 
         * DELTA TIME
         * To consistenly show the frames, we calculate the difference between the last time we printed and current time
         * If Desired Amount of time has passed we print or else we wait.
         */

        /*
         * ImageService and ConsoleService are helper functions that abstract unnecesary operations
         */

        //Maximise the Console to max possible

        //ImageService.LoadImages all the images from the directory and returns it as a stack
        Stack<Bitmap> images = ImageService.LoadImages("./pics/");

        //Main function for drawing, It is a recursive function, meaning it will keep calling itself until the Images Stack is empty
        ConsoleService.PrintImages(images);
    }
}
