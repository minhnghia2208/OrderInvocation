using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderInvocation.Model;

namespace OrderInvocation
{
    internal class Utils
    {
        public static async Task InvokeEvent(HttpClient httpClient, Order order)
        {
            using StringContent serializedOrder = new(
                JsonSerializer.Serialize(order),
                Encoding.UTF8,
                "application/json");

            try { 
                using HttpResponseMessage response = await httpClient.PostAsync("https://localhost:4000/Order", serializedOrder);
            }
            catch (InvalidCastException e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
