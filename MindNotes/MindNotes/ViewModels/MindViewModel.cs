using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MindNotes.ViewModels
{
    public class MindViewModel
    {
        private NoteViewModel _current;
        private List<MindViewModel> _childs = new List<MindViewModel>();
        private MindViewModel _parent;
        private Frame _currentFrame;
        private MindMapNotesViewModel _currentMvm;

        public MindViewModel(NoteViewModel current, MindMapNotesViewModel mvm)
        {
            _current = current;
            _currentMvm = mvm;
        }

        public String Title => _current.Title;

        public NoteViewModel Current => _current;

        public List<MindViewModel> Childs => _childs;

        public MindViewModel Parent
        {
            get => _parent;
            set => _parent = value;
        }

        public void Add(MindViewModel newMind)
        {
            if(_childs.Contains(newMind))
                return;
            _childs.Add(newMind);
        }

        public Frame CurrentFrame
        {
            get => _currentFrame;
            set => _currentFrame = value;
        }

        public void DeleteFromChildren(NoteViewModel deletedNote)
        {
            if (_childs == null || _childs.Count == 0)
                return;
            foreach (var child in _childs)
            {
                if (child.Current == deletedNote)
                {
                    _childs.Remove(child);
                    continue;
                }
                child.DeleteFromChildren(deletedNote);
            }
        }
    }
}