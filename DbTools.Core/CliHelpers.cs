using System;
using System.Text;

namespace DbTools.Core
{
    public static class CliHelpers
    {
        /// <summary>
        /// Write object to console
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="fgC">Foreground color</param>
        /// <param name="bgC">Background color</param>
        public static void Write(object obj = null, ConsoleColor? fgC = null, ConsoleColor? bgC = null)
        {
            ColorWindow(() => Console.Write(obj), fgC, bgC);
        }

        /// <summary>
        /// Write object to console on a new line
        /// </summary>
        /// <param name="obj">Object</param>
        /// <param name="fgC">Foreground color</param>
        /// <param name="bgC">Background color</param>
        public static void WriteLine(object obj = null, ConsoleColor? fgC = null, ConsoleColor? bgC = null)
        {
            ColorWindow(() => Console.WriteLine(obj), fgC, bgC);
        }

        /// <summary>
        /// Ensure a colored window in which the output will have desired foreground color and background color
        /// </summary>
        /// <param name="actn">Action</param>
        /// <param name="fgC">Foreground color</param>
        /// <param name="bgC">Background color</param>
        public static void ColorWindow(Action actn, ConsoleColor? fgC = null, ConsoleColor? bgC = null)
        {
            #region Foreground color
            var prevFgC = Console.ForegroundColor;
            if (fgC.HasValue)
                Console.ForegroundColor = fgC.Value;
            #endregion

            #region Background color
            var prevBgC = Console.BackgroundColor;
            if (bgC.HasValue)
                Console.BackgroundColor = bgC.Value;
            #endregion

            actn.Invoke();

            Console.ForegroundColor = prevFgC;
            Console.BackgroundColor = prevBgC;
        }

        /// <summary>
        /// Read line from console
        /// </summary>
        /// <param name="size">Buffer size</param>
        /// <returns>Input string</returns>
        public static string ReadLine(int size = Constants.READLINE_BUFFER_SIZE)
        {
            using (var stream = Console.OpenStandardInput(size))
            {
                var buffer = new byte[size];
                return new string(Encoding.UTF8.GetChars(buffer, 0, stream.Read(buffer, 0, buffer.Length)));
            }
        }
    }
}
