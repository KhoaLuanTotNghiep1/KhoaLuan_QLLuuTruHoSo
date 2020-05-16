using Newtonsoft.Json;
using System.Web.Http;

namespace S3Train.WebHeThong.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettingsCustom();
        }

        private class JsonSerializerSettingsCustom : JsonSerializerSettings
        {
            public JsonSerializerSettingsCustom()
            {
                NullValueHandling = NullValueHandling.Ignore;
                DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                Formatting = Formatting.None;

            }
        }
    }
}