using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MindNotes.Annotations;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace MindNotes.ViewModels
{
    public class NotePageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand BackCommand { get; protected set; }
        private NoteViewModel _note;
        public NotePageViewModel(NoteViewModel noteViewModel)
        {
            _note = noteViewModel;
            BackCommand = new Command(Back);
        }

        public String Title
        {
            get => _note.Note.Title;
            set { _note.Title = value; }
        }

        public String Text
        {
            get => _note.Text;
            set => _note.Text = value;
        }

        private void Back()
        {
            if (!_note.IsValid)
                _note.ListViewModel.Notes.Remove(_note);
            _note.ListViewModel.Back();
        }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}