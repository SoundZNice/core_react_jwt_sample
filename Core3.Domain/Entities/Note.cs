using System;
using System.Collections.Generic;
using System.Text;

namespace Core3.Domain.Entities
{
    public class Note
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public DateTime? DateCreated { get; set; }

        public DateTime? DateModified { get; set; }
    }
}
