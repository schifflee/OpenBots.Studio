using System.CodeDom.Compiler;

namespace taskt.Core.Common
{
    public class CompilerServices
    {
        public CompilerResults CompileInput(string codeInput)
        {
            //define file output
            string output = "OpenBotsOnTheFly.exe";

            //create provider
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");

            //create compile parameters
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = true;
            parameters.OutputAssembly = output;

            //compile
            CompilerResults results = codeProvider.CompileAssemblyFromSource(parameters, codeInput);
            return results;
        }
    }
}
