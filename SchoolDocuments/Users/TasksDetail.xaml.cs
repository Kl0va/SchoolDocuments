using SchoolDocuments.Moduls;
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

namespace SchoolDocuments.Users
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class TasksDetail : Page
    {
        public TasksDetail()
        {
            this.InitializeComponent();
        }

        private static Models.Task task;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            task = e.Parameter as Models.Task;

            pageHeader.Text = task.title;
            Description.Text = task.desc;

            foreach(Models.Performer performer in task.performs)
            {
                Agreement.Items.Add(performer.user.firstName + " " + performer.user.secondName + " " + performer.user.middleName);
            }
        }

        private void Agreement_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (Models.Performer performer in task.performs)
            {
                if (Agreement.SelectedItem.ToString() == performer.user.firstName + " " + performer.user.secondName + " " + performer.user.middleName)
                {
                    if(performer.status == Models.PerformerStatus.InProgress)
                    {
                        perfStatus.Text = "В процессе";
                    }
                    else if(performer.status == Models.PerformerStatus.Completed)
                    {
                        perfStatus.Text = "Выполнено";
                    }
                    else if(performer.status == Models.PerformerStatus.Waiting)
                    {
                        perfStatus.Text = "В ожидании";
                    }
                    if (performer.comment == "" || performer.comment == null)
                    {
                        perfComment.Text = "Комментария не было оставлено";
                    }
                    else
                    {
                        perfComment.Text = performer.comment;
                    }
                }
            }
        }

        private async void save_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog errorDialog = new ContentDialog()
            {
                Title = "Удаление",
                Content = "Вы точно хотите удалить задание ?",
                PrimaryButtonText = "Ok",
                SecondaryButtonText = "Отмена"
            };

            ContentDialogResult result = await errorDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ApiWork.DeleteTask(task);
                Frame.GoBack();
            }
        }
    }
}
