using System;
using System.Collections.Generic;
using System.Text;

namespace SharedSkribbl
{
    struct Line
    {
        public int Id { get; set; }
        public short BeginX { get; set }
        public short BeginY { get; set }
        public short EndX { get; set }
        public short EndY { get; set }
        public byte StrokeWidth { get; set; }
        public byte Color { get; set; }
    }
}
