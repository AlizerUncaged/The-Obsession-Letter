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
            None, Normal, Error, Success, Received
        }

        private static Dictionary<Type, (string, Color)> _types = new Dictionary<Type, (string, Color)>
        {
            { Type.None, new ("[>] ",  System.Drawing.ColorTranslator.FromHtml("#9F5F80")) },
            { Type.Normal, new ("[>] ",  System.Drawing.ColorTranslator.FromHtml("#9F5F80")) },
            { Type.Success, new ("[>>] ",  System.Drawing.ColorTranslator.FromHtml("#FF8474")) },
            { Type.Received, new ("[<<] ",  System.Drawing.ColorTranslator.FromHtml("#9F5F80")) },
            { Type.Error, new ("[xxx] ",  System.Drawing.ColorTranslator.FromHtml("#583D72")) }
        };

        private static Color _prefixcolor = System.Drawing.ColorTranslator.FromHtml("#583D72");
        public static string GetTypePrefix(Type t) {
            return _types[t].Item1;
        }
        public static Color GetTypeColor(Type t)
        {
            return _types[t].Item2;
        }
        public static void Write(string s, bool newline = true)
        {
            Write(Type.Normal, s, newline);
        }

        public static void Write(Type type, string s, bool newline = true)
        {
            if (newline) s += Environment.NewLine;

            Console.Write(GetTypePrefix(type), _prefixcolor);
            Console.ResetColor();
            Console.Write(s, GetTypeColor(type));
        }
        public static void Write(string s, string hexstart, string hexend)
        {
            if (!hexstart.StartsWith('#')) hexstart = "#" + hexstart;
            if (!hexend.StartsWith('#')) hexend = "#" + hexend;

            Console.WriteWithGradient(s,
                System.Drawing.ColorTranslator.FromHtml(hexstart),
                System.Drawing.ColorTranslator.FromHtml(hexend));
        }
    }
}
