using System;
using System.Collections.Generic;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for global options settings.
    /// </summary>
    public class OptionsTests : TestBase
    {
        [Fact]
        public void CanSetConfigurationPaths()
        {
            var programData = GlobalOptions.GetSearchPath(ConfigurationLevel.ProgramData);
            var global = GlobalOptions.GetSearchPath(ConfigurationLevel.Global);
            var xdg = GlobalOptions.GetSearchPath(ConfigurationLevel.XDG);
            var system = GlobalOptions.GetSearchPath(ConfigurationLevel.System);

            try
            {
                GlobalOptions.SetSearchPath(ConfigurationLevel.ProgramData, "testProgramData");
                GlobalOptions.SetSearchPath(ConfigurationLevel.Global, "testGlobal");
                GlobalOptions.SetSearchPath(ConfigurationLevel.XDG, "testXDG");
                GlobalOptions.SetSearchPath(ConfigurationLevel.System, "testSystem");

                Assert.Equal("testProgramData", GlobalOptions.GetSearchPath(ConfigurationLevel.ProgramData));
                Assert.Equal("testGlobal", GlobalOptions.GetSearchPath(ConfigurationLevel.Global));
                Assert.Equal("testXDG", GlobalOptions.GetSearchPath(ConfigurationLevel.XDG));
                Assert.Equal("testSystem", GlobalOptions.GetSearchPath(ConfigurationLevel.System));
            }
            finally
            {
                GlobalOptions.SetSearchPath(ConfigurationLevel.ProgramData, programData);
                GlobalOptions.SetSearchPath(ConfigurationLevel.Global, global);
                GlobalOptions.SetSearchPath(ConfigurationLevel.XDG, xdg);
                GlobalOptions.SetSearchPath(ConfigurationLevel.System, system);
            }
        }
    }
}
