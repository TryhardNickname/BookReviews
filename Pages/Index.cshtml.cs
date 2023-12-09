using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace BookReviews.Pages
{
    public class ApiResponse
    {
        public int numFound { get; set; }
        //public int start { get; set; }
        //public bool numfoundexact { get; set; }
        public List<Doc> docs { get; set; }
    }

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
        // Add other properties as needed
    }

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        [BindProperty]
        public string UserInput { get; set; }
        public ApiResponse FetchedData { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://openlibrary.org/search.json?q={UserInput}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                FetchedData = JsonSerializer.Deserialize<ApiResponse>(content);
                if (FetchedData != null && FetchedData.docs != null)
                {
                    HttpContext.Session.SetString("Docs", JsonSerializer.Serialize(FetchedData.docs));
                    FetchedData.docs = FetchedData.docs.Take(10).ToList();
                }

                foreach (Doc doc in FetchedData.docs)
                {
                    if (doc.isbn != null && doc.isbn.Any())
                    {
                        string imageUrl = $"http://covers.openlibrary.org/b/isbn/{doc.isbn[0]}-M.jpg?default=false";
                        doc.imageUrl = await UrlExists(imageUrl) ? imageUrl : "/images/book.jpg";
                    }
                    else
                    {
                        doc.imageUrl = "/images/book.jpg";
                    }

                    doc.subject = TakeFirstThree(doc.subject);
                    doc.publisher = TakeFirstThree(doc.publisher);
                }
            }
            else
            {
                // Handle the error
            }

            return Page();
        }

        public async Task<bool> UrlExists(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    // Using HEAD request to get only headers, not the full content
                    var request = new HttpRequestMessage(HttpMethod.Head, url);
                    var response = await httpClient.SendAsync(request);

                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                // Handle exceptions (optional)
                return false;
            }
        }

        public List<T> TakeFirstThree<T>(List<T> originalList)
        {
            return originalList?.Take(3).ToList() ?? new List<T>();
        }
    }
}