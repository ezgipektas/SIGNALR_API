using System.Collections.Generic;
using System.Collections;

namespace İEA_SİGNALR_API.Models
{
    public class Room
    {
        public Room()
        {
            Users = new List<User>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
