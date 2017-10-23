﻿using ITOps.Json;
using ITOps.ViewModelComposition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Divergent.Sales.ViewModelComposition
{
    public class OrderDetailsViewModelAppender : IViewModelAppender
    {
        // Matching is a bit weak in this demo.
        // It's written this way to satisfy both the composite gateway and website samples.
        public bool Matches(RouteData routeData, string verb) =>
            HttpMethods.IsGet(verb)
                && string.Equals((string)routeData.Values["controller"], "orders", StringComparison.OrdinalIgnoreCase)
                && routeData.Values.ContainsKey("id");

        public async Task Append(dynamic vm, RouteData routeData, IQueryCollection query)
        {
            var id = (string)routeData.Values["id"];

            // Hardcoded to simplify the demo. In a production app, a config object could be injected.
            var url = $"http://localhost:20295/api/orders/{id}";
            var response = await new HttpClient().GetAsync(url);

            dynamic order = await response.Content.AsExpandoAsync();

            vm.OrderNumber = order.OrderNumber;
            vm.OrderItemsCount = order.ItemsCount;
        }
    }
}
