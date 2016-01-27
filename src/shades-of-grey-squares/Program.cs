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

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Shades of Grey Squares.\n\n");
            Console.WriteLine("Generating Squares at 1024 on 500 passes.");
            _rand = new Random();
            Console.WriteLine("In Progress...");
            DrawSquares(1024,500);
            Console.WriteLine("Complete.");
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
                        var rgbVal = _rand.Next(1, 255);
                        var wh = _rand.Next(1, size);
                        var x = _rand.Next(0, size);
                        var y = _rand.Next(0, size);
                        var newGrey = Color.FromArgb(150, rgbVal, rgbVal, rgbVal);
                        g.FillRectangle(new SolidBrush(newGrey),x,y,wh,wh);
                    }
                }
                
            }
        }
    }
}
