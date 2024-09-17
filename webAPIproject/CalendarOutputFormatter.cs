using A2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

public class CalendarOutputFormatter : TextOutputFormatter
    {
    public CalendarOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/calendar"));
            SupportedEncodings.Add(Encoding.UTF8);
        }
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
        Events cEvent = (Events)context.Object;
        StringBuilder c = new StringBuilder();
        c.AppendLine("BEGIN:VCALENDAR");
        c.AppendLine("VERSION:4.0");
        c.AppendLine("PRODID:lpel645");
        c.AppendLine("BEGIN:VEVENT");
        c.AppendLine("UID:" + cEvent.Id);
        c.AppendLine("DTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ"));
        c.AppendLine("DTSTART:" + cEvent.Start);
        c.AppendLine("DTEND:" + cEvent.End);
        c.AppendLine("SUMMARY:" + cEvent.Summary);
        c.AppendLine("DESCRIPTION:" + cEvent.Description);
        c.AppendLine("LOCATION:" + cEvent.Location);
        c.AppendLine("END:VEVENT");
        c.AppendLine("END:VCALENDAR");

        string output = c.ToString();
        byte[] outBytes = selectedEncoding.GetBytes(output);
        var response = context.HttpContext.Response.Body;
        return response.WriteAsync(outBytes, 0, outBytes.Length);
    }
}