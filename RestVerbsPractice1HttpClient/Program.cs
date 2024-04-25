using System.Net.Http;

namespace RestVerbsPractice1HttpClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            IHostBuilder builder = Host.CreateDefaultBuilder(args);
            builder.ConfigureServices((context, services) =>
            {
                // approach 1, IHttpClientFactory
//                services.AddHttpClient();
//                services.AddSingleton<HttpFactoryClient>();

                // approach 2, Named client
                services.AddHttpClient("TimeEnriesClient", httpClient =>
                {
                    httpClient.BaseAddress = new Uri("https://localhost:7195");
                });
                // registering a concretete class, but ideally should be 
                // interfaced based to stick to RCM rules,  just trying to keep the code short
                services.AddSingleton<NamedHttpClient>();

                // approach 3, Typed client
                // registering a concretete class, but ideally should be 
                // interfaced based to stick to RCM rules,  just trying to keep the code short
                services.AddHttpClient<TypedHttpClient>();

            });

            IHost host = builder.Build();

            // approach 1
//            HttpFactoryClient httpFactoryClient = host.Services.GetRequiredService<HttpFactoryClient>();

            // hard-code, but should be in configuration
//            await httpFactoryClient.GetAsync("api/v1/timeentries");

            // approach 2
//            NamedHttpClient namedHttpClient = host.Services.GetRequiredService<NamedHttpClient>();
//            // hard-code, but should be in configuration
//            await namedHttpClient.GetAsync("api/v1/timeentries");

            // approach 3 - Typed client
            TypedHttpClient typedHttpClient = host.Services.GetRequiredService<TypedHttpClient>();
            await typedHttpClient.GetAsync("api/v1/timeentries");
        }
    }

    public class HttpFactoryClient
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HttpFactoryClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task GetAsync(string url)
        {
            HttpClient httpClient = httpClientFactory.CreateClient();
            // base address must be set on each client instance 
            httpClient.BaseAddress = new Uri("https://localhost:7195");
            
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.ReadLine();
        }
    }

    public class NamedHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NamedHttpClient(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public async Task GetAsync(string url)
        {
            // get client by name, base address already configured during ioc injection
            HttpClient httpClient = _httpClientFactory.CreateClient("TimeEnriesClient");
            HttpResponseMessage response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.ReadLine();
        }
    }

    public class TypedHttpClient
    {
        private readonly HttpClient _httpClient;

        public TypedHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // shared config done here in ctor or in callback in ioc registration
            _httpClient.BaseAddress = new Uri("https://localhost:7195");
        }

        public async Task GetAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            Console.ReadLine();
        }
    }   

}