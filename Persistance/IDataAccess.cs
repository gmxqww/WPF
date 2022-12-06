using Pushing2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pushing2.Persistance
{
    public interface IDataAccess
    {
        public void SaveGame(string path,int size,FieldState[,] board,int w,int b);
        public PushingTable LoadGame(string path);
    }
}
