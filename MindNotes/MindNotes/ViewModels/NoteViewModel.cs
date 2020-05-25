using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MindNotes.Annotations;
using MindNotes.Models;

namespace MindNotes.ViewModels
{
    public class NoteViewModel : INotifyPropertyChanged
    {
        private const String EmptyTitleText = "Без названия";
        public event PropertyChangedEventHandler PropertyChanged;
        private NotesListViewModel _lvm;
        public Note Note { get; private set; }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public NoteViewModel()
        {
            Note = new Note();
        }

        public NotesListViewModel ListViewModel
        {
            get => _lvm;
            set
            {
                if (_lvm != value)
                {
                    _lvm = value;
                    OnPropertyChanged("ListViewModel");
                }
            }
        }

        public String Title
        {
            get => String.IsNullOrWhiteSpace(Note.Title) ? "Без названия" : Note.Title;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    Note.Title = EmptyTitleText;
                else
                    Note.Title = value;
                OnPropertyChanged("Title");
            }
        }

        public String Text
        {
            get => Note.Text;
            set => Note.Text = value;
        }

        public Boolean IsValid => !(Title == EmptyTitleText && String.IsNullOrWhiteSpace(Text));
    }
}