using CustomerWeb.Models;
using CustomerWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;

namespace CustomerWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _baseUrl;

     
        public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IOptions<ApiSettings> apiSettings)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _baseUrl = apiSettings.Value.BaseUrl;
        }

        public async Task<IActionResult> Index()
        {
            string apiUrl = _baseUrl;
            List<Customer> customers = new List<Customer>();

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    customers = JsonConvert.DeserializeObject<List<Customer>>(jsonResponse);
                }
                else
                {
                    _logger.LogError($"Failed to retrieve customers: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customers from API");
            }

            return View(customers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                string apiUrl = _baseUrl;

                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        _logger.LogError($"Failed to create customer: {response.StatusCode}");
                        ModelState.AddModelError("", "Failed to create customer. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while creating a customer");
                    ModelState.AddModelError("", "An error occurred while creating a customer. Please try again.");
                }
            }

            return View(customer);
        }

        public async Task<IActionResult> View(int id)
        {
            string apiUrl = $"{_baseUrl}/{id}";
            Customer customer = null;

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    customer = JsonConvert.DeserializeObject<Customer>(jsonResponse);
                }
                else
                {
                    _logger.LogError($"Failed to retrieve customer for viewing: {response.StatusCode}");
                    // Handle error (e.g., redirect to an error page or show a message)
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customer data for viewing");
               
            }

            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            string apiUrl = $"{_baseUrl}/{id}";

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Failed to delete customer: {response.StatusCode}");
                    TempData["ErrorMessage"] = "Failed to delete customer. Please try again.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the customer.");
                TempData["ErrorMessage"] = "An error occurred while deleting the customer. Please try again.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
            string apiUrl = $"{_baseUrl}/{id}";

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var customer = JsonConvert.DeserializeObject<Customer>(jsonResponse);
                    return View(customer);
                }
                else
                {
                    _logger.LogError($"Failed to retrieve customer details: {response.StatusCode}");
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching customer details.");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return View(customer);
            }

            string apiUrl = $"{_baseUrl}/{customer.Id}";

            try
            {
                var client = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
                var response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError($"Failed to update customer: {response.StatusCode}");
                    TempData["ErrorMessage"] = "Failed to update customer. Please try again.";
                    return View(customer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the customer.");
                TempData["ErrorMessage"] = "An error occurred while updating the customer. Please try again.";
                return View(customer);
            }
        }
    }
}
