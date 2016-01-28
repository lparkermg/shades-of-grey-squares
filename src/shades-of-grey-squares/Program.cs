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
        private static int _imagesToGen = 5;
        private static int _size = 512;
        private static int _passes = 500;
        private static int _div = 2;
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
                    DrawSquares(_size, _passes,_div);
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

        }

        private static void DrawSquares(int size, int passes, int divider)
        {
            using (var bmp = new Bitmap(size, size))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.FillRectangle(new SolidBrush(Color.White),0,0,size,size);
                    Noise(g, size);

                }

                SaveImage(bmp);
            }
        }

        //Randomizes all values.
        private static Graphics RandomAlgo(Graphics g,int size, int passes, int divider)
        {
            for (var pass = 0; pass < passes; pass++)
            {
                var rgbVal = _rand.Next(40, 240);
                var width = _rand.Next(10, 20);
                var wh = _rand.Next(20, size / divider);
                var x = _rand.Next(-50, size);
                var y = _rand.Next(-50, size);
                var newGrey = Color.FromArgb(255, rgbVal, rgbVal, rgbVal);
                g.FillRectangle(new SolidBrush(newGrey), x, y, wh, wh);
            }

            return g;
        }

        private static Graphics ShrinkingSquares(Graphics g, int size, int squareAmount, int passes)
        {
            for (var pass = 0; pass < passes; pass++)
            {
                var baseRgbVal = _rand.Next(0, 255);
                var wh = _rand.Next(size/4, size/2);
                var x = _rand.Next(-50, size);
                var y = _rand.Next(-50, size);
                var loops = (wh/squareAmount)*2;
                var rgbInc = baseRgbVal/loops;
                var curWh = wh;
                var curRgb = baseRgbVal;
                for (var loop = 0; loop < loops; loop++)
                {
                    var grey = Color.FromArgb(255, curRgb, curRgb, curRgb);
                    g.FillRectangle(new SolidBrush(grey),x,y,curWh,curWh );

                    curWh -= loops;
                    curRgb -= rgbInc;
                    x += loops/2;
                    y += loops/2;
                }
            }
            return g;
        }

        private static Graphics Noise(Graphics g, int size)
        {
            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < size; y++)
                {
                    var curRgb = _rand.Next(0, 255);
                    var grey = Color.FromArgb(255, curRgb, curRgb, curRgb);
                    g.FillRectangle(new SolidBrush(grey),x,y,2,2 );
                }
            }
            return g;
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

        private static Bitmap Blur(Bitmap image, Rectangle rectangle, Int32 blurSize)
        {
            Bitmap blurred = new Bitmap(image.Width, image.Height);

            // make an exact copy of the bitmap provided
            using (Graphics graphics = Graphics.FromImage(blurred))
                graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height),
                    new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);

            // look at every pixel in the blur rectangle
            for (var xx = rectangle.X; xx < rectangle.X + rectangle.Width; xx++)
            {
                for (var yy = rectangle.Y; yy < rectangle.Y + rectangle.Height; yy++)
                {
                    Int32 avgR = 0, avgG = 0, avgB = 0;
                    Int32 blurPixelCount = 0;

                    // average the color of the red, green and blue for each pixel in the
                    // blur size while making sure you don't go outside the image bounds
                    for (var x = xx; (x < xx + blurSize && x < image.Width); x++)
                    {
                        for (var y = yy; (y < yy + blurSize && y < image.Height); y++)
                        {
                            var pixel = blurred.GetPixel(x, y);

                            avgR += pixel.R;
                            avgG += pixel.G;
                            avgB += pixel.B;

                            blurPixelCount++;
                        }
                    }

                    avgR = avgR / blurPixelCount;
                    avgG = avgG / blurPixelCount;
                    avgB = avgB / blurPixelCount;

                    // now that we know the average for the blur size, set each pixel to that color
                    for (var x = xx; x < xx + blurSize && x < image.Width && x < rectangle.Width; x++)
                        for (var y = yy; y < yy + blurSize && y < image.Height && y < rectangle.Height; y++)
                            blurred.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }

            return blurred;
        }
    }
}
