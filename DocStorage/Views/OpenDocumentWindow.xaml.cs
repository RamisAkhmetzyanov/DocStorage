using System.Windows;
namespace DocStorage.Views
{
    /// <summary>
    /// Interaction logic for OpenDocumentWindow.xaml
    /// </summary>
    public partial class OpenDocumentWindow : Window
    {
        public OpenDocumentWindow()
        {
            InitializeComponent();
            Data.DataModels.Document selectedDocument = Document.SelectedDocument;
            if (selectedDocument != null)
            {
                DocNameLbl.Content = selectedDocument.name;
                DocContentTxt.Text = selectedDocument.text;
                UserTxt.Text = selectedDocument.user;
                CreateDateTxt.Text = selectedDocument.createDate.Date.ToString().Substring(0, 10);
                TagsTxt.Text = selectedDocument.tags;
            }

        }
    }
}
