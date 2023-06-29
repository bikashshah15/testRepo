using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TestWeb.API.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }


        [Required]
        public string LastName { get; set; }


        [Required]
        public string Email { get; set; }


        public decimal Salary { get; set; }



        public DateTime JoiningDate { get; set; }

        [JsonIgnore] //To hide from user
        public bool IsActive { get; set; }
    }
}
