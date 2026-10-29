using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderUtilityWebConsole.Infrastructure
{
    public interface IFileReaderService
    {
        List<Domain.Lead> ReadLeads(string filePath);
        string ReadFileContent(string filePath);
    }
}
