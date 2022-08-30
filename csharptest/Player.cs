using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace csharptest
{
    class Pos
    {
        public Pos(int y, int x) { Y = y; X = x; }
        public int Y;
        public int X;
    }
    class Player
    {
        public int Posy { get; private set; }
        public int Posx { get; private set; }
        Random _random = new Random();
        Board _board;

        enum Dir
        {
            Up = 0,
            Left = 1,
            Down = 2,
            Right = 3
        }

        int _dir = (int)Dir.Up;
        List<Pos> _points = new List<Pos>();

        public void initialize(int posY, int posX, Board board)
        {
            Posx = posX;
            Posy = posY;
            _board = board;

            AStar();

        }

        struct PQNode : IComparable<PQNode>
        {
            public int F;
            public int G;
            public int Y;
            public int X;

            public int CompareTo(PQNode other)
            {
                if (F == other.F)
                    return 0;
                return F < other.F ? 1 : -1;
            }
        }

        void AStar()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            int[] cost = new int[] { 1, 1, 1, 1 };
            // score measurement, F = G + H
            // F = final score, G = start to aimpoint cost, H = how much close to aimpoint

            // (y, x) visit
            bool[,] closed = new bool[_board.Size, _board.Size];

            // (y, x) discover
            int[,] open = new int[_board.Size, _board.Size];
            for (int y = 0; y < _board.Size; y++)
                for (int x = 0; x < _board.Size; x++)
                    open[y, x] = Int32.MaxValue;

            Pos[,] parent = new Pos[_board.Size, _board.Size];

            PriorityQueue<PQNode> pq = new PriorityQueue<PQNode>();

            // discover startpoint
            open[Posy, Posx] = Math.Abs(_board.DestY - Posy) + Math.Abs(_board.DestX - Posx);
            pq.Push(new PQNode() { F = Math.Abs(_board.DestY - Posy) + Math.Abs(_board.DestX - Posx), G = 0, Y = Posy, X = Posx });
            parent[Posy, Posx] = new Pos(Posy, Posx);

            while (pq.Count > 0)
            {
                // find best route
                PQNode node = pq.Pop();

                if (closed[node.Y, node.X])
                    continue;

                // visit
                closed[node.Y, node.X] = true;
                if (node.Y == _board.DestY && node.X == _board.DestX)
                    break;

                // reserve
                for (int i = 0; i < deltaY.Length; i++)
                {
                    int nextY = node.Y + deltaY[i];
                    int nextX = node.X + deltaX[i];
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    if (closed[nextY, nextX])
                        continue;

                    // cost calculation
                    int g = node.G + cost[i];
                    int h = Math.Abs(_board.DestY - nextY) + Math.Abs(_board.DestX - nextX);
                    // skip if already found
                    if (open[nextY, nextX] < g + h)
                        continue;

                    // reserve
                    open[nextY, nextX] = g + h;
                    pq.Push(new PQNode() { F = g + h, G = g, Y = nextY, X = nextX });
                    parent[nextY, nextX] = new Pos(node.Y, node.X);


                }
            }
            CalcPathFromParent(parent);
        }

        void BFS()
        {
            int[] deltaY = new int[] { -1, 0, 1, 0 };
            int[] deltaX = new int[] { 0, -1, 0, 1 };
            bool[,] found = new bool[_board.Size, _board.Size];
            Pos[,] parent = new Pos[_board.Size, _board.Size];


            Queue<Pos> q = new Queue<Pos>();
            q.Enqueue(new Pos(Posy, Posx));
            found[Posy, Posx] = true;
            parent[Posy, Posx] = new Pos(Posy, Posx);

            while (q.Count > 0)
            {
                Pos pos = q.Dequeue();
                int nowY = pos.Y;
                int nowX = pos.X; 

                for (int i = 0; i < 4; i++)
                {
                    int nextY = nowY + deltaY[i];
                    int nextX = nowX + deltaX[i];
                    if (nextX < 0 || nextX >= _board.Size || nextY < 0 || nextY >= _board.Size)
                        continue;
                    if (_board.Tile[nextY, nextX] == Board.TileType.Wall)
                        continue;
                    if (found[nextY, nextX])
                        continue;

                    q.Enqueue(new Pos(nextY, nextX));
                    found[nextY, nextX] = true;
                    parent[nextY, nextX] = new Pos(nowY, nowX); 
                }
            }

            CalcPathFromParent(parent);
        }

        void CalcPathFromParent(Pos[,] parent)
        {
            int y = _board.DestY;
            int x = _board.DestX;
            while (parent[y, x].Y != y || parent[y, x].X != x)
            {
                _points.Add(new Pos(y, x));
                Pos pos = parent[y, x];
                y = pos.Y;
                x = pos.X;
            }
            _points.Add(new Pos(y, x));
            _points.Reverse();
        }

        void RightHand()
        {
            int[] frontY = new int[] { -1, 0, 1, 0 };
            int[] frontX = new int[] { 0, -1, 0, 1 };
            int[] rightY = new int[] { 0, -1, 0, 1 };
            int[] rightX = new int[] { 1, 0, -1, 0 };

            _points.Add(new Pos(Posy, Posx));
            while (Posy != _board.DestY || Posx != _board.DestX) // 목적지 도착 전에는 계속 실	
            {
                // 1. check turn right and go straight
                if (_board.Tile[Posy + rightY[_dir], Posx + rightX[_dir]] == Board.TileType.Empty)
                {
                    _dir = (_dir + 3) % 4;
                    Posy += frontY[_dir];
                    Posx += frontX[_dir];
                    _points.Add(new Pos(Posy, Posx));
                }
                // 2. check go straight
                else if (_board.Tile[Posy + frontY[_dir], Posx + frontX[_dir]] == Board.TileType.Empty)
                {
                    Posy += frontY[_dir];
                    Posx += frontX[_dir];
                    _points.Add(new Pos(Posy, Posx));
                }
                // 3. turn left
                else
                {
                    _dir = (_dir + 1) % 4;
                }
            }
        }

        const int MOVE_TICK = 50; // 0.1sec
        int _sumTick = 0;
        int _lastIndex = 0;
        public void Update(int deltaTick)
        {
            if (_lastIndex >= _points.Count)
            {
                _lastIndex = 0;
                _points.Clear();
                _board.initialize(_board.Size, this);
                initialize(1, 1, _board);
            }

            _sumTick += deltaTick;
            if (_sumTick >= MOVE_TICK)
            {
                _sumTick = 0;

                Posy = _points[_lastIndex].Y;
                Posx = _points[_lastIndex].X;
                _lastIndex++;
            }
        }

    }
}
