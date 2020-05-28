using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MindNotes.Annotations;

namespace MindNotes.ViewModels
{
    public class MindMapNotesViewModel : INotifyPropertyChanged
    {
        private MindViewModel _center;

        private NotesListViewModel _lvm;
        public MindMapNotesViewModel(NotesListViewModel lvm)
        {
            _lvm = lvm;
        }

        public NotesListViewModel ListViewModel => _lvm;
        
        public MindViewModel Center
        {
            get => _center;
            set
            {
                if (value == _center)
                    return;
                _center = value;
                OnPropertyChanged("Center");
            }
        }

        public Boolean IsCenterUnenabled => _center == null;
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}