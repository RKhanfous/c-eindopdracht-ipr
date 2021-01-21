using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharedSkribbl
{
    public struct Line
    {
        public int Id { get; set; }
        public short X1 { get; set; }
        public short Y1 { get; set; }
        public short X2 { get; set; }
        public short Y2 { get; set; }
        public byte Stroke { get; set; }
        public byte Color { get; set; }

        public byte[] serialize()
        {
            byte[] bytes = new byte[14];

            BitConverter.GetBytes(Id).CopyTo(bytes, 0);
            BitConverter.GetBytes(X1).CopyTo(bytes, 4);
            BitConverter.GetBytes(Y1).CopyTo(bytes, 6);
            BitConverter.GetBytes(X2).CopyTo(bytes, 8);
            BitConverter.GetBytes(Y2).CopyTo(bytes, 10);
            bytes[12] = Stroke;
            bytes[13] = Color;
            return bytes;
        }

        public static Line GetLine(byte[] bytes)
        {
            if (bytes == null || bytes.Length != 14)
                throw new ArgumentNullException("bytes null or not of length 14" + bytes.Length);
            //improvement needed
            return new Line()
            {
                Id = BitConverter.ToInt32(bytes),
                X1 = BitConverter.ToInt16(bytes.Skip(4).ToArray()),
                Y1 = BitConverter.ToInt16(bytes.Skip(6).ToArray()),
                X2 = BitConverter.ToInt16(bytes.Skip(8).ToArray()),
                Y2 = BitConverter.ToInt16(bytes.Skip(10).ToArray()),
                Stroke = bytes[12],
                Color = bytes[13]
            };
        }

        public bool Collide1(Line otherLine)
        {
            float denominator = ((X2 - X1) * (otherLine.Y2 - otherLine.Y1)) - ((Y2 - Y1) * (otherLine.X2 - otherLine.X1));
            float numerator1 = ((Y1 - otherLine.Y1) * (otherLine.X2 - otherLine.X1)) - ((X1 - otherLine.X1) * (otherLine.Y2 - otherLine.Y1));
            float numerator2 = ((Y1 - otherLine.Y1) * (X2 - X1)) - ((X1 - otherLine.X1) * (Y2 - Y1));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            float r = numerator1 / denominator;
            float s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }

        public bool Collide2(Line otherLine)
        {
            short rect1X = Math.Min(X1, X2);
            short rect1Width = Math.Max(X1, X2);
            short rect1Y = Math.Min(Y1, Y2);
            short rect1Height = Math.Max(Y1, Y2);

            short rect2X = Math.Min(otherLine.X1, otherLine.X2);
            short rect2Width = Math.Max(otherLine.X1, otherLine.X2);
            short rect2Y = Math.Min(otherLine.Y1, otherLine.Y2);
            short rect2Height = Math.Max(otherLine.Y1, otherLine.Y2);

            if (rect1X < rect2X + rect2Width &&
                rect1X + rect1Width > rect2X &&
                rect1Y < rect2Y + rect2Height &&
                rect1Y + rect1Height > rect2Y)
            {
                float fC = (X2 * otherLine.Y1) - (Y2 * otherLine.X1); //<0 == to the left, >0 == to the right
                float fD = (X2 * otherLine.Y2) - (Y2 * otherLine.X2);

                if ((fC < 0) && (fD < 0)) //both to the left  -> No Cross!
                    return false;
                if ((fC > 0) && (fD > 0)) //both to the right -> No Cross!
                    return false;

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
