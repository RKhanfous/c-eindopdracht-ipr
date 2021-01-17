using System;
using System.Collections.Generic;
using System.Text;
using WpfClient.Utils;

namespace WpfClient.Models
{
    class Player : ObservableObject
    {
        public string Username { get; set; }
        public uint Id { get; set; }
        public uint Score { get; set; }
        public bool IsDrawing { get; set; }
        public PenState PenState { get; set; } = PenState.STROKE1;
    }
}
