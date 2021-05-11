using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SharpBrick.PoweredUp;


namespace Example
{
    public class ExampleColorDistanceSensor : BaseExample
    {
        //https://stackoverflow.com/questions/1988833/converting-color-to-consolecolor
        private Dictionary<ConsoleColor, Color> listOfKnownColors = new();
        private int Rmax, GMax, BMax = 0;

        public override async Task ExecuteAsync()
        {
            //Fill the KnownColors-list with the equivalents of availabe ConsoleColors
            foreach (KnownColor color in Enum.GetValues(typeof(KnownColor)))
            {
                var col = Color.FromKnownColor(color);
                var consoleColor = FromColor(col);
                if (!listOfKnownColors.ContainsKey(consoleColor))
                {
                    listOfKnownColors.Add(consoleColor, col);
                }
            }

            using (var twoPortHub = Host.FindByType<TwoPortHub>())
            {
                var colorDistSensor = twoPortHub.B.GetDevice<ColorDistanceSensor>();

                var observers = new[] {
                    colorDistSensor.ColorObservable
                        .Subscribe(color => Log.LogInformation($"LEGO-TechnicColor (as defined by the sensor): {color}")),
                    colorDistSensor.RgbObservable
                        .Subscribe(rgb => {
                        Log.LogInformation($"RGB: R: {rgb.red}, G: {rgb.green}, B: {rgb.blue}");
                            PrintColorName(listOfKnownColors , GetNormalizedColorFromSI(rgb.red , rgb.green , rgb.blue));
                            Rmax = Math.Max(Rmax , rgb.red);
                            GMax= Math.Max(GMax, rgb.green);
                            BMax=Math.Max(BMax , rgb.blue);
                            Log.LogInformation($"Maxima SI: R:{Rmax} G:{GMax} B:{BMax}");

                        }),
                    colorDistSensor.ReflectionObservable
                        .Subscribe(refl => Log.LogInformation("Reflection: {0}", refl)),
                    colorDistSensor.AmbientLightObservable
                        .Subscribe(aml => Log.LogInformation("Ambient Light: {0}", aml)),
                    colorDistSensor.CountObservable
                        .Subscribe(cnt => Log.LogInformation("Count: {0}", cnt)),
                    colorDistSensor.ProximityObservable
                        .Subscribe(dst => Log.LogInformation("Proximity: {0}", dst)),
                };

                await TestMode(colorDistSensor, colorDistSensor.ModeIndexColor);
                await TestMode(colorDistSensor, colorDistSensor.ModeIndexRgb, 300_000);
                await TestMode(colorDistSensor, colorDistSensor.ModeIndexReflection);
                await TestMode(colorDistSensor, colorDistSensor.ModeIndexAmbientLight);
                await TestMode(colorDistSensor, colorDistSensor.ModeIndexCount);
                await TestMode(colorDistSensor, colorDistSensor.ModeIndexProximity);

                foreach (var observer in observers)
                {
                    observer.Dispose();
                }

                await twoPortHub.SwitchOffAsync();
            }

            async Task TestMode(ColorDistanceSensor colorDistSensor, byte mode, int durationMilliseconds = 60_000)
            {
                Log.LogInformation("Teseting mode {0} - START", mode);
                await colorDistSensor.SetupNotificationAsync(mode, enabled: true);

                await Task.Delay(TimeSpan.FromMilliseconds(durationMilliseconds));

                await colorDistSensor.SetupNotificationAsync(mode, enabled: false);
                Log.LogInformation("Teseting mode {0} - END", mode);
            }
        }
        private static ConsoleColor FromColor(Color c)
        {
            var index = (c.R > 128 | c.G > 128 | c.B > 128) ? 8 : 0; // Bright bit
            index |= (c.R > 64) ? 4 : 0; // Red bit
            index |= (c.G > 64) ? 2 : 0; // Green bit
            index |= (c.B > 64) ? 1 : 0; // Blue bit
            return (ConsoleColor)index;
        }
        private void PrintColorName(Dictionary<ConsoleColor, Color> knownColors, Color foundColor)
        {
            var estimatedConsoleColor = ClosestConsoleColorByRGB(knownColors, foundColor);
            var estimatedColor = knownColors[estimatedConsoleColor];
            Log.LogInformation($"Estimated known System.Color (R:{estimatedColor.R} G:{estimatedColor.G} B:{estimatedColor.B}) is named \"{(estimatedColor.IsNamedColor ? estimatedColor.Name : "Unknown")}\"");
            var previousForegroundColor = Console.ForegroundColor;
            var previousBackgroundColor = Console.BackgroundColor;
            if (estimatedConsoleColor == ConsoleColor.Black)
            {
                Console.BackgroundColor = ConsoleColor.White;
            }
            Console.ForegroundColor = estimatedConsoleColor;
            Console.WriteLine($"The detected color is {Enum.GetName(estimatedConsoleColor)}");
            Console.ForegroundColor = previousForegroundColor;
            Console.BackgroundColor = previousBackgroundColor;
        }
        //find index of nearest match by RGB space
        private ConsoleColor ClosestConsoleColorByRGB(Dictionary<ConsoleColor, Color> knownColors, Color foundColor)
        {
            var minColorDistance = knownColors.Select(knownColor => ColorDiff(knownColor.Value, foundColor)).Min(distance => distance);
            return knownColors.First(knownColor => ColorDiff(knownColor.Value, foundColor) == minColorDistance).Key;
        }
        // Euclidean distance in RGB space (without regarding Alpha!)
        private int ColorDiff(Color c1, Color c2)
        {
            return (int)Math.Sqrt(((c1.R - c2.R) * (c1.R - c2.R))
                                   + ((c1.G - c2.G) * (c1.G - c2.G))
                                   + ((c1.B - c2.B) * (c1.B - c2.B)));
        }

        /// <summary>
        /// Get the normalized (to RGB) color from the SI-values reported by the sensor
        /// </summary>
        /// <param name="red">SI-value of the red component</param>
        /// <param name="green">SI-value of the green component</param>
        /// <param name="blue">SI-value of the blue component</param>
        /// <returns></returns>
        private Color GetNormalizedColorFromSI(short red, short green, short blue)
        {
            //the highest value for SI detected was 441 up to now; being safe with 460 from 1023 max
            var factor = 460f / 1024f;
            var r = (int)(float)(red * factor);
            var g = (int)(float)(green * factor);
            var b = (int)(float)(blue * factor);
            return Color.FromArgb(r, g, b);
        }
        /// <summary>
        /// Get the normalized (to RGB) color from the percentage-values reported by the sensor
        /// </summary>
        /// <param name="red">%-value of the red component</param>
        /// <param name="green">%-value of the green component</param>
        /// <param name="blue">%-value of the blue component</param>
        /// <returns></returns>
        private Color GetNormalizedColorFromPct(short red, short green, short blue)
        {
            var factor = 100f * 255f;
            var r = (int)(float)(red * factor);
            var g = (int)(float)(green * factor);
            var b = (int)(float)(blue * factor);
            return Color.FromArgb(r, g, b);
        }
    }
}
