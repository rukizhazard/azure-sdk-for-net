// Code generated by Microsoft (R) AutoRest Code Generator 0.17.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Microsoft.Azure.Management.RecoveryServices.Backup.Models
{
    using System.Linq;

    /// <summary>
    /// Filters to list backup items.
    /// </summary>
    public partial class ProtectedItemQueryObject
    {
        /// <summary>
        /// Initializes a new instance of the ProtectedItemQueryObject class.
        /// </summary>
        public ProtectedItemQueryObject() { }

        /// <summary>
        /// Initializes a new instance of the ProtectedItemQueryObject class.
        /// </summary>
        /// <param name="backupManagementType">Backup management type for the
        /// backed up item. Possible values include: 'Invalid',
        /// 'AzureIaasVM', 'MAB', 'DPM', 'AzureBackupServer',
        /// 'AzureSql'</param>
        /// <param name="itemType">Type of workload this item represents.
        /// Possible values include: 'Invalid', 'VM', 'FileFolder',
        /// 'AzureSqlDb', 'SQLDB', 'Exchange', 'Sharepoint',
        /// 'DPMUnknown'</param>
        /// <param name="policyName">Backup policy name associated with the
        /// backup item.</param>
        /// <param name="containerName">Name of the container.</param>
        public ProtectedItemQueryObject(BackupManagementType? backupManagementType = default(BackupManagementType?), DataSourceType? itemType = default(DataSourceType?), string policyName = default(string), string containerName = default(string))
        {
            BackupManagementType = backupManagementType;
            ItemType = itemType;
            PolicyName = policyName;
            ContainerName = containerName;
        }

        /// <summary>
        /// Gets or sets backup management type for the backed up item.
        /// Possible values include: 'Invalid', 'AzureIaasVM', 'MAB', 'DPM',
        /// 'AzureBackupServer', 'AzureSql'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "backupManagementType")]
        public BackupManagementType? BackupManagementType { get; set; }

        /// <summary>
        /// Gets or sets type of workload this item represents. Possible
        /// values include: 'Invalid', 'VM', 'FileFolder', 'AzureSqlDb',
        /// 'SQLDB', 'Exchange', 'Sharepoint', 'DPMUnknown'
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "itemType")]
        public DataSourceType? ItemType { get; set; }

        /// <summary>
        /// Gets or sets backup policy name associated with the backup item.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "policyName")]
        public string PolicyName { get; set; }

        /// <summary>
        /// Gets or sets name of the container.
        /// </summary>
        [Newtonsoft.Json.JsonProperty(PropertyName = "containerName")]
        public string ContainerName { get; set; }

    }
}