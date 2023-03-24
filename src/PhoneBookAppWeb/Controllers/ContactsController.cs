using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace PhoneBookAppWeb.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(ILogger<ContactsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Contact>> GetAsync()
        {
            IEnumerable<Contact>? contacts = null;
            HttpResponseMessage response = await HttpClientHelper.GetHttpClient().GetAsync("contacts");
            if (response.IsSuccessStatusCode)
            {
                contacts = await response.Content.ReadFromJsonAsync<IEnumerable<Contact>>();
            }
            return contacts;
        }
    }
}