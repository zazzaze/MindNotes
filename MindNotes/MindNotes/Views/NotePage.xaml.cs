using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
        private NotePageViewModel _notePageViewModel;
        public NotePage(NoteViewModel noteViewModel)
        {
            InitializeComponent();
            _notePageViewModel = new NotePageViewModel(noteViewModel);
            this.BindingContext = _notePageViewModel;
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
                MediaFile photo = null;
                try
                {
                    photo = await CrossMedia.Current.TakePhotoAsync(mediaOptions);
                }
                catch (Exception)
                {
                    await DisplayAlert("Ошибка", "При попытке открть камеру возникла ошибка", "Попробовать снова");
                }
                if (photo == null)
                    return;
                UploadPhoto(photo);
                return;
            }
            
            await DisplayAlert("Ошибка", "Не удалось получить доступ к камере", "Поробовать снова");
        }

        private async void PickPhotoButton_OnClicked(object sender, EventArgs e)
        {
            if (CrossMedia.Current.IsTakePhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                if (photo == null)
                    return;
                UploadPhoto(photo);
            }
        }

        private async void UploadPhoto(MediaFile photo)
        {
            var uri = new Uri(App.ServerUrl + "files/upload");
            var content = new MultipartFormDataContent();

            content.Add(new StreamContent(photo.GetStream()),
                "\"file\"",
                $"\"{photo.Path}\"");

            var httpClient = new HttpClient();
            var httpResponseMessage = await httpClient.PostAsync(uri, content);
            var result = await httpResponseMessage.Content.ReadAsStringAsync();
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                TextFromPhotoEditor.Text = result;
                TextFromPhotoEditorPopUp.IsVisible = true;
                return;
            }
            await DisplayAlert("Не удалось получить текст с фотографии", result, "Ok");
        }

        private void CancelAddPhotoText(object sender, EventArgs e)
        {
            TextFromPhotoEditorPopUp.IsVisible = false;
            TextFromPhotoEditor.Text = String.Empty;
        }

        private void AddTextFromPhoto(object sender, EventArgs e)
        {
            NoteTextEditor.Text += "\n" + TextFromPhotoEditor.Text;
            CancelAddPhotoText(sender, e);
        }
    }
}