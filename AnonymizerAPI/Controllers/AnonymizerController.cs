using AnonymizerAPI.Model;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace AnonymizerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnonymizerController : ControllerBase
    {
        private readonly ILogger<AnonymizerController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public AnonymizerController(ILogger<AnonymizerController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Anonymize([FromBody]string input)
        {
            var analyzerInput = new AnalyzerInput { Text = input };
            var analyzerClient = _httpClientFactory.CreateClient("Analyzer");
            var analyzerResponse = await analyzerClient.PostAsJsonAsync("analyze", analyzerInput);
            analyzerResponse.EnsureSuccessStatusCode();

            var analyzerResult = await analyzerResponse.Content.ReadFromJsonAsync<List<AnalyzerResult>>();

            var anonymizerInput = new AnonymizerInput
            {
                Text = input,
                AnalyzerResults = analyzerResult
            };

            var anonymizerClient = _httpClientFactory.CreateClient("Anonymizer");
            var anonymizerResponse = await anonymizerClient.PostAsJsonAsync("anonymize", anonymizerInput);
            anonymizerResponse.EnsureSuccessStatusCode();

            var anonymizerResult = await anonymizerResponse.Content.ReadFromJsonAsync<AnonymizerResult>();

            return Ok(anonymizerResult.Text);
        }
    }
}