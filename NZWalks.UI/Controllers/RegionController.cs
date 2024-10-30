using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models.DTO;
using System;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegionController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> regionDtos = new List<RegionDto>();
            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7003/api/Region");

                httpResponseMessage.EnsureSuccessStatusCode();

                regionDtos.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());

            }
            catch (Exception)
            {

                throw;
            }

            return View(regionDtos);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionToViewModelDto model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var client = _httpClientFactory.CreateClient();


                var request = new HttpRequestMessage
                {
                    Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"),
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7003/api/Region")
                };                        

                var httpResponseMessage = await client.SendAsync(request);

                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Region");
                }
                else
                {
                    // Leer el contenido del error
                    var errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error al crear la región: {errorContent}");
                }

            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid Id) 
        {
            var client = _httpClientFactory.CreateClient();

            var request = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7003/api/Region/{Id.ToString()}");

            if (request is not null)
            {
                return View(request);
            }
            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto regionDto)
        {
            var client = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage
            {
                Content = new StringContent(JsonSerializer.Serialize(regionDto), Encoding.UTF8, "application/json"),
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7003/api/Region/{regionDto.Id}")
            };

            var httpRequest =await client.SendAsync(request);

            httpRequest.EnsureSuccessStatusCode();

            var resposnse = httpRequest.Content.ReadFromJsonAsync<RegionDto>();

            if (resposnse is not null)
            {
                return RedirectToAction("Index", "Region");
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid Id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var request = await client.DeleteAsync($"https://localhost:7003/api/Region/{Id}");

                request.EnsureSuccessStatusCode();

                return RedirectToAction("Index","Region");
            }
            catch (Exception)
            {

                
            }

            return View("Index");


        }
    }
}
