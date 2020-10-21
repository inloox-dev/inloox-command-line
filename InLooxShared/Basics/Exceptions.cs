using System;

namespace InLooxShared.Basics
{
    public static class Exceptions
    {
        public static void PrintException(Exception ex)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            TraverseExceptions(ex);
            Console.WriteLine(ex.StackTrace);

            Console.ForegroundColor = defaultColor;
        }

        private static void TraverseExceptions(Exception ex)
        {
            Console.WriteLine(ex.Message);
            if (ex.InnerException != null)
                TraverseExceptions(ex.InnerException);
        }
    }
}
