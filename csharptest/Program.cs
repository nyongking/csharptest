using System;

namespace csharptest
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board();
            Player player = new Player();
            board.initialize(25, player);
            player.initialize(1, 1, board);

            Console.CursorVisible = false; // not cursor viewable

            int lastTick = 0;
            const int WAIT_TICK = 3000 / 30;
            
            while (true)
            {
                #region frame
                int currentTick = System.Environment.TickCount;
                if (currentTick - lastTick < WAIT_TICK)
                    continue;
                int deltaTick = currentTick - lastTick;
                lastTick = currentTick;
                #endregion

                player.Update(deltaTick);
                Console.SetCursorPosition(0, 0);
                board.Render();
                
            }
        }
    }
}
