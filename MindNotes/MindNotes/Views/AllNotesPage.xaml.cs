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
    public partial class AllNotesPage : ContentPage
    {
        public AllNotesPage()
        {
            InitializeComponent();
            this.BindingContext = new NotesListViewModel
            {
                Navigation = this.Navigation
            };
        }
    }
}