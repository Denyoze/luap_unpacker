using System;

namespace LuapSandbox
{
    class LogSystem
    {
        public static bool IsShowInfo = false;

        public static void Info(string str)
        {
            if (IsShowInfo)
            {
                Console.WriteLine($"> [INFO]: {str}");
            }
        }

        public static void Error(string str)
        {
            Console.WriteLine($"> [ERROR]: {str}");
        }
    }
}
