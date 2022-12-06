using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pushing2.Eventargs
{
    public class GameFinishedEvenetArgs
    {
        public GameFinishedEvenetArgs(int w, int b)
        {
            W = w;
            B = b;
        }

        public int W  { get; set; }
        public int B { get; set; }

    }
}
