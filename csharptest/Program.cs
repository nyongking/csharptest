using System;

namespace csharptest
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            board.initialize(25);

            Console.CursorVisible = false; // not cursor viewable

            int lastTick = 0;
            const int WAIT_TICK = 1000 / 30;
            
            while (true)
            {
                #region frame
                int currentTick = System.Environment.TickCount;
                if (currentTick - lastTick < WAIT_TICK)
                    continue;
                lastTick = currentTick;
                #endregion
                Console.SetCursorPosition(0, 0);
                board.Render();
                
            }
        }
    }
}
