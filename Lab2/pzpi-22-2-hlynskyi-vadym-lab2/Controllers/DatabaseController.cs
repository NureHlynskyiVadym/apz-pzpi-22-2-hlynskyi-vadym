using LevelUp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LevelUp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly DatabaseBackupService _backupService;
        private readonly IWebHostEnvironment _env;

        public DatabaseController(DatabaseBackupService backupService, IWebHostEnvironment env)
        {
            _backupService = backupService;
            _env = env;
        }

        [HttpPost("backup")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BackupDatabase()
        {
            var backupDirectory = Path.Combine(_env.ContentRootPath, "Backups");
            if (!Directory.Exists(backupDirectory))
                Directory.CreateDirectory(backupDirectory);

            try
            {
                var backupFilePath = await _backupService.BackupDatabaseAsync(backupDirectory);
                return Ok(new { success = true, backupFile = Path.GetFileName(backupFilePath) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}
