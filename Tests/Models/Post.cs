using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool Published { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public List<int> ListInt { get; set; }
        public List<string> ListString { get; set; }
        public List<Tag> ListTag { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (ReferenceEquals(obj, this)) return true;

            var post = obj as Post;

            if (post == null) return false;

            return Id == post.Id
                && Title == post.Title
                && Published == post.Published
                && CreatedAt == post.CreatedAt
                && ListInt.SequenceEqual(post.ListInt)
                && ListString.SequenceEqual(post.ListString)
                && ListTag.SequenceEqual(post.ListTag);
        }

        public override int GetHashCode() => Id;
    }
}
