using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace OpenBots.Core.IO
{
    public class JavaInterface
    {
        public Process Create(string jarName, string args)
        {
            var jarLibary = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Resources", jarName);

            if (!File.Exists(jarLibary))
            {
                throw new FileNotFoundException("JAR Library was not found at " + jarLibary);
            }

            Process javaProc = new Process();
            javaProc.StartInfo.FileName = "java";
            javaProc.StartInfo.Arguments = string.Join(" ", "-jar", "\"" + jarLibary + "\"", args);
            javaProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            javaProc.StartInfo.UseShellExecute = false;
            javaProc.StartInfo.RedirectStandardOutput = true;

            return javaProc;
        }
    }
}
