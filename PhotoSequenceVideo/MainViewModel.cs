using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace PhotoSequenceVideo;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<byte[]?>? _images = new ObservableCollection<byte[]?>();

    PeriodicTimer? timer;

    [ObservableProperty]
    int _imageCount;

    [ObservableProperty]
    byte[]? _currentImage;
    private readonly IMediaPicker? _mediaPicker;

    public MainViewModel(IMediaPicker? mediaPicker)
    {
        _mediaPicker = mediaPicker ?? throw new ArgumentNullException(nameof(mediaPicker));
        Images= new ObservableCollection<byte[]?>();
    }

    // Add properties and methods to handle the logic for the main page
    // For example, you might want to add commands for picking media, processing images, etc.

    [RelayCommand]
    // This command can be bound to a button in the MainPage.xaml to trigger media picking
    async Task PickMediaAsync()
    {
        try
        {
            if (Images.Count == 5)
            {
                // Optionally, you can show a message or handle the case when the limit is reached
                await Shell.Current.DisplayAlert(
                    "Limit Reached",
                    "You can only add up to 5 images.",
                    "OK"
                );
                return;
            }
            FileResult? fileResult = await _mediaPicker.PickPhotoAsync(
                new MediaPickerOptions { Title = "Pick a photo" }
            );

            if (fileResult != null)
            {
                // Handle the picked media file
                using Stream? photoStream = await fileResult.OpenReadAsync();
                if (photoStream != null)
                {
                    using MemoryStream memoryStream = new MemoryStream();
                    await photoStream.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;
                    photoStream.Position = 0;
                    byte[] imageBytes = new byte[memoryStream.Length];
                    await memoryStream.ReadAsync(imageBytes, 0, (int)memoryStream.Length);
                    // Add the image bytes to the collection
                    Images.Add(imageBytes);
                    ImageCount = Images.Count;
                    CurrentImage = imageBytes; // Set the current image to the newly added image
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, such as user cancellation or permission issues
        }
    }

    [RelayCommand]
    async Task ShowAsync()
    {
        try
        {
            if(Images.Count == 3)
            {
                await Shell.Current.DisplayAlert(
                    "No Images",
                    "Please add images before starting the slideshow.",
                    "OK"
                );
                return;
            }
            else if (Images.Count < 3)
            {
                await Shell.Current.DisplayAlert(
                    "Not Enough Images",
                    "Please add 3 to 5 images before starting the slideshow.",
                    "OK"
                );
                return;
            }
            timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

            // Show some UI element or perform an action when showing

            while (await timer.WaitForNextTickAsync())
            {
                if (CurrentImage != null)
                {
                    // Update the current image or perform any periodic action
                    // For example, you might want to change the image or update some UI element
                    int currentIndex = Images.IndexOf(CurrentImage);
                    if (currentIndex < Images.Count - 1)
                    {
                        CurrentImage = Images[currentIndex + 1];
                    }
                    else
                    {
                        CurrentImage = Images[0]; // Loop back to the first image
                    }
                }
                else
                {
                    // Handle the case when there are no images
                    CurrentImage = Images.Count > 0 ? Images[0] : null;
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions if needed
        }
    }

    [RelayCommand]
    async Task StopShowAsync()
    {
        // Stop the periodic timer or any ongoing process
        if (timer != null)
        {
            timer.Dispose();
            timer = null;
        }
    }
}
