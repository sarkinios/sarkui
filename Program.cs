using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace Sarkui
{
    static class Program
    {

        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //     AppDomain.CurrentDomain.AppendPrivatePath(@"lib");
            //      initializeAssembly();
            //       Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
            //    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);
        }



        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            try
            {
                AssemblyName name = new AssemblyName(args.Name);

                string expectedFileName = name.Name + ".dll";
                string rootDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                return LoadAssembly(rootDir, expectedFileName, "", @"lib");
            }
            catch
            {
                return null;
            }
        }

        private static Assembly LoadAssembly(string rootDir, string fileName, params string[] directories)
        {
            foreach (string directory in directories)
            {
                string path = Path.Combine(rootDir, directory, fileName);
                if (File.Exists(path))
                    return Assembly.LoadFrom(path);
            }
            return null;
        }
       



    }

}





