using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Dna;

namespace MyRepoWebApp.Services.Logger
{
    public class WriteToLog
    {
        public static void writeToLogInformation(string message)
        {
            Program.Logger.LogInformation(message);
            FrameworkDI.Logger.LogInformation(message);
        }
        public static void writeToLogError(string message)
        {
            Program.Logger.LogError(message);
            FrameworkDI.Logger.LogError(message);
        }

    }
}
