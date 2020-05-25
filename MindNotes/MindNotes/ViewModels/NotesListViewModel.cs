using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MindNotes.Annotations;
using MindNotes.Models;
using MindNotes.Views;
using Xamarin.Forms;

namespace MindNotes.ViewModels
{
    public class NotesListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<NoteViewModel> Notes { get; set; }
        
        public ICommand CreateNoteCommand { protected set; get; }
        public ICommand SaveNoteCommand { protected set; get; }
        public ICommand DeleteNoteCommand { protected set; get; }
        private NoteViewModel _selectedNote;
        
        public INavigation Navigation { get; set; }


        public NotesListViewModel()
        {
            Notes = new ObservableCollection<NoteViewModel>();
            CreateNoteCommand = new Command(CreateNote, () => true);
            SaveNoteCommand = new Command(SaveNote);
        }

        public NoteViewModel SelectedNote
        {
            get => _selectedNote;
            set
            {
                if (_selectedNote != value)
                {
                    NoteViewModel currentNote = value;
                    _selectedNote = null;
                    OnPropertyChanged("SelectedNote");
                    Navigation.PushAsync(new NotePage(currentNote));
                }
            }
        }

        private async void CreateNote()
        {
            NoteViewModel newNote = new NoteViewModel
            {
                ListViewModel = this
            };
            Notes.Insert(0,newNote);
            NotePage notePage= new NotePage(newNote);
            await Navigation.PushAsync(notePage);
        }

        public void Back()
        {
            Navigation.PopAsync();
        }

        private void SaveNote(object noteViewModelObj)
        {
            NoteViewModel noteViewModel = noteViewModelObj as NoteViewModel;
            if (noteViewModel == null)
                return;
            if (Notes.Contains(noteViewModel))
            {
                if (!noteViewModel.IsValid)
                    Notes.Remove(noteViewModel);
                Back();
                return;
            }
            if(noteViewModel.IsValid)
                Notes.Insert(0, noteViewModel);
            Back();
        }
        
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}