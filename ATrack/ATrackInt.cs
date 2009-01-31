using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Text;
using ATrack.Library;

namespace ATrack
{
    class Program
    {
        static string[] validArgs = new string[7] { "/F", "/L", "/N", "/C", "/D", "/Q", "/O" };
        private static string wmiNameSpace = string.Empty;
        private static string wmiClass = string.Empty;

        static void Main(string[] args)
        {

            if ((args.Length == 0) || (args.Length > 4))
            {
                PrintHelp();
                return;
            }
            else if (args.Length != 0)
            {
                VerifyArgs(args);
            }

        }

        private static void PrintHelp()
        {
            string appName = Process.GetCurrentProcess().ProcessName;
            StringBuilder s = new StringBuilder();
            s.Append("\n" + appName + " ");
            s.Append("[/F] <file> [/N] <namespace>  [/C] <class> [/O] <computer> <query> \n\t[/L] [/D]\n\n");
            s.Append("   /F\t\tFile with list of computers to be queried.\n");
            s.Append("     \t\tThe File must have one entry per line\n");
            s.Append("     \t\tand must be followed by /Q and a query.\n");
            s.Append("   /L\t\tList Available NameSpaces\n");
            s.Append("   /N\t\tUse specified WMI NameSpace. If no NameSpace is given,\n");
            s.Append("     \t\t'root\\CIMV2' is used by default.\n");
            s.Append("   /C\t\tUse specified WMI Class.\n");
            s.Append("   /D\t\tUse The default NameSpace 'root\\CIMV2' and the default Class\n");
            s.Append("   \t\t'Win32_ComputerSystem' and display on screen\n");
            s.Append("   /Q\t\tQuery WMI using WQL\n");
            s.Append("   /O\t\tQuery using WQL and output to file. Needs a file name, a DNS\n");
            s.Append("     \t\tresovable computer name or ip address and valid WQL query in\n");
            s.Append("      \t\tquotes.\n");

            Console.WriteLine("{0}", s);
            Environment.Exit(0);
            //Console.Read();
        }

        private static void UseDefaults()
        {
            wmiClass = "Win32_ComputerSystem";
            wmiNameSpace = "root\\CIMV2";
            //wmiClass = "__NAMESPACE";
            //wmiNameSpace = "root";
        }

        private static void VerifyArgs(string[] args)
        {
            int i = 0;
            /*foreach (string arg in args)
            {
                Console.WriteLine("Arg {0} : {1}", i, args.GetValue(i));
                i++;
            }
             */
          
            if (args[0].Contains("/L"))
            {
                ATrackLib.GetCIMV2Classes();
                Environment.Exit(0);
            }
            else if (args[0].Contains("/D"))
            {
                UseDefaults();
                string compName = Environment.MachineName;
                string fileName = "C:\\" + compName + ".txt";
                ATrackLib.DisplayInfo(@"\\.\root\CIMV2:" + wmiClass + "='" + compName + "'");
                Environment.Exit(1);
            }
            else if (args[0].Contains("/O"))
            {
                if (args.Length >= 3)
                {
                    try
                    {
                        Console.WriteLine("Doing stuff with /O arg\n");
                        DoStub();
                        //ATrackLib.QueryDataOutput(args[1].ToString(), args[2]);
                    }
                    catch
                    {
                        Console.WriteLine("\nCheck your parameters.\n");
                        Environment.Exit(1);
                    }
                }
                else
                {
                    PrintHelp();
                }

            }
            else if (args[0].Contains("/Q"))
            {
                try
                {
                    ATrackLib.QueryData(@"\\.\root\CIMV2:", args[1].ToString());
                }
                catch
                {
                    Console.WriteLine("\nCheck your query.\n");
                    Environment.Exit(1);
                }
            }
            else
            {
                PrintHelp();
            }
        }

        private static void DoStub()
        {
            //throw new NotImplementedException();
            Console.WriteLine("Stub to Catch all unimplemneted methods");
            return;
        }

    }

}
