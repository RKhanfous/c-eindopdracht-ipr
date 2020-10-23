using System;
using System.Collections.Generic;
using System.Text;
using WpfClient.Utils;

namespace WpfClient.Models
{
    class Player : ObservableObject
    {
        public string username { get; set; }
        public uint Id { get; set; }
        public uint score { get; set; }
        public bool IsDrawing { get; set; }
    }
}
