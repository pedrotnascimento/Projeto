using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DataAccessLayer
{
    public class MessageDAL
    {
        public UserDAL User { get; set; }
        public ChatRoomDAL ChatRoom { get; set; }
        public DateTime DateTime { get; set; }
        public string Payload { get; set; }
    }
}
