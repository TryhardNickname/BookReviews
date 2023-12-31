﻿namespace BookReviews.Models
{
    //Used as a container about the various "docs" that are search results from the api.
    public class ApiResponse
    {
        public int numFound { get; set; }
        //public int start { get; set; }
        //public bool numfoundexact { get; set; }
        public List<Doc> docs { get; set; }
    }

    //A book, properties are lowercase to match the keys in the json as to map it easier.
    public class Doc
    {
        public string key { get; set; }
        //public string type { get; set; }
        //public List<string> seed { get; set; }
        public string title { get; set; }
        public List<string> author_name { get; set; }
        public List<string> subject { get; set; }
        public List<string> isbn { get; set; }
        public int first_publish_year { get; set; }
        public List<string> publisher { get; set; }
        public string imageUrl { get; set; }
    }

    //A book in the format used in the DB.
    public class Book
    {
        public int Id { get; set; }
        public string Key { get; set; }
        //public string type { get; set; }
        //public List<string> seed { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Subject { get; set; }
        public string Isbn { get; set; }
        public int FirstPublishYear { get; set; }
        public string Publisher { get; set; }
        public string ImageUrl { get; set; }
        public string? Review {  get; set; }
        public int? Score { get; set; }
    }

}
