using CommandLine;
using System;
using System.ComponentModel.DataAnnotations;

namespace cds
{
    class Program
    {
        static int Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
               .WithParsed(o =>
               {
                   string res = Helper.ChangeDisplaySettings(
                                        o.Width,
                                        o.Height,
                                        o.RefreshRate,
                                        o.Test
                                            ? Helper.Flags.CDS_TEST
                                            : Helper.Flags.CDS_SET_PRIMARY,
                                        out var success);

                   if (success)
                       Console.WriteLine(res);
                   else {
                       Console.Error.WriteLine(res);
                       Environment.Exit(1);
                   }
               });

            return 0;
        }
    }

    /// <summary>
    /// Command line arguments
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        /// <value>
        /// The width.
        /// </value>
        [Range(1, int.MaxValue)]
        [Option('w', "width", Required = true, HelpText = "Width of the screen")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>
        /// The height.
        /// </value>
        [Range(1,int.MaxValue)]
        [Option('h', "height", Required = true, HelpText = "Height of the screen")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the refresh rate.
        /// </summary>
        /// <value>
        /// The refresh rate.
        /// </value>
        [Range(1, int.MaxValue)]
        [Option('r', "refreshrate", Required = true, HelpText = "Refresh rate of the screen")]
        public int RefreshRate { get; set; }

        /// <summary>
        /// Test resolution change or actually perform.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the change is to be tested only; otherwise, <c>false</c>.
        /// </value>
        [Option('t', "test", Required = false, HelpText = "Test resolution change or actually perform the change.", Default = false)]
        public bool Test { get; set; }
    }
}
