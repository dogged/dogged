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
            var global = GlobalOptions.GetSearchPath(ConfigurationLevel.Global);
            var xdg = GlobalOptions.GetSearchPath(ConfigurationLevel.XDG);
            var system = GlobalOptions.GetSearchPath(ConfigurationLevel.System);

            try
            {
                GlobalOptions.SetSearchPath(ConfigurationLevel.Global, "testGlobal");
                GlobalOptions.SetSearchPath(ConfigurationLevel.XDG, "testXDG");
                GlobalOptions.SetSearchPath(ConfigurationLevel.System, "testSystem");

                Assert.Equal("testGlobal", GlobalOptions.GetSearchPath(ConfigurationLevel.Global));
                Assert.Equal("testXDG", GlobalOptions.GetSearchPath(ConfigurationLevel.XDG));
                Assert.Equal("testSystem", GlobalOptions.GetSearchPath(ConfigurationLevel.System));
            }
            finally
            {
                GlobalOptions.SetSearchPath(ConfigurationLevel.Global, global);
                GlobalOptions.SetSearchPath(ConfigurationLevel.XDG, xdg);
                GlobalOptions.SetSearchPath(ConfigurationLevel.System, system);
            }
        }
    }
}
