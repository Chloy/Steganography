using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Stenography {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e) {
            var langCode = Stenography.Properties.Settings.Default.language;
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(langCode);

            base.OnStartup(e);
        }
    }
}
