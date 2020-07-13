using System.Net.Mime;
using System;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PortalRandkowy.API.Helpers
{
    public static class Extensions
    {
        public static int CalculateAge(this DateTime datetime)
        {
            var age = DateTime.Today.Year - datetime.Year;

            if (datetime.AddYears(age) > DateTime.Today)
                age--;
            return age;
        }

        public static void AddAplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination(this HttpResponse response , int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var PaginationHeader = new PaginationHeader(currentPage,itemsPerPage,totalItems,totalPages);        // tworzymy obiekt klasy paginationheader o odpowieedziach ... i przekazujemy ja do wartosci

            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();     // zmiana na male literki dla paginacji w naglowku

            response.Headers.Add("Pagination", JsonConvert.SerializeObject(PaginationHeader,camelCaseFormatter));          // twozymy obiekt dla headera o nazwie pagination o odpowiedzi paginationheader
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

    }


}