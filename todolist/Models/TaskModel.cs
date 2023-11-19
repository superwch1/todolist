using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
namespace todolist.Models
{
	public class TaskModel : INotifyPropertyChanged
	{
        public int Id { get; set; }
        public int IntType { get; set; }
        public string Topic { get; set; } = string.Empty;
        public string? Content { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now;
        public int IntSymbol { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string UserId { get; set; } = string.Empty;


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public bool IsLate
        {
            get
            {
                if (IntSymbol == 0 && DateTime.Today > DueDate)
                    return true;
                return false;
            }
        }

        bool isContentVisible = false;
        public bool IsContentVisible
        {
            get => isContentVisible;
            set
            {
                if (isContentVisible == value) return;

                isContentVisible = value;
                OnPropertyChanged();
            }
        }
    }
}

