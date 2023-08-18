using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using WebPage.Shared.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace WebPage.Shared.Services
{
    public class CRUDService
    {
        private readonly HttpClient _httpClient;

        public CRUDService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public string getservidor = "https://localhost:7153";

        public async Task<bool> AgregarPersonaAsync(Persona persona)
        {
            var response = await _httpClient.PostAsJsonAsync(getservidor + "/api/Persona/AgregarPersona", persona);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarPersonaAsync(Persona persona)
        {
            var response = await _httpClient.PutAsJsonAsync(getservidor + "/api/Persona/ActualizarPersona", persona);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarPersonaAsync(int id, string usuario)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7153/api/Persona/EliminarPersona?id={id}&usuario={usuario}");
            return response.IsSuccessStatusCode;
        }
        public async Task<Persona> GetPersonaAsync(int id)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7153/api/Persona/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Persona>();
            }
            else
            {
                return null;
            }
        }
    }
    
  }
