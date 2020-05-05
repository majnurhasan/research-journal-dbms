using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int AuthorId { get; set; }
        public string Message { get; set; }
        public Author Author { get; set; }
    }
}
