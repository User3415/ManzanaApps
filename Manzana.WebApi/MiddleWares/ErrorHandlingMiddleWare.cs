using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Reflection;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace Manzana.WebApi.MiddleWares
{
    /// <summary>
    /// Middleware обработки исключений.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        /// <summary>
        /// Функция, которая может обрабатывать HTTP-запрос.
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Интерфейс сервиса логирования
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="next">Функция, которая может обрабатывать HTTP-запрос.</param>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Выполнить текущие запросы
        /// </summary>
        /// <param name="context"></param>
        public async Task Invoke(HttpContext context)
        {
            var bodyAsText = string.Empty;
            try
            {
                if (context.Request.Headers.TryGetValue("Content-Type", out var headerValues)
                    && headerValues.Count > 0
                    && headerValues[0].Contains("application/json"))
                {
                    var injectedRequestStream = new MemoryStream();
                    var bodyReader = new StreamReader(context.Request.Body);
                    bodyAsText = await bodyReader.ReadToEndAsync();
                    var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
                    await injectedRequestStream.WriteAsync(bytesToWrite, 0, bytesToWrite.Length);
                    injectedRequestStream.Seek(0, SeekOrigin.Begin);
                    context.Request.Body = injectedRequestStream;
                        
                    await _next.Invoke(context);
                    injectedRequestStream.Dispose();
                    bodyReader.Dispose();
                }
                else
                {
                    await _next.Invoke(context);
                }
                
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, bodyAsText);
            }
        }

        /// <summary>
        /// Обработать исключение
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <param name="bodyAsText"></param>
        /// <returns></returns>
        private Task HandleExceptionAsync(HttpContext context, Exception exception, string bodyAsText)
        {
            var (code, message) = GetHttpStatusCodeAndMessage(exception);
            if (code >= HttpStatusCode.InternalServerError)
            {
                var fullRequestPath =
                    $"{context.Request.Method} {context.Request.Scheme}://{context.Request.Host}/{context.Request.Path}{context.Request.QueryString}";
                _logger.Error($"{exception.GetType()} {fullRequestPath} {bodyAsText}");
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(GenerateResponseMessage(message));
        }

        /// <summary>
        /// Преобразование ошибок
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private (HttpStatusCode, string) GetHttpStatusCodeAndMessage(Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = exception.Message;
            switch (exception)
            {
                case ArgumentNullException _:
                case InvalidOperationException _:
                    {
                        code = HttpStatusCode.BadRequest;
                        break;
                    }
                case UnauthorizedAccessException _:
                case AuthenticationException _:
                    {
                        code = HttpStatusCode.Unauthorized;
                        break;
                    }
            }

            return (code, message);
        }

        /// <summary>
        /// Формирование сообщения об ошибке
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private string GenerateResponseMessage(string message)
        {
            return JsonConvert.SerializeObject(new ErrorResponse(message),
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
    }
}
