using Pushing2.Eventargs;
using Pushing2.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Pushing2.Model
{
    public class PushingModel
    {
        private IDataAccess dataAccess;
        private FieldState[,] board;
        private int size;
        private int length;
        private int w;
        private int b;
        private PushingTable _table;
        public FieldState[,] Board { get { return board; } }
        public int Size { get { return size; } }
        public int Length { get { return length; } }
        public int W { get { return w; } }
        public int B { get { return b; } }

        public event EventHandler<GameStartedEventArgs> GameStarted;
        public event EventHandler<GameStartedEventArgs> Refresh;
        public event EventHandler<GameFinishedEvenetArgs> Finish;

        public PushingModel(IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public void StartNewGame(int size)
        {
            w = 0;
            b = 0;
            this.size = size;
            length = 3 * size;
            board = new FieldState[size, size];
            var rng = new Random();
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    board[i, j] = (FieldState)rng.Next(0, 2);
                }
            }

            if(GameStarted is not null)
            {
                GameStarted(this, new GameStartedEventArgs(size, board));
            }
        }

        public void MoveUp(int row ,int col)
        {
            if (row > 0 && board[row, col] != FieldState.Gray)
            {
                if (board[0, col] == FieldState.White)
                { ++b; }
                else if (board[0, col] == FieldState.Black)
                { ++w; }
                Colormove(col, row, "Up");
                --length;
                FinishM();
            }
        }

        public void MoveDown(int row, int col)
        {
            if (row + 1 < size && board[row, col] != FieldState.Gray)
            {
                if (board[size - 1, col] == FieldState.White)
                { ++b; }
                else if (board[size - 1, col] == FieldState.Black)
                { ++w; }
                Colormove(col, row, "Down");
                --length;
                FinishM();
            }
        }

        public void MoveRight(int row, int col)
        {
            if (col + 1 < size && board[row, col] != FieldState.Gray)
            {
                if (board[row, size - 1] == FieldState.White)
                { ++b; }
                else if (board[row, size - 1] == FieldState.Black)
                { ++w; }
                Colormove(col, row, "Right");
                --length;
                FinishM();
            }
        }

        public void MoveLeft(int row, int col)
        {
            if (col > 0 && board[row, col] != FieldState.Gray)
            {
                if (board[row, 0] == FieldState.White)
                { ++b; }
                else if (board[row, 0] == FieldState.Black)
                { ++w; }
                Colormove(col, row, "Left");
                --length;
                FinishM();
            }
        }

       /* public void Move(Keys dir)
        {
            int col = t.Column;
            int row = t.Row;
            if (dir==Keys.Up && row>0 && board[row,col] != FieldState.Gray)
            {
                if (board[0, col] == FieldState.White)
                { ++b; }
                else if (board[0, col] == FieldState.Black)
                { ++w; }
                Colormove(col, row, dir);
            }
            else if(dir==Keys.Down && row+1<size && board[row, col] != FieldState.Gray)
            {
                if (board[size-1, col] == FieldState.White)
                { ++b; }
                else if (board[size-1, col] == FieldState.Black)
                { ++w; }
                Colormove(col, row, dir);
            }
            else if (dir == Keys.Right && col+1<size && board[row, col] != FieldState.Gray)
            {
                if (board[row, size-1] == FieldState.White)
                { ++b; }
                else if (board[row, size-1] == FieldState.Black)
                { ++w; }
                Colormove(col, row, dir);
            }
            else if (dir == Keys.Left && col>0 && board[row, col] != FieldState.Gray)
            {
                if (board[row, 0] == FieldState.White)
                { ++b; }
                else if (board[row, 0] == FieldState.Black)
                { ++w; }
                Colormove(col, row, dir);
            }
            length--;
            FinishM();
        }*/

        public void Colormove(int col , int row,string dir)
        {
            if (dir == "Up")
            {
                if (board[row - 1, col] == FieldState.Gray)
                {
                    board[row - 1, col] = board[row, col];
                    board[row, col] = FieldState.Gray;
                }
                else
                {
                    for (int i = 0; i < row; i++)
                    {
                        board[i, col] = board[i + 1, col];
                    }
                    board[row, col] = FieldState.Gray;
                }
            }
            else if (dir == "Down") 
            {
               if (board[row + 1, col] == FieldState.Gray)
               {
                    board[row + 1, col] = board[row, col];
                    board[row, col] = FieldState.Gray;
               }
                else
                {
                    for (int i = size-2; i >= row; i--)
                    {
                        board[i + 1, col] = board[i, col];
                    }
                    board[row, col] = FieldState.Gray;
                }
            }
            else if (dir == "Right") 
            {
                if (board[row, col + 1] == FieldState.Gray)
                {
                    board[row, col + 1] = board[row, col];
                    board[row, col] = FieldState.Gray;
                }
                else
                {
                    for (int i = size-2; i >= col; i--)
                    {
                        board[row, i + 1] = board[row, i];
                    }
                    board[row, col] = FieldState.Gray;
                }
            }
            else if (dir == "Left")
            {
                if (board[row, col - 1] == FieldState.Gray)
                {
                    board[row, col - 1] = board[row, col];
                    board[row, col] = FieldState.Gray;
                }
                else
                {
                    for (int i = 0; i < col; i++)
                    {
                        board[row, i] = board[row, i + 1];
                    }
                    board[row, col] = FieldState.Gray;
                }
            }
            onRefresh();
        }

        private void onRefresh()
        {
            Refresh?.Invoke(this, new GameStartedEventArgs(size,board));
        }

        public void FinishM()
        {
            if(length == 0 && Finish is not null)
            {
                Finish(this, new GameFinishedEvenetArgs(w,b));
                StartNewGame(size);
            }
        }

        public void SaveGame(string path)
        {
            dataAccess.SaveGame(path,size,board,w,b);
        }

        public void LoadGame(string path)
        {
            _table = dataAccess.LoadGame(path);
            board = _table.board;
            size = _table.size;
            w = _table.w;
            b = _table.b;
            onRefresh();

        }
    }
}
