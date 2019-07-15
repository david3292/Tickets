using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tickets.Models
{
    public class Client
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string nameQueue { get; set; }

        [Required]
        public string idClient { get; set; }

        [Required]
        public string nameCient { get; set; }

        [Required]
        public bool attended { get; set; }

    }
}
