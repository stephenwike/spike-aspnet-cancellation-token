using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spike.CancellationTokenExample.ViewModels;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Spike.CancellationTokenExample.Controllers
{
    public class WorkController : Controller
    {
        private readonly ILogger<WorkController> _logger;

        public WorkController(ILogger<WorkController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Work2Secs()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri("http://localhost:15238/work/");
                var cancellationTokenSource = new CancellationTokenSource();
                cancellationTokenSource.CancelAfter(2000);

                using (var response = await httpClient.GetAsync("nonstop",
                    HttpCompletionOption.ResponseHeadersRead, cancellationTokenSource.Token))
                {
                    response.EnsureSuccessStatusCode();
                }

                var viewModel = new HomeViewModel();
                viewModel.Message = "Process completed.";
                return View("~/Views/Home/Index.cshtml", viewModel);
            }
            catch (TaskCanceledException)
            {
                var message = "Task was cancelled becuase it took too long to process";
                _logger.LogInformation(message);

                var viewModel = new HomeViewModel();
                viewModel.Message = message;
                return View("~/Views/Home/Index.cshtml", viewModel);
            }
        }

        public async Task<IActionResult> NonStop(CancellationToken cancellationToken)
        {
            try
            {
                ViewBag.message = "Workload finished processing.";
                await Task.Delay(10000, cancellationToken);

                var viewModel = new HomeViewModel();
                viewModel.Message = "Process completed.";
                return View("~/Views/Home/Index.cshtml", viewModel);
            }
            catch (TaskCanceledException)
            {
                ViewBag.message = "Workload cancelled - took too long.";
                _logger.LogInformation("Task cancelled.");
                return Ok("Request cancelled");
            }
        }
    }
}
