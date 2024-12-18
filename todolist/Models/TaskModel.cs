﻿using System.ComponentModel;
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

        public string BackgroundColor 
        {
            get
            {
                if (IntSymbol == 0 && DateTime.Today > DueDate) 
                {

                    return "#F5C6CB";
                }
                else 
                {
                    if (IntType == 0)
                        return "#D9E6D3";
                    else 
                        return "#D3DEE6";
                }
            }
        }

        public bool IsDoneIconVisible
        {
            get
            {
                if (IntSymbol == 0) 
                {

                    return false;
                }
                else 
                {
                    return true;
                }
            }
        }

        public bool isContentVisible = false;
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

        public string arrowImageSource = "downarrow";
        public string ArrowImageSource
        {
            get => arrowImageSource;
            set
            {
                if (arrowImageSource == value) return;

                arrowImageSource = value;
                OnPropertyChanged();
            }
        }
    }
}

