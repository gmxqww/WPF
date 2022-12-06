using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pushing2.Model;

namespace Pushing2.Persistance
{
    public class DataAccess : IDataAccess
    {
        private int size;
        private FieldState[,] board;
        private int w;
        private int b;
        private string[] a;
         
        public PushingTable LoadGame(string path)
        {
            try
            {

                StreamReader r = new StreamReader(path);
                size = int.Parse(r.ReadLine());
                board = new FieldState[size, size];
                for (int i = 0; i < size; i++)
                {
                    a = r.ReadLine().Split(" ");
                    for (int j = 0; j < size; j++)
                    {
                        board[i, j] = (FieldState)Enum.Parse(typeof(FieldState), a[j]);
                    }
                }
                w = int.Parse(r.ReadLine());
                b = int.Parse(r.ReadLine());
                r.Close();

                PushingTable t = new PushingTable(size, board, w, b);
                return t;
            }
            catch
            {
                throw new PushingDataException();
            }
            
        }

        public void SaveGame(string path , int size, FieldState[,] board,int wi,int b)
        {
            try
            {
                StreamWriter w = new StreamWriter(path);
                w.WriteLine(size);
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        if (j != size - 1)
                        {
                            w.Write(board[i, j] + " ");
                        }
                        else
                        {
                            w.Write(board[i, j]);
                        }
                    }
                    w.WriteLine();
                }
                w.WriteLine(wi);
                w.WriteLine(b);
                w.Close();
            }
            catch
            {
                throw new PushingDataException();
            }
            
        }
    }
}
