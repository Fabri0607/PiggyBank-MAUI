using Markdig;
using System.Globalization;

namespace PiggyBank_MAUI.Converters
{
    public class MarkdownToHtmlConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var markdown = value as string;
            if (string.IsNullOrEmpty(markdown))
            {
                return string.Empty;
            }

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
            var html = Markdown.ToHtml(markdown, pipeline);
            return $@"
<html>
    <head>
        <style>
            html, body {{
                border: 0 !important;
                margin: 0 !important;
                padding: 0 !important;
                background-color: transparent !important;
                
            }}
        </style>
    </head>
    <body>
        {html}
    </body>
</html>";

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
