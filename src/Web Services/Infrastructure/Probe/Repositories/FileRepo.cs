﻿using Aiursoft.Probe.Data;
using Aiursoft.Probe.SDK.Models;
using Aiursoft.Probe.Services;
using Aiursoft.Scanner.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Aiursoft.Probe.Repositories
{
    public class FileRepo : IScopedDependency
    {
        private readonly ProbeDbContext _dbContext;
        private readonly IStorageProvider _storageProvider;

        public FileRepo(
            ProbeDbContext dbContext,
            IStorageProvider storageProvider)
        {
            _dbContext = dbContext;
            _storageProvider = storageProvider;
        }

        public async Task DeleteFileObject(File file)
        {
            _dbContext.Files.Remove(file);
            var haveDaemon = await _dbContext.Files.Where(f => f.Id != file.Id).AnyAsync(f => f.HardwareId == file.HardwareId);
            if (!haveDaemon)
            {
                _storageProvider.Delete(file.HardwareId);
            }
        }

        public async Task DeleteFile(int fileId)
        {
            var file = await _dbContext.Files.SingleOrDefaultAsync(t => t.Id == fileId);
            if (file != null)
            {
                await DeleteFileObject(file);
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> SaveFileToDb(string fileName, int folderId, long size)
        {
            var newFile = new File
            {
                FileName = fileName, //file.FileName,
                ContextId = folderId,
                FileSize = size
            };
            _dbContext.Files.Add(newFile);
            await _dbContext.SaveChangesAsync();
            return newFile.HardwareId;
        }

        public async Task CopyFile(string fileName, long fileSize, int contextId, string hardwareId)
        {
            var newFile = new File
            {
                FileName = fileName,
                FileSize = fileSize,
                ContextId = contextId,
                HardwareId = hardwareId
            };
            _dbContext.Files.Add(newFile);
            await _dbContext.SaveChangesAsync();
        }
    }
}
