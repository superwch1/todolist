using System;
using SQLite;

namespace todolist.Models
{
	public class AccountModel
	{
        [PrimaryKey]
        public int Id { get; set; }

        public string JwtToken { get; set; }
    }
}

