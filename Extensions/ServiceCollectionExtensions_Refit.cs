//using Microsoft.Extensions.DependencyInjection;
//using Refit;
//using System;
//using ApiClientPrzelewy24.Clients;
//using Microsoft.Extensions.Http; // Add this if needed for IHttpClientBuilder

//namespace ApiClientPrzelewy24.Extensions
//{
//    public static class ServiceCollectionExtensionsRefit
//    {
//        public static IServiceCollection AddPrzelewy24Refit(this IServiceCollection services, string baseUrl)
//        {
//            if (string.IsNullOrWhiteSpace(baseUrl))
//                throw new ArgumentException("Base URL for Przelewy24 must be provided.", nameof(baseUrl));

//            services.AddRefitClient<IPrzelewy24Api>()
//                .ConfigureHttpClient(c => c.BaseAddress = new Uri(baseUrl));

//            return services;
//        }
//    }
//}