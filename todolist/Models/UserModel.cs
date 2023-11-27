using SQLite;

namespace todolist.Models
{
	public class UserModel
	{
        [PrimaryKey]
        public int Id { get; set; }

        public string JwtToken { get; set; }
    }
}

