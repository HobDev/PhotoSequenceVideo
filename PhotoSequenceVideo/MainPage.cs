using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace PhotoSequenceVideo;

public class MainPage : ContentPage
{
    enum section
    {
        Carousel,
        Button1,
        Button2,
    }

    public MainPage(IMediaPicker mediaPicker, MainViewModel viewModel)
    {
        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                (section.Carousel, 450),
                (section.Button1, Auto),
                (section.Button2, Star)
            ),

            Children =
            {
                new CarouselView
                {
                    IsSwipeEnabled = false,
                    HeightRequest = 400,
                    EmptyView = "No images available",
                    ItemTemplate = new DataTemplate(() =>
                    {
                        var image = new Image();
                        image.Bind(
                            Image.SourceProperty,
                            ".",
                            converter: new ByteArrayToImageSourceConverter()
                        );
                        return image;
                    }),
                }
                    .Bind(ItemsView.ItemsSourceProperty, nameof(viewModel.Images))
                    .Bind(CarouselView.CurrentItemProperty, nameof(viewModel.CurrentImage))
                    .Row(section.Carousel),
                new VerticalStackLayout
                {
                    Children =
                    {
                        new Button
                        {
                            Text = "Add Image",
                            BorderWidth = 1,
                            CornerRadius = 5,
                            BackgroundColor = Colors.LightYellow,
                            Padding = new Thickness(20, 10),
                            TextColor = Colors.Black,
                            FontSize = 20,
                        }
                            .Bind(Button.CommandProperty, nameof(viewModel.PickMediaCommand))
                            .CenterHorizontal()
                            .CenterVertical(),
                        new Label { HorizontalOptions = LayoutOptions.Center, FontSize = 16 }
                            .Bind(Label.TextProperty, nameof(viewModel.ImageCount), stringFormat: "Added Images: {0}")
                            .CenterHorizontal()
                            .CenterVertical(),
                    },
                }
                    .Row(section.Button1)
                    .CenterHorizontal()
                    .CenterVertical(),
                new VerticalStackLayout
                {
                    Children =
                    {
                        new HorizontalStackLayout
                        {
                            Spacing = 20,
                            Children =
                            {
                                new Button
                                {
                                    Text = "Start Show",
                                    BorderWidth = 1,
                                    CornerRadius = 5,
                                    BackgroundColor = Colors.LightYellow,
                                    Padding = new Thickness(20, 10),
                                    TextColor = Colors.Black,
                                    FontSize = 20,
                                }
                                    .Bind(Button.CommandProperty, nameof(viewModel.ShowCommand))
                                    .CenterHorizontal()
                                    .CenterVertical(),
                                new Button
                                {
                                    Text = "Stop Show",
                                    BorderWidth = 1,
                                    CornerRadius = 5,
                                    BackgroundColor = Colors.LightYellow,
                                    Padding = new Thickness(20, 10),
                                    TextColor = Colors.Black,
                                    FontSize = 20,
                                }
                                    .Bind(Button.CommandProperty, nameof(viewModel.StopShowCommand))
                                    .CenterHorizontal()
                                    .CenterVertical(),
                            },
                        },
                        new Label
                        {
                            Text = "Add 3 to 5 images to see the slideshow",
                            HorizontalOptions = LayoutOptions.Center,
                            FontSize = 16,
                        },
                    },
                }
                    .Row(section.Button2)
                    .CenterHorizontal()
                    .CenterVertical(),
            },
        };

        BindingContext = viewModel;
    }
}
