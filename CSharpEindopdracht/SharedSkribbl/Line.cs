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

        public static Line getLine(byte[] bytes)
        {
            if (bytes == null || bytes.Length != 14)
                throw new ArgumentNullException("bytes null or not of length 14");
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
    }
}
