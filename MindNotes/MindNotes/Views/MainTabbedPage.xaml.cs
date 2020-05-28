using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindNotes.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindNotes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainTabbedPage : TabbedPage
    {
        public MainTabbedPage()
        {
            InitializeComponent();
            NotesListViewModel notesListViewModel = new NotesListViewModel
            {
                Navigation = this.Navigation
            };
            Children.Add(new AllNotesPage(notesListViewModel));
            Children.Add(new MindMap(notesListViewModel));
        }
    }
}