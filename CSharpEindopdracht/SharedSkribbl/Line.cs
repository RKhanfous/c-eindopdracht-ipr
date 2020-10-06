using System;
using System.Collections.Generic;
using System.Text;

namespace SharedSkribbl
{
    struct Line
    {
        public int BeginX { get; set }
        public int BeginY { get; set }
        public int EndX { get; set }
        public int EndY { get; set }
        public int StrokeWidth { get; set; }
    }
}
