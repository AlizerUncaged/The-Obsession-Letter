using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Armitage
{
    /// <summary>
    /// Compiles C# code at runtime to an executable.
    /// </summary>
    public class Compiler
    {
        private string[] _dependencies;
        private CSharpCodeProvider _roslyn;
        private CompilerParameters _params;
        private string _outputfile;
        public Compiler(string[] dependencies, string output) {
            _dependencies = dependencies;
            _outputfile = output;

            _roslyn = new CSharpCodeProvider();
            _params = new CompilerParameters(_dependencies, _outputfile, true);
        }

        public bool CompileCode(string code) {
            try
            {
                _params.GenerateExecutable = true;
                CompilerResults results = _roslyn.CompileAssemblyFromSource(_params, code);
                results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));
            }
            catch {
                return false;
            }
            return File.Exists(_outputfile);
        }
    }
}
