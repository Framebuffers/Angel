using Angel.Helpers;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angel
{
    internal static class GodotExtensions
    {
        public static void ToConsole(this string s) => GD.Print(s);
       
    }
}
