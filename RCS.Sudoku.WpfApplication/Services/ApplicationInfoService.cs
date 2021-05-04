using RCS.Sudoku.WpfApplication.Contracts.Services;
using System;
using System.Diagnostics;
using System.Reflection;

namespace RCS.Sudoku.WpfApplication.Services
{
    public class ApplicationInfoService : IApplicationInfoService
    {
        public ApplicationInfoService()
        {
        }

        public Version GetVersion()
        {
            // Set the app version in App2 > Properties > Package > PackageVersion
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            var version = FileVersionInfo.GetVersionInfo(assemblyLocation).FileVersion;
            return new Version(version);
        }
    }
}
