
using Newtonsoft.Json;
using Smarthub.API.Models;
using Smarthub.ViewModels;
using System.Text;

namespace Smarthub.Service
{
    public class OrderServiceRepository
    {
        private readonly HttpClient _httpClient;

        public IEnumerable<object> Orders { get; internal set; }

        public OrderServiceRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<OrderDTO>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync("api/Order");


            if (response.IsSuccessStatusCode)
            {
                var orders = await response.Content.ReadFromJsonAsync<List<OrderDTO>>();

                return orders;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return (List<OrderDTO>)JsonConvert.DeserializeObject<IEnumerable<OrderDTO>>(responseContent);

                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}. Response: {response}");
            }
        }

        public async Task<OrderDTO> GetOrderAsyncById(Guid OrderId)
        {
            var response = await _httpClient.GetAsync($"api/Order/{OrderId}");

            if (response.IsSuccessStatusCode)
            {
                var orders = await response.Content.ReadFromJsonAsync<OrderDTO>();

                return orders;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<OrderDTO>(responseContent);

                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}. Response: {response}");

            }
        }

        public async Task<Order> AddOrderAsync(OrderDTO order)
        {
            var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Order", content);

            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadAsStringAsync();

                var createdOrder = JsonConvert.DeserializeObject<Order>(responseContent);

                return createdOrder;
            }
            else
            {

                var errorContent = await response.Content.ReadAsStringAsync();

                Console.Error.WriteLine($"Request failed with status code: {response.StatusCode}. Response: {errorContent}");

                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}. Response: {errorContent}");
            }
        }


        public async Task<OrderDTO> UpdateOrderAsync(OrderDTO order)
        {

            var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");


            var response = await _httpClient.PutAsync($"api/Order/{order.OrderId}", content);


            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadAsStringAsync();

                var updatedOrder = JsonConvert.DeserializeObject<OrderDTO>(responseContent);

                return updatedOrder;
            }
            else
            {

                var errorContent = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}. Response: {errorContent}");
            }
        }


        public async Task DeleteOrderAsync(Guid OrderId)
        {
            var response = await _httpClient.DeleteAsync($"api/Order/{OrderId}");
            response.EnsureSuccessStatusCode();
        }


        public async Task<List<OrderLineDTO>> GetOrderLinesAsync()
        {
            var response = await _httpClient.GetAsync("api/OrderLine/GetOrderLines");

            if (response.IsSuccessStatusCode)
            {
                var orderLines = await response.Content.ReadFromJsonAsync<List<OrderLineDTO>>();

                return orderLines;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return (List<OrderLineDTO>)JsonConvert.DeserializeObject<IEnumerable<OrderLineDTO>>(responseContent);

                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}. Response: {response}");
            }
        }

        public async Task<OrderLineDTO> GetOrderLineAsyncById(Guid OrderLineId)
        {
            var response = await _httpClient.GetAsync($"api/OrderLine/{OrderLineId}");

            if (response.IsSuccessStatusCode)
            {
                var orderLine = await response.Content.ReadFromJsonAsync<OrderLineDTO>();

                return orderLine;
            }
            else
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<OrderLineDTO>(responseContent);

                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}. Response: {response}");

            }
        }

        public async Task<OrderLine> AddOrderLineAsync(OrderLineDTO orderLine)
        {
            var content = new StringContent(JsonConvert.SerializeObject(orderLine), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"api/OrderLine/{orderLine.OrderId}", content);

            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadAsStringAsync();

                var createdOrder = JsonConvert.DeserializeObject<OrderLine>(responseContent);

                return createdOrder;
            }
            else
            {

                var errorContent = await response.Content.ReadAsStringAsync();

                Console.Error.WriteLine($"Request failed with status code: {response.StatusCode}. Response: {errorContent}");

                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}. Response: {errorContent}");
            }
        }

        public async Task<OrderLineDTO> UpdateOrderLineAsync(OrderLineDTO orderLine)
        {

            var content = new StringContent(JsonConvert.SerializeObject(orderLine), Encoding.UTF8, "application/json");


            var response = await _httpClient.PutAsync($"api/OrderLine/{orderLine.OrderLineId}", content);


            if (response.IsSuccessStatusCode)
            {

                var responseContent = await response.Content.ReadAsStringAsync();

                var updatedOrder = JsonConvert.DeserializeObject<OrderLineDTO>(responseContent);

                return updatedOrder;
            }
            else
            {

                var errorContent = await response.Content.ReadAsStringAsync();

                throw new HttpRequestException($"Request failed with status code: {response.StatusCode}. Response: {errorContent}");
            }
        }

        public async Task DeleteOrderLineAsync(Guid OrderLineId)
        {
            var response = await _httpClient.DeleteAsync($"api/OrderLine/{OrderLineId}");

            response.EnsureSuccessStatusCode();
        }

    }
}
