using System;

namespace DocStorage.Data.DataModels
{
    public class Document
    {
        public int Id { get; private set; }

        public string name { get; set; }

        public DateTime createDate { get; set; }

        public string user { get; set; }

        public string tags { get; set; }

        public string text { get; set; }

    }
}
