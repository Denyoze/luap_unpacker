using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using LuapSandbox;

namespace LuapUnpacker
{
    struct LuapStruct
    {
        public byte[]   _unknown; // hash?
        public int      offest;
        public int      length1;
        public int      length2;
    };

    class Program
    {
        private static bool isDecompile     = false;
        private static bool isSkipDecompile = false;
        private static string fileName      = "";

        private static List<LuapStruct> headers = new List<LuapStruct>();

        static void Main(string[] args)
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("--- The Saboteur .luap unpacker by Denyoze ---");
            Console.WriteLine("---      2018 - http://denyoze.space       ---");
            Console.WriteLine("---      Decompiler: unluac by tehtmi      ---");
            Console.WriteLine("----------------------------------------------");

            if (args.Length == 0)
            {
                Console.WriteLine("> no arguments...");
                Console.WriteLine("> Call like this -> luap_unpacker.exe -f <file_name> [-info] [-d]");
                Console.WriteLine("> Where:");
                Console.WriteLine("> -f <file name> - file to unpack");
                Console.WriteLine("> -info - show more info in console");
                Console.WriteLine("> -d - decompile files when unpack");
                Console.WriteLine("> -pleaseno - skip decompiled file, if it exist");

                Environment.Exit(1);
            }

            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-f":
                        i++;
                        fileName = args[i];
                        break;

                    case "-info":
                        LogSystem.IsShowInfo = true;
                        break;

                    case "-d":
                        isDecompile = true;
                        break;

                    case "-pleaseno":
                        isSkipDecompile = true;
                        break;

                    default:
                        Console.WriteLine($"> Wrong arg.: {args[i]}");
                        break;
                }
            }

            Unpack(fileName);
        }

        public static void Unpack(string fileName)
        {
            LogSystem.Info($"starting unpacking of {fileName}...");

            try
            {
                using (BinaryReader br = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read)))
                {
                    int size = br.ReadInt32();

                    LogSystem.Info($"files in {fileName} = {size}");

                    for (int i = 0; i < size; i++)
                    {
                        var structInfo = new LuapStruct();

                        structInfo._unknown = br.ReadBytes(8);
                        structInfo.offest = br.ReadInt32();
                        structInfo.length1 = br.ReadInt32();
                        structInfo.length2 = br.ReadInt32();
                        br.ReadByte();

                        LogSystem.Info($"file {i} header -> [{BitConverter.ToString(structInfo._unknown)}, {structInfo.offest}, {structInfo.length1}, {structInfo.length2}]");

                        headers.Add(structInfo);
                    }

                    LogSystem.Info("unpacking and decompiling...");

                    for (int i = 0; i < size; i++)
                    {
                        var currenFile = headers[i];
                        var fileBytes = br.ReadBytes(currenFile.length1);

                        Directory.CreateDirectory("compiled");

                        File.Create($"compiled/file_{i}.luac").Close();
                        File.WriteAllBytes($"compiled/file_{i}.luac", fileBytes);

                        if (isDecompile)
                        {
                            Directory.CreateDirectory("decompiled");

                            if (File.Exists($"decompiled/file_{i}.lua") && isSkipDecompile)
                            {
                                continue;
                            }

                            ProcessStartInfo info = new ProcessStartInfo
                            {
                                FileName = "cmd.exe",
                                Arguments = $"/c java -jar jar/unluac.jar compiled/file_{i}.luac > decompiled/file_{i}.lua",
                                WindowStyle = ProcessWindowStyle.Hidden
                            };

                            LogSystem.Info($"decompiling [compiled/file_{i}.luac]");
                            var proc = Process.Start(info);
                            proc.WaitForExit();
                            proc.Close();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                LogSystem.Error(ex.Message);
                Environment.Exit(1);
            }
        }
    }
}
