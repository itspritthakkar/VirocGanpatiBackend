//using Microsoft.AspNetCore.Mvc;
//using System.Threading;

//namespace VirocGanpati.Controllers
//{
//    [Route("api/document-compressor")]
//    [ApiController]
//    public class DocumentCompressorController : ControllerBase
//    {
//        private readonly IServiceScopeFactory _serviceScopeFactory;
//        private readonly IDocumentService _migrationService;
//        private static int _totalFiles = 0;
//        private static bool _isCompressing = false;
//        private static CancellationTokenSource? _cancellationTokenSource = null;

//        public DocumentCompressorController(IServiceScopeFactory serviceScopeFactory, IDocumentService migrationService)
//        {
//            _serviceScopeFactory = serviceScopeFactory;
//            _migrationService = migrationService;
//        }

//        [HttpPost("start")]
//        public async Task<IActionResult> StartCompression([FromQuery] int batchSize = 10)
//        {
//            if (_isCompressing)
//            {
//                return BadRequest("Compression is already in progress.");
//            }

//            _isCompressing = true;

//            _cancellationTokenSource = new CancellationTokenSource();

//            _totalFiles = await _migrationService.GetNonCompressedCountAsync(); // Count files before migration

//            _ = Task.Run(async () =>
//            {
//                using (var scope = _serviceScopeFactory.CreateScope())  // Create a new scope
//                {
//                    var migrationService = scope.ServiceProvider.GetRequiredService<IDocumentService>();

//                    try
//                    {
//                        await migrationService.CompressDocumentsAsync(batchSize, _cancellationTokenSource.Token);
//                    }
//                    catch (OperationCanceledException)
//                    {
//                        // Log cancellation if needed
//                    }
//                    finally
//                    {
//                        _isCompressing = false;
//                    }

//                    _isCompressing = false;
//                }
//            });

//            return Ok(new { message = "Compression started.", totalFiles = _totalFiles });
//        }

//        [HttpGet("initial-count")]
//        public async Task<IActionResult> GetMigrationInitialCount()
//        {
//            if (!_isCompressing)
//            {
//                _totalFiles = await _migrationService.GetNonCompressedCountAsync();
//                return Ok(new { status = "Completed", totalFiles = _totalFiles });
//            }

//            return Ok(new
//            {
//                status = "In Progress",
//                totalFiles = _totalFiles
//            });
//        }

//        [HttpGet("progress")]
//        public async Task<IActionResult> GetMigrationProgress()
//        {
//            int remainingCount = await _migrationService.GetNonCompressedCountAsync();

//            if (!_isCompressing)
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
//            if (!_isCompressing)
//            {
//                return BadRequest("No migration is currently in progress.");
//            }

//            _cancellationTokenSource?.Cancel();
//            _isCompressing = false;

//            return Ok(new { message = "Migration has been stopped." });
//        }
//    }

//}
