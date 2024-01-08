using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IUPackageFunctionApp.Models
{
    public class Patient
    {
        public int Id { get; set; }

        public string PatientId { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Gender { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public DateTime PatientCreatedDate { get; set; }

    }
}
