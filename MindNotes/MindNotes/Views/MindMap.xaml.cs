using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MindNotes.ViewModels;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using Xamarin.Forms.Xaml;

namespace MindNotes.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MindMap : ContentPage
    {
        private MindMapNotesViewModel vm;
        private const Double radius = 150;
        private MindViewModel addTo;
        public MindMap(NotesListViewModel lvm)
        {
            InitializeComponent();
            vm = new MindMapNotesViewModel(lvm);
            this.BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (vm.Center == null)
                return;
            absoluteLayout.Children.Clear();
            DrawCenter(vm.Center);
            DrawChildren(vm.Center);
            // Label center = new Label
            // {
            //     Text = "Center",
            //     Padding = 5
            // };
            // Frame centerFrame = new Frame
            // {
            //     CornerRadius = 5,
            //     BorderColor = Color.Gray,
            //     Padding = 0,
            //     BackgroundColor = Color.White
            // };
            // center.FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label));
            // centerFrame.Content = center;
            // absoluteLayout.Children.Add(centerFrame, new Point(absoluteLayout.Width / 2,
            //      absoluteLayout.Height / 2 ));
            // var tapGestureRecognizer = new TapGestureRecognizer();
            // tapGestureRecognizer.Tapped += OnFrameTapped;
            // centerFrame.GestureRecognizers.Add(tapGestureRecognizer);
            // centerFrame.TranslationX = -centerFrame.Width / 2;
            // centerFrame.TranslationY = -centerFrame.Height / 2;
            // Double x = absoluteLayout.Width / 2;
            // Double y = absoluteLayout.Height / 2;
            // Int32 n = 8;
            // Double angle = 360 / (Double)n;
            // for (int i = 0; i < n; i++)
            // {
            //     Double newAngle = angle*(i+1);
            //     Double x1 = Math.Cos(newAngle*Math.PI/180) * 200;
            //     Double y1 = Math.Sin(newAngle*Math.PI/180) * 200;
            //     Label label = new Label
            //     {
            //         Text = "label " + (i+1),
            //         Padding = 5,
            //         BackgroundColor = absoluteLayout.BackgroundColor,
            //         FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            //     };
            //     Frame frame = new Frame
            //     {
            //         CornerRadius = 5,
            //         BorderColor = Color.Gray,
            //         Padding = 0
            //     };
            //     frame.Content = label;
            //     frame.GestureRecognizers.Add(tapGestureRecognizer);
            //     BoxView boxView = new BoxView();
            //     boxView.BackgroundColor = Color.Gray;
            //     boxView.HeightRequest = 3;
            //     boxView.WidthRequest = 200;
            //     boxView.AnchorX = boxView.AnchorY = 0;
            //     boxView.Rotation = newAngle;
            //     absoluteLayout.Children.Add(boxView, new Point(x,y));
            //     absoluteLayout.Children.Add(frame, new Point(x+x1, y+y1));
            //     if (x1 < 0)
            //     {
            //         frame.TranslationX = -frame.Width / 2;
            //     }
            //
            //     if (y1 < 0)
            //     {
            //         frame.TranslationY = -frame.Height / 2;
            //     }
            // }
            //
            // absoluteLayout.Children.Remove(centerFrame);
            // absoluteLayout.Children.Add(centerFrame, new Point(absoluteLayout.Width / 2,
            //     absoluteLayout.Height / 2 ));
        }
        private void DrawCenter(MindViewModel center)
        {
            if (center.CurrentFrame != null)
                absoluteLayout.Children.Remove(center.CurrentFrame);
            Frame centerFrame = CreateFrame(center);
            center.CurrentFrame = centerFrame;
            absoluteLayout.Children.Add(centerFrame, new Point(absoluteLayout.Width / 2,
                 absoluteLayout.Height / 2 ));
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += OnFrameTapped;
            centerFrame.GestureRecognizers.Add(tapGestureRecognizer);
            centerFrame.TranslationX = -centerFrame.Width / 2;
            centerFrame.TranslationY = -centerFrame.Height / 2;
        }

        private Frame CreateFrame(MindViewModel forMind)
        {
            Frame frame = new Frame
            {
                CornerRadius = 5,
                BorderColor = Color.Gray,
                Padding = 5,
                BindingContext = forMind
            };
            Label label = new Label
            {
                Text = forMind.Title,
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label))
            };
            frame.Content = label;
            return frame;
        }

        private void DrawChildren(MindViewModel current)
        {
            if (current == null || current.Childs.Count == 0)
                return;
            Double angle = 360.0 / current.Childs.Count;
            Double x = current.CurrentFrame.X;
            Double y = current.CurrentFrame.Y;
            for (int i = 0; i < current.Childs.Count; i++)
            {
                if (current.Childs[i].CurrentFrame!= null && absoluteLayout.Children.Contains(current.Childs[i].CurrentFrame))
                    absoluteLayout.Children.Remove(current.Childs[i].CurrentFrame);
                Double currentAngle = angle * (i + 1);
                Double x1 = Math.Cos(currentAngle*Math.PI/180) * radius;
                Double y1 = Math.Sin(currentAngle*Math.PI/180) * radius;
                Frame frame = CreateFrame(current.Childs[i]);
                current.Childs[i].CurrentFrame = frame;
                var tapGestureRecognizer = new TapGestureRecognizer();
                tapGestureRecognizer.Tapped += OnFrameTapped;
                frame.GestureRecognizers.Add(tapGestureRecognizer);
                BoxView boxView = new BoxView();
                boxView.BackgroundColor = Color.Gray;
                boxView.HeightRequest = 3;
                boxView.WidthRequest = radius;
                boxView.AnchorX = boxView.AnchorY = 0;
                boxView.Rotation = currentAngle;
                absoluteLayout.Children.Add(boxView, new Point(x,y));
                absoluteLayout.Children.Add(frame, new Point(x+x1, y+y1));
                if (x1 < 0)
                {
                    frame.TranslationX = -frame.Width / 2;
                }
            
                if (y1 < 0)
                {
                    frame.TranslationY = -frame.Height / 2;
                }
                DrawChildren(current.Childs[i]);
            }

            absoluteLayout.Children.Remove(current.CurrentFrame);
            absoluteLayout.Children.Add(current.CurrentFrame, new Point(x,y));
        }

        private async void OnFrameTapped(object sender, EventArgs e)
        {
            var action = await DisplayActionSheet("", "Отмена", "", 
                "Открыть", "Добавить", "Удалить");
            MindViewModel mindViewModel = ((Frame)sender).BindingContext as MindViewModel;
            if (mindViewModel == null)
                return;
            switch(action)
            {
                case "Открыть":
                    vm.ListViewModel.SelectedNote = mindViewModel.Current;
                    break;
                case "Добавить":
                    addTo = mindViewModel;
                    SelectNoteView.IsVisible = true;
                    break;
                default:
                    break;
            }
        }

        private void OnSelectCenterTapped(object sender, EventArgs e)
        {
            SelectNoteView.IsVisible = true;
        }

        private void OnPopupTapped(object sender, EventArgs e)
        {
            SelectNoteView.IsVisible = false;
        }

        private void AddNoteClicked(object sender, EventArgs e)
        {
            NoteViewModel selectedNote = NotePicker.SelectedItem as NoteViewModel;
            if (selectedNote == null)
                return;
            if (vm.Center == null)
                vm.Center = new MindViewModel(selectedNote);
            else if(addTo != null)
                addTo.Childs.Add(new MindViewModel(selectedNote));
            else 
                return;
            SelectNoteView.IsVisible = false;
            ChooseCenterButton.IsVisible = false;
            OnAppearing();
        }
    }
}