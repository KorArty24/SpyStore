using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using SpyStore.Models.Entities;
using SpyStore.Service.Tests.TestClasses.Base;
using Xunit;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace SpyStore.Service.Tests.TestClasses
{
    [Collection("Service Testing")]
    public class CustomerControllerTests: BaseTestClass
    {
        public CustomerControllerTests()
        {
            RootAddress = "api/customer";
        }
        [Fact]
        public async void ShouldGetAllCustomers()
        {
            ////Get All Customers: http://localhost:55882/api/customer
           using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var customers = JsonSerializer.Deserialize<List<Customer>>(jsonResponse);
                Assert.Single(customers);
            }
        }
        [Fact]
        public async void ShouldGetOneCustomer()
        {
            //Get One customer: http://localhost:55882/api/customer/0
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/2");
                Assert.True(response.IsSuccessStatusCode);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var customer = JsonSerializer.Deserialize<Customer>(jsonResponse);
                Assert.Equal("Bob", customer.FullName);
            }
        }
        [Fact]
        public async void ShouldFailIfBadCustomerId()
        {
            //Get One Category: http://localhost:55882/api/customer/1
            using (var client = new HttpClient()) 
            {
                var response = await client.GetAsync($"{ServiceAddress}{RootAddress}/2");
                Assert.False(response.IsSuccessStatusCode);
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
