using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace MindNotes.ViewModels
{
    public class MindViewModel
    {
        private NoteViewModel _current;
        private List<MindViewModel> _childs = new List<MindViewModel>();
        private Frame _currentFrame;

        public MindViewModel(NoteViewModel current)
        {
            _current = current;
        }

        public String Title => _current.Title;

        public NoteViewModel Current => _current;

        public List<MindViewModel> Childs => _childs;

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
    }
}