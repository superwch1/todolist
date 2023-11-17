using System;
using SQLite;

namespace todolist.Models
{
	public class AccountInfo
	{
        [PrimaryKey]
        public int Id { get; set; }

        public string JwtToken { get; set; }
    }
}

