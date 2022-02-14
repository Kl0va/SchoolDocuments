using SchoolDocuments.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class AdminPage : Page
    {
        public AdminPage()
        {
            this.InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (documents.IsSelected)
            {
                NavigateDocuments();
            }
            else if (templates.IsSelected)
            {
                NavigateTemplates();
            }
        }

        private void NavigateDocuments()
        {
            myFrame.Navigate(typeof(DocumentsPage), Frame);
            pageHeader.Text = "Документы";
        }

        private void NavigateTemplates()
        {
            myFrame.Navigate(typeof(TemplatesPage), Frame);
            pageHeader.Text = "Шаблоны документов";
        }


        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            NavigateDocuments();
        }
    }
}
