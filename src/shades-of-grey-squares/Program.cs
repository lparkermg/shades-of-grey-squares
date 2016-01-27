using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shades_of_grey_squares
{
    class Program
    {
        private static Random _rand;
        private static int _imagesToGen = 25;
        private static int _size = 1024;
        private static int _passes = 1000;
        static void Main(string[] args)
        {
            
            Console.WriteLine("Welcome to Shades of Grey Squares.\n");
            Console.WriteLine($"Generating Squares at {_size} on {_passes} passes, {_imagesToGen} time(s).");
            _rand = new Random();
            Console.WriteLine("In Progress...");
            for (var image = 0; image < _imagesToGen; image++)
            {
                try
                {
                    DrawSquares(_size, _passes);
                }
                catch (IOException ioe)
                {
                    Console.WriteLine("The File already exists...");
                    throw;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An Exception Occured: {e} - {e.Message}\nStack Trace:{e.StackTrace}");
                }
            }
            Console.WriteLine("");
            Console.WriteLine("Complete!!");
            Console.ReadLine();

        }

        private static void DrawSquares(int size, int passes)
        {
            using (var bmp = new Bitmap(size, size))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    for (var pass = 0; pass < passes; pass++)
                    {
                        var rgbVal = _rand.Next(50, 255);
                        var wh = _rand.Next(1, size/3);
                        var x = _rand.Next(-50, size);
                        var y = _rand.Next(-50, size);
                        var newGrey = Color.FromArgb(100, rgbVal, rgbVal, rgbVal);
                        g.FillRectangle(new SolidBrush(newGrey),x,y,wh,wh);
                    }
                }
                SaveImage(bmp);
            }
        }

        private static void SaveImage(Bitmap bmp)
        {
            var fileLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var filePath = Path.GetDirectoryName(fileLocation);
            if (!Directory.Exists($"{filePath}\\Images"))
                Directory.CreateDirectory($"{filePath}\\Images");

            var fileCount = Directory.GetFiles($"{filePath}\\Images").Length;
            using (var fs = new FileStream($"{filePath}\\Images\\{fileCount}.png", FileMode.CreateNew, FileAccess.Write, FileShare.Write))
            {
                bmp.Save(fs,ImageFormat.Png);
            }
        }
    }
}
