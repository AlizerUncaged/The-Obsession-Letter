﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Console = Colorful.Console;

namespace Controller.Utils
{
    public static class Logging
    {
        public enum Type
        {
            Normal, Error, Success, Fail
        }

        private static Dictionary<Type, string> TypesPrefix = new Dictionary<Type, string>
        {
            { Type.Normal, "> " },
            { Type.Error, "! " },
            { Type.Success, "/ " },
            { Type.Fail, "x " }
        };

        public static void Write(string s, string hex)
        {
            if (!hex.StartsWith('#')) hex = "#" + hex;
            var color = System.Drawing.ColorTranslator.FromHtml(hex);
            Console.Write(TypesPrefix[Type.Normal], color);
            Console.WriteLine(s, color);
        }
        public static void Write(Type type, string s, string hex)
        {
            if (!hex.StartsWith('#')) hex = "#" + hex;
            var color = System.Drawing.ColorTranslator.FromHtml(hex);
            Console.Write(TypesPrefix[type], color);
            Console.WriteLine(s, color);
        }
        public static void Write(string s, string hexstart, string hexend)
        {
            if (!hexstart.StartsWith('#')) hexstart = "#" + hexstart;
            if (!hexend.StartsWith('#')) hexend = "#" + hexend;
            Console.WriteWithGradient(s,
                System.Drawing.ColorTranslator.FromHtml(hexstart),
                System.Drawing.ColorTranslator.FromHtml(hexend), 10);
        }
    }
}
