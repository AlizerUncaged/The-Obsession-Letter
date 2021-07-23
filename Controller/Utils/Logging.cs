using System;
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
            Normal, Error, Success, Received
        }

        private static Dictionary<Type, (string, Color)> _types = new Dictionary<Type, (string, Color)>
        {
            { Type.Normal, new ("[>] ",  System.Drawing.ColorTranslator.FromHtml("#9F5F80")) },
            { Type.Success, new ("[>>] ",  System.Drawing.ColorTranslator.FromHtml("#FF8474")) },
            { Type.Received, new ("[<<] ",  System.Drawing.ColorTranslator.FromHtml("#9F5F80")) },
            { Type.Error, new ("[xxx] ",  System.Drawing.ColorTranslator.FromHtml("#583D72")) }
        };

        private static Color _prefixcolor = System.Drawing.ColorTranslator.FromHtml("#583D72");
        public static void Write(string s, bool newline = true)
        {
            Write(Type.Normal, s, newline);
        }
        public static void Write(Type type, string s, bool newline = true)
        {
            if (newline) s += Environment.NewLine;

            Console.Write(_types[type].Item1, _prefixcolor);
            Console.Write(s, _types[type].Item2);
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
