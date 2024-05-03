using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// Singleton class for easy access to a Global Stopwatch object
/// </summary>
public class GlobalWatch
{
    private GlobalWatch()
    {

    }
    private static Stopwatch stopwatch = new Stopwatch();
    public static Stopwatch GetWatch()
    {
        return stopwatch;
    }
}

