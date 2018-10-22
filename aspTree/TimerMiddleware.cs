using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
//using static webPages.Startup;

namespace aspTree
{
    public class TimerMiddleware
    {
        private readonly RequestDelegate _next;
        TimeService _timeService;
        DateService _dateService;
        Webreq _webreqService;

        public TimerMiddleware(RequestDelegate next, TimeService timeService, DateService dateService, Webreq webreqService)
        {
            _next = next;
            _timeService = timeService;
            _dateService = dateService;
            _webreqService =webreqService;
        }

        public async Task Invoke(HttpContext context)
        {
            string request = context.Request.Path.Value.ToLower();
            switch(request)
            {
                case "/time":
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync($"Текущее время сейчас: {_timeService?.GetTime()}");
                    StreamReader srn = File.OpenText("test.txt");

                    string input1 = null;
                    while ((input1 = srn.ReadLine()) != null)
                    {
                      await context.Response.WriteAsync(input1 + "<br/>");
                    }
                    break;
                case "/date":
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync($"Дата сегодня: {_dateService?.GetDate()}");
                    FileInfo f = new FileInfo("test.txt");

                    StreamWriter writer = f.CreateText();
                    writer.WriteLine("string  1");
                    writer.WriteLine("string  2");

                    for (int i = 0; i < 100; i++)
                    {
                        writer.WriteLine(i + " Новая строка");
                    }
                    writer.Write(writer.NewLine);
                    writer.Close();

                    //  Now  read  it  all  back  in.  
                    StreamReader sr = File.OpenText("test.txt");
                    //test
                    string input = null;
                    while ((input = sr.ReadLine()) != null)
                    {
                        await context.Response.WriteAsync(input +"<br/>");
                    }
           break;
                case "/webreq":
                    context.Response.ContentType = "text/html; charset=utf-8";
                    await context.Response.WriteAsync($" {_webreqService?.GetReq()}");
                    break;
                default:
                    await _next.Invoke(context);
                    break;
            }
         }
    }
    public static class TimerExtensions
    {
        public static IApplicationBuilder UseTimer(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TimerMiddleware>();
        }
    }
}
