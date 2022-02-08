using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace SchoolDocuments.Admin
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateTemplatePage : Page
    {
        public CreateTemplatePage()
        {
            this.InitializeComponent();
        }

        public static WordDocument document = new WordDocument();
        public static WSection section = document.AddSection() as WSection;
        private async void save_Click(object sender, RoutedEventArgs e)
        {
            //Отступы
            section.PageSetup.Margins.All = 72;
            //размер окна документа
            section.PageSetup.PageSize = new Syncfusion.DocIO.DLS.SizeF(612, 792);

            //Создание стиля
            WParagraphStyle style = document.AddParagraphStyle("Normal") as WParagraphStyle;
            style.CharacterFormat.FontName = "Times New Roman";
            style.CharacterFormat.FontSize = 12f;
            style.ParagraphFormat.BeforeSpacing = 0;
            style.ParagraphFormat.LineSpacing = 13.8f;
            //Создание стиля 
            style = document.AddParagraphStyle("Heading 1") as WParagraphStyle;
            style.ApplyBaseStyle("Normal");
            style.CharacterFormat.FontName = "Calibri Light";
            style.CharacterFormat.FontSize = 16f;
            style.CharacterFormat.TextColor = Syncfusion.DocIO.DLS.Color.FromArgb(46, 116, 181);
            style.ParagraphFormat.BeforeSpacing = 12;
            style.ParagraphFormat.AfterSpacing = 0;
            style.ParagraphFormat.Keep = true;
            style.ParagraphFormat.KeepFollow = true;
            style.ParagraphFormat.OutlineLevel = OutlineLevel.Level1;
            //Создание параграфа
            IWParagraph paragraph = section.HeadersFooters.Header.AddParagraph();
            paragraph.ApplyStyle("Normal");
            paragraph.ParagraphFormat.HorizontalAlignment = Syncfusion.DocIO.DLS.HorizontalAlignment.Left;
            paragraph = section.AddParagraph();
            //Добавление картинки
            Stream imageStream = this.GetType().Assembly.GetManifestResourceStream("Documents.Assets.Header.png");
            IWPicture picture = paragraph.AppendPicture(imageStream);
            picture.TextWrappingStyle = TextWrappingStyle.InFrontOfText;
            picture.VerticalOrigin = VerticalOrigin.Margin;
            picture.VerticalPosition = -45;
            picture.HorizontalOrigin = HorizontalOrigin.Column;
            picture.HorizontalPosition = 0f;
            picture.WidthScale = 30;
            picture.HeightScale = 27;
            paragraph = section.AddParagraph();
            string text = "";
            //Добавление текста
            TemplateText.Document.GetText(Windows.UI.Text.TextGetOptions.None, out text);
            string typeName = pageHeader.Text;
            string inputText = typeName + "\n" + text;
            TemplateText.Document.SetText(TextSetOptions.FormatRtf, inputText);
            WTextRange textRange = new WTextRange(document);
            textRange = paragraph.AppendText("\n\n" + inputText) as WTextRange;
            textRange.CharacterFormat.FontSize = 12f;

            MemoryStream stream = new MemoryStream();

            //Saves the Word document to MemoryStream
            await document.SaveAsync(stream, FormatType.Docx);

            //Saves the stream as Word document file in local machine
            Save(stream, "Sample.docx");
        }

        async void Save(MemoryStream streams, string filename)
        {
            streams.Position = 0;
            StorageFile stFile;
            if (!(Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons")))
            {
                FileSavePicker savePicker = new FileSavePicker();
                savePicker.DefaultFileExtension = ".docx";
                savePicker.SuggestedFileName = filename;
                savePicker.FileTypeChoices.Add("Word Documents", new List<string>() { ".docx" });
                stFile = await savePicker.PickSaveFileAsync();
            }
            else
            {
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                stFile = await local.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            }
            if (stFile != null)
            {
                using (IRandomAccessStream zipStream = await stFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (Stream outstream = zipStream.AsStreamForWrite())
                    {
                        byte[] buffer = streams.ToArray();
                        outstream.Write(buffer, 0, buffer.Length);
                        outstream.Flush();
                    }
                }
            }
            await Windows.System.Launcher.LaunchFileAsync(stFile);
        }
    }
}
