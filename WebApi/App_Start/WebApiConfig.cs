using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API 設定和服務
            config.EnableCors();

            // Web API 路由(屬性路由)
            config.MapHttpAttributeRoutes();
            //傳統路由
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                //id是optional的可以不要有
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
