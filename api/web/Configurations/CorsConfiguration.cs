namespace web.Configurations
{
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
        {
            var isProduction = Environment.GetEnvironmentVariable("PRODUCTION") == "true";

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .SetIsOriginAllowed(origin =>
                        {
                            if (string.IsNullOrEmpty(origin))
                                return false;

                            try
                            {
                                var uri = new Uri(origin);
                                var host = uri.Host;

                                if (!isProduction)
                                {
                                    if (host == "localhost")
                                        return true;

                                    if (System.Net.IPAddress.TryParse(host, out var ip))
                                    {
                                        if (ip.ToString().StartsWith("192.168."))
                                            return true;
                                    }
                                }

                                var clientUrl = Environment.GetEnvironmentVariable("CLIENT_URL");
                                if (!string.IsNullOrEmpty(clientUrl))
                                {
                                    var allowedHost = new Uri(clientUrl).Host;
                                    return host == allowedHost;
                                }

                                return false;
                            }
                            catch
                            {
                                return false;
                            }
                        })
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });

                options.AddPolicy("PublicPolicy", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }
    }
}
