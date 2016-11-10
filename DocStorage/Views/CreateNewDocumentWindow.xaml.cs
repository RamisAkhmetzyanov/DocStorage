using DocStorage.ViewModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace DocStorage.Views
{
    /// <summary>
    /// Interaction logic for CreateNewDocumentWindow.xaml
    /// </summary>
    public partial class CreateNewDocumentWindow : Window
    {
        List<string> CreatingMethods = new List<string>();

        public CreateNewDocumentWindow()
        {
            InitializeComponent();
            CreatingMethods.Add("База данных");
            CreatingMethods.Add("Файл XML");
            CreatingMethodComboBox.ItemsSource = CreatingMethods;
        }

        private void SaveDocBtnOnClick(object sender, RoutedEventArgs e)
        {
            string docName = DocNameTxt.Text,
                docTags = TagsTxt.Text,
                docText = new TextRange(DocContentTxt.Document.ContentStart, DocContentTxt.Document.ContentEnd).Text;
            if (CreatingMethodComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите способ создания");
            }
            else
            {
                string responseMessage;
                if (CreatingMethodComboBox.SelectedIndex == 0)
                {
                    DocumentsControl.CreateNewDocument(docName, docTags, docText, out responseMessage);
                    MessageBox.Show(responseMessage);
                }
                if (CreatingMethodComboBox.SelectedIndex == 1)
                {
                    DocumentsControl.CreateNewDocumentAsXMLByStandard(docName, docTags, docText, out responseMessage);
                    MessageBox.Show(responseMessage);
                }
            }
        }

    }
}