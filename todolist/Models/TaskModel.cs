using System;
namespace todolist.Models
{
	public class TaskModel
	{
        public int Id { get; set; }

        public int IntType { get; set; }

        public string Topic { get; set; } = string.Empty;

        public string? Content { get; set; }

        public DateTime DueDate { get; set; } = DateTime.Now;

        public int IntSymbol { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public string UserId { get; set; } = string.Empty;
    }
}

