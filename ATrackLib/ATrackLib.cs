using System;
using System.Collections.Generic;
using System.Text;
using System.Management;
using System.Collections;

namespace ATrack.Library
{

    /// <summary>
    /// 
    /// </summary>
    public class ATrackLib
    {


        /*public static Hashtable GetData(string location)
        {
            Hashtable list = new Hashtable();

            ManagementObject mo = new ManagementObject(location);
            foreach (PropertyData d in mo.Properties)
            {
                list.Add(d.Name, d.Value);
            }
            return list;
        }
        */

        public static void QueryData(string location, string inquery)
        {
            
            ConnectionOptions co = new ConnectionOptions();
            ManagementScope scope = new ManagementScope(@"\\.\root\CIMV2", co);
            ObjectQuery query = new ObjectQuery(inquery);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection collection = searcher.Get();

            foreach (ManagementObject wmiobj in collection)
            {
                PropertyDataCollection searcherProperties = wmiobj.Properties;
                Console.WriteLine("Class : {0}\n", wmiobj.ClassPath.ClassName);

                foreach (PropertyData sp in searcherProperties)
                {
                    Console.WriteLine("\t{0}: {1}", sp.Name, sp.Value);
                }
            }
            Environment.Exit(0);
        }

        /// <summary>
        /// Generates a text file of the information of the computer specified
        /// </summary>
        /// <param name="root">The connection string to the computer to generate info for.</param>
        /// <param name="fileName">The filename of the file to generate.</param>
        public static void GenerateFullInfoToFile(string root, string fileName)
        {
            StringBuilder sb = new StringBuilder();
            ManagementObject mo = new ManagementObject(root);
            sb.AppendFormat("============================ SYSTEM PROPERTIES =========================\n\n");
            foreach (PropertyData d in mo.SystemProperties)
            {
                sb.AppendFormat("{0} = {1}\n", d.Name, d.Value);
            }
            sb.AppendLine("\n================================ PROPERTIES ============================\n");
            foreach (PropertyData d in mo.Properties)
            {
                if (d.Value is string[])
                {
                    sb.AppendFormat("{0} = \n", d.Name);
                    foreach (string s in d.Value as string[])
                    {
                        sb.AppendFormat("\t{0}\n", s);
                    }
                } else {
                    sb.AppendFormat("{0} = {1}\n", d.Name, d.Value);
                }
            }
            System.IO.File.WriteAllText(fileName, sb.ToString());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public static void DisplayInfoFromFile(string fileName)
        {

            string[] input = System.IO.File.ReadAllLines(fileName);
            foreach (string line in input)
            {
                Console.WriteLine("{0}", line);
            }
            Console.Read();
        }

        public static void DisplayInfo(string root)
        {
            ManagementObject mo = new ManagementObject(root);
            Console.WriteLine("\n================================ PROPERTIES ============================\n");
            foreach (PropertyData d in mo.Properties)
            {
                if (d.Value is string[])
                {
                    Console.WriteLine("{0} = \n", d.Name);
                    foreach (string s in d.Value as string[])
                    {
                        Console.WriteLine("\t{0}\n", s);
                    }
                }
                else
                {
                    Console.WriteLine("{0} = {1}\n", d.Name, d.Value);
                }
            }
        }
        
        public static void GetCIMV2Classes()
        {
            //ArrayList objal = new ArrayList();
            ConnectionOptions co = new ConnectionOptions();
            // "", "Henry", "Password123", "", ImpersonationLevel.Impersonate, AuthenticationLevel.Connect, true, null, 10000);
            ManagementScope scope = new ManagementScope(@"\\.\root\CIMV2", co);
            ObjectQuery query = new ObjectQuery("SELECT * FROM meta_class where __class like \"%\"");

            Console.WriteLine("Connected\n");

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            ManagementObjectCollection collection = searcher.Get();

            foreach (ManagementObject wmiClass in collection)
            {
                Console.WriteLine("Class : {0}\n", wmiClass.Path.ClassName);
            }

        }
    }

}
