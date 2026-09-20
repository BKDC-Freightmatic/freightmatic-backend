using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Freightmatic.Functions.Shared;
using Freightmatic.Application.Shared;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Freightmatic.Application.Files;
using System.Collections.Generic;

namespace Freightmatic.Functions
{
    public class FileFunction
    {
        private readonly FileAppService _fileAppService;
        private readonly IConfiguration _configuration;
        private readonly JwtValidation _jwtValidation;
        private readonly HttpClient _httpClient;

        public FileFunction(IConfiguration configuration, JwtValidation jwtValidation, FileAppService fileAppService)
        {
            _configuration = configuration;
            _jwtValidation = jwtValidation;
            _fileAppService = fileAppService;
            _httpClient = new HttpClient();
        }

        [FunctionName("upload-files")]
        public async Task<IActionResult> UploadFiles(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "files")] HttpRequest req,
            ILogger log)
        {
            try
            {
                var isValid = _jwtValidation.ValidateJwtAccessToken(req);
                if (!isValid)
                    throw new CustomException("Unauthorized", "Unauthorized", HttpStatusCode.Unauthorized);

                IFormFileCollection files = req.Form.Files;
                if (files == null || files.Count == 0)
                    throw new CustomException("No files were uploaded", "Bad Request", HttpStatusCode.BadRequest);

                List<FileUploadDto> fileUploadDtos = [];
                foreach (var file in files)
                {
                    var fileContent = new StreamContent(file.OpenReadStream());
                    fileUploadDtos.Add(new FileUploadDto { Content = await fileContent.ReadAsByteArrayAsync(), Title = file.FileName });
                }

                List<Application.Files.FileDto> result = await _fileAppService.Upload(fileUploadDtos);
                return new OkObjectResult(result);
            }
            catch (CustomException ex)
            {
                var response = new ObjectResult(new ErrorResponse(ex.Message, ex.Error, ex.StatusCode))
                {
                    StatusCode = (int?)ex.StatusCode
                };
                return response;
            }
            catch (System.Exception ex)
            {
                log.LogError(ex.StackTrace);
                var response = new ObjectResult(new ErrorResponse())
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
                return response;
            }
        }
    }

    public class FileDto {
        [JsonPropertyName("title")]
        public string Title {get;set;}          
        [JsonPropertyName("url")]
        public string Url{get;set;}
    }
} 