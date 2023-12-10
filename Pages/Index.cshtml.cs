using BookReviews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace BookReviews.Pages
{

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        [BindProperty]
        public string UserInput { get; set; }
        //FetchedData is a collection of Docs that are found in the search result.
        public ApiResponse FetchedData { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        //When you click the search button in the index page, this method runs. It uses the OpenLibrary API. Results are not pretty sometimes.
        //Manually limit results to 10 but inefficient because it fetches all the results that are found, and then removes all but 10. 

        //Also fetches the cover image if available for the first ISBN, if not use standard image. Would be possible to loop through all ISBNS
        //to maybe find an ISBN with an image but that would be a lot of api calls. Also limit subjects and publishers to 3 to make the frontend
        //look more similar to eachother.
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
                HttpContext.Session.SetString("Docs", JsonSerializer.Serialize(FetchedData.docs));
            }
            else
            {
                // Handle the error
            }

            return Page();
        }

        //Check if the coverimage exists.
        public async Task<bool> UrlExists(string url)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
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

        //Limit the whole List to only contain three.
        public List<T> TakeFirstThree<T>(List<T> originalList)
        {
            return originalList?.Take(3).ToList() ?? new List<T>();
        }
    }
}