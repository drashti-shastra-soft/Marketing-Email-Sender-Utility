using EmailSenderUtilityWebConsole.Domain;
using EmailSenderUtilityWebConsole.Infrastructure;
using EmailSenderUtilityWebConsole.Infrastructure.IService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace EmailSenderUtilityWebConsole.Service
{
    public class FileReaderService : IFileReaderService
    {
        private readonly ICSVReaderService _csvReaderService;

        public FileReaderService(ICSVReaderService csvReaderService)
        {
            _csvReaderService = csvReaderService;
        }

        public string ReadFileContent(string filePath)
        {
            if (filePath.IsNullOrEmpty())
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            string physicalPath = HostingEnvironment.MapPath(filePath);

            if (physicalPath == null || !File.Exists(physicalPath))
                throw new FileNotFoundException($"File not found at path: {filePath}");

            return File.ReadAllText(physicalPath);
        }

        public List<Domain.Lead> ReadLeads(string filePath)
        {
            var mInvoices = new List<Domain.Lead>();

            var dataTable = _csvReaderService.ReadFile(filePath);
            if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
            {
                dataTable = Infrastructure.StaticMethod.RemoveEscapeSequences(dataTable);
                ValidateColumnName(dataTable);
                mInvoices = ConvertFromDataTableToModel(dataTable);
            }

            return mInvoices;
        }

        private void ValidateColumnName(DataTable dataTable)
        {
            if (dataTable != null && dataTable.Columns != null && dataTable.Columns.Count > 0)
            {
                var mColumns = EnumsHelper.GetEnumDisplayNames(new Enums.FileType());
                foreach (DataColumn column in dataTable.Columns)
                {
                    if (mColumns.Where(x => x.Equals(column.ColumnName)).Count() == 0)
                        throw new InvalidOperationException($"Invalid Column Name '{column.ColumnName}'. Please correct them and upload file again.");
                }

                if (mColumns != null && mColumns.Count != dataTable.Columns.Count)
                    throw new InvalidOperationException($"Invalid import file format. Please correct them and upload file again");
            }
        }

        private List<Domain.Lead> ConvertFromDataTableToModel(DataTable dataTable)
        {
            string json = JsonConvert.SerializeObject(dataTable);
            var mLeads = JsonConvert.DeserializeObject<List<Lead>>(json);
            return mLeads;
        }
    }
}
