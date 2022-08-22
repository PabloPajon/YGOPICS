using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace YGOPICS.Models
{
    [Keyless]
    public partial class Comment
    {
        
        public int Cardid { get; set; }
        public string Text { get; set; } = null!;
        public int Userid { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
