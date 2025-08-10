//using Microsoft.AspNetCore.Mvc;
//using System.Threading;

//namespace SmartSurveyAPI.Controllers
//{
//    [Route("api/document-migration")]
//    [ApiController]
//    public class DocumentMigrationController : ControllerBase
//    {
//        private readonly IServiceScopeFactory _serviceScopeFactory;
//        private readonly IDocumentService _migrationService;
//        private static int _totalFiles = 0;
//        private static bool _isMigrating = false;
//        private static CancellationTokenSource? _cancellationTokenSource = null;

//        public DocumentMigrationController(IServiceScopeFactory serviceScopeFactory, IDocumentService migrationService)
//        {
//            _serviceScopeFactory = serviceScopeFactory;
//            _migrationService = migrationService;
//        }

//        [HttpPost("start")]
//        public async Task<IActionResult> StartMigration([FromQuery] int batchSize = 10)
//        {
//            if (_isMigrating)
//            {
//                return BadRequest("Migration is already in progress.");
//            }

//            _isMigrating = true;

//            _cancellationTokenSource = new CancellationTokenSource();

//            _totalFiles = await _migrationService.GetTotalNonMigratedCountAsync(); // Count files before migration

//            _ = Task.Run(async () =>
//            {
//                using (var scope = _serviceScopeFactory.CreateScope())  // Create a new scope
//                {
//                    var migrationService = scope.ServiceProvider.GetRequiredService<IDocumentService>();

//                    try
//                    {
//                        await migrationService.MigrateDocumentsAsync(batchSize, _cancellationTokenSource.Token);
//                    }
//                    catch (OperationCanceledException)
//                    {
//                        // Log cancellation if needed
//                    }
//                    finally
//                    {
//                        _isMigrating = false;
//                    }

//                    _isMigrating = false;
//                }
//            });

//            return Ok(new { message = "Migration started.", totalFiles = _totalFiles });
//        }

//        [HttpGet("initial-count")]
//        public async Task<IActionResult> GetMigrationInitialCount()
//        {
//            if (!_isMigrating)
//            {
//                _totalFiles = await _migrationService.GetTotalNonMigratedCountAsync();
//                return Ok(new { status = "Completed", totalFiles = _totalFiles });
//            }

//            return Ok(newa
//            {
//                status = "In Progress",
//                totalFiles = _totalFiles
//            });
//        }

//        [HttpGet("progress")]
//        public async Task<IActionResult> GetMigrationProgress()
//        {
//            int remainingCount = await _migrationService.GetTotalNonMigratedCountAsync();

//            if (!_isMigrating)
//            {
//                return Ok(new { status = "Completed", totalFiles = _totalFiles, remainingFiles = remainingCount });
//            }

//            return Ok(new
//            {
//                status = "In Progress",
//                totalFiles = _totalFiles,
//                remainingFiles = remainingCount,
//                percentage = (_totalFiles > 0 && _totalFiles != remainingCount) ? Math.Round((double)(_totalFiles - remainingCount) / _totalFiles * 100, 2) : 0
//            });
//        }

//        [HttpPost("stop")]
//        public IActionResult StopMigration()
//        {
//            if (!_isMigrating)
//            {
//                return BadRequest("No migration is currently in progress.");
//            }

//            _cancellationTokenSource?.Cancel();
//            _isMigrating = false;

//            return Ok(new { message = "Migration has been stopped." });
//        }
//    }

//}
