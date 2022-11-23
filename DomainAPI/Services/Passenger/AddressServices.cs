using DomainAPI.Models.Passenger;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DomainAPI.Services.Passenger
{
    public class AddressServices
    {

        private PassengerAddress _address;

        public AddressServices()
        {

        }

        public async Task<PassengerAddress> MainAsync(string zipCode)
        {

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync("https://viacep.com.br/ws/" + zipCode + "/json/");

                var adressJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                    return _address = JsonConvert.DeserializeObject<PassengerAddress>(adressJson);

                else
                    return null;
            }
        }
    }
}
