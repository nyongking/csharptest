using System;

namespace csharptest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false; // not cursor viewable

            int lastTick = 0;
            const int WAIT_TICK = 1000 / 30;
            const char CIRCLE = '\u25cf';
            while (true)
            {
                #region frame
                int currentTick = System.Environment.TickCount;
                if (currentTick - lastTick < WAIT_TICK)
                    continue;
                lastTick = currentTick;
                #endregion
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < 25; i++)
                {
                    for (int j = 0; j < 25; j++)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(CIRCLE);
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
