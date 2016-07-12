using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(obj, this)) return true;

            var tag = obj as Tag;

            if (tag == null) return false;

            return Id == tag.Id && Name == tag.Name;
        }

        public override int GetHashCode() => Id;
    }
}
