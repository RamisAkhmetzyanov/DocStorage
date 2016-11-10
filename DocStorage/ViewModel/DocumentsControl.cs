using DocStorage.Data.DataModels;
using DocStorage.Extensions;
using DocStorage.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DocStorage.ViewModel
{
    public class DocumentsControl
    {
        //Методы для Базы Данных
        public static bool CreateNewDocument(string name, string tags, string text, out string responseMessage)
        {
            DateTime createDate = DateTime.Now;
            User user = Authentication.GetCurrentUser();
            Document doc = GetNewDocument(name, createDate, user.name, tags, text);
            if (doc == null)
            {
                responseMessage = "Документ с таким названием уже существует";
                return false;
            }
            if (ExtensionFactory.DataContext.SaveDocument(doc))
            {
                responseMessage = "Документ сохранен";
                return true;
            }
            else
            {
                responseMessage = "Не удалось сохранить документ";
                return false;
            }
        }

        public static Document GetNewDocument(string name, DateTime createDate, string user, string tags, string text)
        {
            if (ExtensionFactory.DataContext.Documents.Where(d => d.name.Equals(name)).Any())
                return null;
            var newDoc = ExtensionFactory.DataContext.Documents.Create();
            newDoc.name = name;
            newDoc.user = user;
            newDoc.createDate = createDate;
            newDoc.tags = tags;
            newDoc.text = text;
            return newDoc;
        }

        public static List<Document> GetAllDocuments()
        {
            return ExtensionFactory.DataContext.Documents.ToList<Document>();
        }
        
        public static List<Document> SearchDocuments(string docName, string userName, string dateString, string tags)
        {
            DateTime createDate;
            if (dateString != "")
                createDate = Convert.ToDateTime(dateString);
            else
                createDate = DateTime.Now;
            List<string> tagsList = tagsToList(tags);
            var searchResult = ExtensionFactory.DataContext.Documents.Where(d => (docName == "" || d.name.Contains(docName)) &&
                (userName == "" || d.user.Contains(userName))).Cast<Document>().Where(d =>
                (dateString == "" || d.createDate.ToString().Substring(0, 10) == createDate.ToString().Substring(0, 10)) &&
                (tags == "" || !tagsList.Where(t => !d.tags.Contains(t)).Any()));
            if (searchResult == null)
            {
                return new List<Document>();
            }
            return searchResult.ToList<Document>();
        }

        //Методы для XML файлов стандартно
        public static bool CreateNewDocumentAsXMLByStandard(string name, string tags, string text, out string responseMessage)
        {
            DateTime createDate = DateTime.Now;
            User user = Authentication.GetCurrentUser();
            Document doc = GetNewDocumentAsXML(name, createDate, user.name, tags, text);
            if (GetAllDocumentsFromXMLByStandard().Where(d => d.name.Equals(name)).Any())
                doc = null;
            if (doc == null)
            {
                responseMessage = "Документ с таким названием уже существует";
                return false;
            }
            if (SaveDocumentToXMLByStandard(doc))
            {
                responseMessage = "Документ сохранен";
                return true;
            }
            else
            {
                responseMessage = "Не удалось сохранить документ";
                return false;
            }
        }

        public static Document GetNewDocumentAsXML(string name, DateTime createDate, string user, string tags, string text)
        {
            var newDoc = ExtensionFactory.DataContext.Documents.Create();
            newDoc.name = name;
            newDoc.user = user;
            newDoc.createDate = createDate;
            newDoc.tags = tags;
            newDoc.text = text;
            return newDoc;
        }

        public static bool SaveDocumentToXMLByStandard(Document document)
        {
            XDocument xdoc = new XDocument(
                new XElement("document",
                    new XAttribute("text", document.text),
                    new XAttribute("tags", document.tags),
                    new XAttribute("user", document.user),
                    new XAttribute("createDate", document.createDate),
                    new XAttribute("name", document.name)
                )
                );
            try
            {
                string path = Directory.GetCurrentDirectory() + @"\XML\" + document.name + ".xml";
                xdoc.Save(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<Document> GetAllDocumentsFromXMLByStandard()
        {
            string path = Directory.GetCurrentDirectory() + @"\XML\";
            string[] files = Directory.GetFiles(path, "*.xml");
            List<XDocument> xdocs = new List<XDocument>();
            try
            {
                foreach (string file in files)
                    xdocs.Add(XDocument.Load(file));
            }
            catch
            { }
            List<Document> docs = new List<Document>();
            foreach (XDocument xdoc in xdocs)
            {
                try
                {
                    var elements = xdoc.Elements().First().Attributes().ToList<XAttribute>();
                    docs.Add(GetNewDocumentAsXML(elements[4].Value, Convert.ToDateTime(elements[3].Value),
                        elements[2].Value, elements[1].Value, elements[0].Value));
                }
                catch { }
            }

            return docs;
        }

        public static List<Document> SearchDocumentsFromXMLByStandard(string docName, string userName, string dateString, string tags)
        {
            DateTime createDate;
            if (dateString != "")
                createDate = Convert.ToDateTime(dateString);
            else
                createDate = DateTime.Now;
            List<string> tagsList = tagsToList(tags);
            var searchResult = GetAllDocumentsFromXMLByStandard().Where(d => (docName == "" || d.name.Contains(docName)) &&
                (userName == "" || d.user.Contains(userName))).Cast<Document>().Where(d =>
                (dateString == "" || d.createDate.ToString().Substring(0, 10) == createDate.ToString().Substring(0, 10)) && 
                (tags == "" || !tagsList.Where(t => !d.tags.Contains(t)).Any()));
            if (searchResult == null)
            {
                return new List<Document>();
            }
            return searchResult.ToList<Document>();
        }

        //Методы для XML файлов через ORM

        
        
        //Вспомагательные методы
        public static List<string> tagsToList(string tags)
        {
            List<string> tagsList = new List<string>();
            string st = tags;
            bool converted = false;
            int i = st.IndexOf(','), j = 1;
            if (tags == "") converted = true;
            while (!converted)
            {
                i = st.IndexOf(',');
                if (i <= 0)
                {
                    tagsList.Add(st);
                    converted = true;
                }
                else
                {
                    tagsList.Add(st.Substring(j - 1, i - j + 1));
                    st = st.Substring(i + 1);
                }
            }
            return tagsList;
        }
        
    }

    public class DocumentView
    {
        public string name { get; set; }
        public string createDate { get; set; }
        public string user { get; set; }
        public string tags { get; set; }
        public string openBtn { get; set; }

        public DocumentView(Document doc)
        {
            this.name = doc.name;
            this.createDate = doc.createDate.Date.ToString().Substring(0, 10);
            this.user = doc.user;
            this.tags = doc.tags;
            this.openBtn = "Открыть документ";
        }

        public static List<DocumentView> GetView(List<Document> docs)
        {
            List<DocumentView> docsView = new List<DocumentView>();
            foreach (Data.DataModels.Document doc in docs)
            {
                docsView.Add(new DocumentView(doc));
            }
            return docsView;
        }

        public static int OpenBtnIndex()
        {
            return 4;
        }
    }
}