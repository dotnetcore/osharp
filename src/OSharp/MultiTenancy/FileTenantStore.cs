using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OSharp.MultiTenancy
{
    /// <summary>
    /// 基于文件的租户存储实现，将租户信息保存在JSON文件中
    /// </summary>
    public class FileTenantStore : ITenantStore
    {
        private readonly FileTenantStoreOptions _options;
        private readonly ILogger<FileTenantStore> _logger;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private Dictionary<string, TenantInfo> _tenants = new Dictionary<string, TenantInfo>();
        private bool _isInitialized = false;

        public FileTenantStore(
            IOptions<FileTenantStoreOptions> options,
            ILogger<FileTenantStore> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        /// <summary>
        /// 获取所有启用的租户
        /// </summary>
        public async Task<IEnumerable<TenantInfo>> GetAllTenantsAsync()
        {
            await EnsureInitializedAsync();
            return _tenants.Values.Where(t => t.IsEnabled).ToList();
        }

        /// <summary>
        /// 根据租户ID获取租户信息
        /// </summary>
        public async Task<TenantInfo> GetTenantAsync(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return null;
            }

            await EnsureInitializedAsync();

            if (_tenants.TryGetValue(tenantId, out var tenant) && tenant.IsEnabled)
            {
                return tenant;
            }

            return null;
        }

        /// <summary>
        /// 根据主机名获取租户信息
        /// </summary>
        public async Task<TenantInfo> GetTenantByHostAsync(string host)
        {
            if (string.IsNullOrEmpty(host))
            {
                return null;
            }

            await EnsureInitializedAsync();

            return _tenants.Values
                .FirstOrDefault(t => t.IsEnabled &&
                    (t.Host.Equals(host, StringComparison.OrdinalIgnoreCase) ||
                     host.EndsWith("." + t.Host, StringComparison.OrdinalIgnoreCase)));
        }

        /// <summary>
        /// 保存租户信息
        /// </summary>
        public async Task<bool> SaveTenantAsync(TenantInfo tenant)
        {
            if (tenant == null || string.IsNullOrEmpty(tenant.TenantId))
            {
                return false;
            }

            await EnsureInitializedAsync();

            await _semaphore.WaitAsync();
            try
            {
                // 更新内存中的租户信息
                _tenants[tenant.TenantId] = tenant;

                // 保存到文件
                await SaveToFileAsync();
                
                _logger.LogInformation("已保存租户信息: {TenantId}, {Name}, {Host}", 
                    tenant.TenantId, tenant.Name, tenant.Host);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存租户信息失败: {TenantId}", tenant.TenantId);
                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 删除租户信息
        /// </summary>
        public async Task<bool> DeleteTenantAsync(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return false;
            }

            await EnsureInitializedAsync();

            await _semaphore.WaitAsync();
            try
            {
                if (!_tenants.ContainsKey(tenantId))
                {
                    return false;
                }

                // 从内存中移除租户
                _tenants.Remove(tenantId);

                // 保存到文件
                await SaveToFileAsync();
                
                _logger.LogInformation("已删除租户: {TenantId}", tenantId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除租户失败: {TenantId}", tenantId);
                return false;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 确保已初始化
        /// </summary>
        private async Task EnsureInitializedAsync()
        {
            if (_isInitialized)
            {
                return;
            }

            await _semaphore.WaitAsync();
            try
            {
                if (_isInitialized)
                {
                    return;
                }

                await LoadFromFileAsync();
                _isInitialized = true;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 从文件加载租户信息
        /// </summary>
        private async Task LoadFromFileAsync()
        {
            string filePath = GetFilePath();
            
            if (!File.Exists(filePath))
            {
                _logger.LogInformation("租户配置文件不存在，将创建新文件: {FilePath}", filePath);
                _tenants = new Dictionary<string, TenantInfo>();
                await SaveToFileAsync(); // 创建空文件
                return;
            }

            try
            {
                string json = await File.ReadAllTextAsync(filePath);
                var tenants = JsonSerializer.Deserialize<List<TenantInfo>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                _tenants = tenants.ToDictionary(t => t.TenantId);
                _logger.LogInformation("已从文件加载 {Count} 个租户", _tenants.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "从文件加载租户信息失败: {FilePath}", filePath);
                _tenants = new Dictionary<string, TenantInfo>();
            }
        }

        /// <summary>
        /// 保存租户信息到文件
        /// </summary>
        private async Task SaveToFileAsync()
        {
            string filePath = GetFilePath();
            string directoryPath = Path.GetDirectoryName(filePath);
            
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            try
            {
                var tenants = _tenants.Values.ToList();
                string json = JsonSerializer.Serialize(tenants, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(filePath, json);
                _logger.LogDebug("已将 {Count} 个租户信息保存到文件: {FilePath}", tenants.Count, filePath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "保存租户信息到文件失败: {FilePath}", filePath);
                throw;
            }
        }

        /// <summary>
        /// 获取文件路径
        /// </summary>
        private string GetFilePath()
        {
            return Path.GetFullPath(_options.FilePath);
        }
    }

    /// <summary>
    /// 文件租户存储选项
    /// </summary>
    public class FileTenantStoreOptions
    {
        /// <summary>
        /// 租户配置文件路径，默认为 "App_Data/tenants.json"
        /// </summary>
        public string FilePath { get; set; } = "App_Data/tenants.json";
    }
} 