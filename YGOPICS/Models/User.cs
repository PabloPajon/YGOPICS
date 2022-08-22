using System;
using System.Collections.Generic;

namespace YGOPICS.Models
{
    public partial class User
    {
        public int Userid { get; set; }
        public string? Mail { get; set; }
        public string? Password { get; set; }
        public string Name { get; set; }
    }
}
