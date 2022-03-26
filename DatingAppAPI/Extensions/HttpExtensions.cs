using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using DatingAppAPI.Helpers;
using System.Text.Json;

namespace DatingAppAPI.Extensions
{
    //extension method to add values to headers.
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems,int totalPages){
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            var options = new JsonSerializerOptions{
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader, options));
            //to expose our header we need to do CORS setting by below code
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");

        }
    }
}