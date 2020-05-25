using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindNotes.Models;
using MindNotes.ViewModels;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MindNotes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotePage : ContentPage
    {
        public delegate void BackHandler(NotePage page, EventArgs e);

        public event BackHandler BackButtonClicked;
        public NotePage(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            this.BindingContext = new NotePageViewModel(noteViewModel);
            NavigationPage.SetHasBackButton(this, false);
        }

        protected override bool OnBackButtonPressed()
        {
            BackButtonClicked?.Invoke(this, new EventArgs());
            return base.OnBackButtonPressed();
        }

        private async void TakePhotoButton_OnClicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsPickPhotoSupported)
            {
                var mediaOptions = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "newPhotos",
                    Name = $"{DateTime.UtcNow}.jpg",
                    SaveToAlbum = true
                };
                MediaFile photo = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
            }
        }

        private async void PickPhotoButton_OnClicked(object sender, EventArgs e)
        {
            if (CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
            }
        }
    }
}