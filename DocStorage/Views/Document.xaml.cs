using DocStorage.ViewModel;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DocStorage.Views
{
    /// <summary>
    /// Interaction logic for Document.xaml
    /// </summary>
    public partial class Document : Window
    {
        private List<Data.DataModels.Document> ViewedDocuments;
        public static Data.DataModels.Document SelectedDocument;
        private List<string> SearchSources = new List<string>();

        public Document()
        {
            InitializeComponent();
            SearchSources.Add("Базe данных");
            SearchSources.Add("Файлах XML");
            SearchSourceComboBox.ItemsSource = SearchSources;
        }

        private void CreateDocBtnOnClick(object sender, RoutedEventArgs e)
        {
            CreateNewDocumentWindow createNewDocWindow = new CreateNewDocumentWindow();
            createNewDocWindow.ShowDialog();
        }

        private void SearchDocBtnOnClick(object sender, RoutedEventArgs e)
        {
            if (SearchSourceComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите источник документов");
            }
            if (SearchSourceComboBox.SelectedIndex == 0)
            {
                var documents = DocumentsControl.SearchDocuments(NameTxt.Text, UserTxt.Text, CreateDateTxt.Text, TagsTxt.Text);
                SetDataGridSource(DocumentsGrid, documents);
            }
            if (SearchSourceComboBox.SelectedIndex == 1)
            {
                var documents = DocumentsControl.SearchDocumentsFromXMLByStandard(NameTxt.Text, UserTxt.Text, CreateDateTxt.Text, TagsTxt.Text);
                SetDataGridSource(DocumentsGrid, documents);
            }
        }

        private void ShowAllDocsBtnOnClick(object sender, RoutedEventArgs e)
        {

            if (SearchSourceComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите источник документов");
            }
            if (SearchSourceComboBox.SelectedIndex == 0)
            {
                var documents = DocumentsControl.GetAllDocuments();
                SetDataGridSource(DocumentsGrid, documents);
            }

            if (SearchSourceComboBox.SelectedIndex == 1)
            {
                var documents = DocumentsControl.GetAllDocumentsFromXMLByStandard();
                SetDataGridSource(DocumentsGrid, documents);
            }
        }
        
        private void SetDataGridSource(DataGrid dataGrid, List<Data.DataModels.Document> source)
        {
            if (source != null && source.Count != 0)
            {
                ViewedDocuments = source;
                dataGrid.ItemsSource = DocumentView.GetView(source);
            }
            else
            {
                ViewedDocuments = new List<Data.DataModels.Document>();
                dataGrid.ItemsSource = new List<DocumentView>();
            }
            dataGrid.Columns[0].Header = "Имя";
            dataGrid.Columns[1].Header = "Дата создания";
            dataGrid.Columns[2].Header = "Автор";
            dataGrid.Columns[3].Header = "Тэги";
            dataGrid.Columns[4].Header = "";
            dataGrid.Columns[0].Width = 80;
            dataGrid.Columns[1].Width = 100;
            dataGrid.Columns[2].Width = 100;
            dataGrid.Columns[3].Width = 170;
            dataGrid.Columns[4].Width = 110;
            dataGrid.Columns[4].CanUserReorder = false;
        }
        
        private void DocumentsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dep = (DependencyObject)e.OriginalSource;
            
            while ((dep != null) && !(dep is DataGridCell))
            {
                dep = VisualTreeHelper.GetParent(dep);
            }

            if (dep == null)
                return;
            else
            {
                DataGridCell cell = dep as DataGridCell;

                while ((dep != null) && !(dep is DataGridRow))
                {
                    dep = VisualTreeHelper.GetParent(dep);
                }

                DataGridRow row = dep as DataGridRow;
                DataGrid dataGrid =
                ItemsControl.ItemsControlFromItemContainer(row) as DataGrid;

                int rowIndex = dataGrid.ItemContainerGenerator.IndexFromContainer(row);

                int columnIndex = cell.Column.DisplayIndex;
                
                if (columnIndex == DocumentView.OpenBtnIndex())
                {
                    SelectedDocument = ViewedDocuments[rowIndex];
                    OpenDocumentWindow openDocWindow = new OpenDocumentWindow();
                    openDocWindow.ShowDialog();
                }
            }
        }
    }
}
