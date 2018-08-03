using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.Models
{
	public class PersonModel
	{
		/// <summary>
		/// General Constructor
		/// </summary>
		/// <param name="iD"></param>
		/// <param name="firstName"></param>
		/// <param name="lastName"></param>
		/// <param name="phone"></param>
		/// <param name="email"></param>
		/// <param name="created"></param>
		/// <param name="updated"></param>

		public PersonModel(int iD, string firstName, string lastName, string phone, string email, DateTime created, DateTime? updated)
		{
			ID = iD;
			FirstName = firstName;
			LastName = lastName;
			Phone = phone;
			Email = email;
			Created = created;
			Updated = updated;
		}

		public PersonModel()
		{
			Created = DateTime.Now;
			Updated = DateTime.Now;
		}


		[Required]
		public int ID { get; set; }

		[Required]
		[MaxLength(32)]
		public string FirstName { get; set; }

		[Required]
		[MaxLength(64)]
		public string LastName { get; set; }

		[Required]
		[MaxLength(25)]
		public string Phone { get; set; }

		[EmailAddress]
		public string Email { get; set; }

		public DateTime Created { get; set; }

		public DateTime? Updated { get; set; }



	}
}
