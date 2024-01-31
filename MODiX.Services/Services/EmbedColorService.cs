using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODiX.Services.Services
{
    public class EmbedColorService
    {
        public static int GenerateRandomEmbedColor()
        {
            var rng = new Random(); 
            var color = rng.Next(0x1000000);
            return color;
        }

        public static Dictionary<string, Color> Colors { get; set; } = new Dictionary<string, Color>()
        {
            { "teal", Color.Teal }, { "red", Color.Red}, { "blue", Color.Blue },
            { "black", Color.Black }, { "yellow", Color.Yellow}, { "green", Color.Green },
            { "gray", Color.DarkGray }, {"purple", Color.Purple }, { "peach", Color.PeachPuff }
            //TODO add more colors here
        };

        public static Color GetColor(string colorName, Color defaultColor)
        {
            if (Colors.TryGetValue(colorName, out Color value)) return value;
            return defaultColor;
        }
    }
}
