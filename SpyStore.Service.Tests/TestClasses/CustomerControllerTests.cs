using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Newtonsoft.Json;
using SpyStore.Models.Entities;
using SpyStore.Service.Tests.TestClasses.Base;
using Xunit;
using System.Net;

namespace SpyStore.Service.Tests.TestClasses
{
    public class CustomerControllerTests: BaseTestClass
    {
        public CustomerControllerTests()
        {
            RootAdress = "api/customer";
        }
        [Fact]
        public async void ShouldGetAllCustomers()
        {
            ////Get All Customers: http://localhost:55882/api/customer
           using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAdress}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var customers = JsonConvert.DeserializeObject<List<Customer>>(jsonResponse);
                Assert.Single(customers);
                

            }
           
        }
        [Fact]
        public async void ShouldGetOneCustomer()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAdress}");
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var customer = JsonConvert.DeserializeObject<Customer>(jsonResponse);
                Assert.Equal("Super Spy", customer.FullName);
            }
        }
        [Fact]
        public async void ShouldFailIfBadCustomerId()
        {
            //Get One Category: http://localhost:55882/api/customer/1
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAdress}/2");
                Assert.False(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);



            }

        }
    }
}
