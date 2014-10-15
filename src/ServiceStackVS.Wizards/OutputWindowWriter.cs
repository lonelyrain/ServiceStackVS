﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace ServiceStackVS.Wizards
{
    public class OutputWindowWriter
    {
        private IVsOutputWindowPane _outputWindowPane;


        public OutputWindowWriter(System.IServiceProvider serviceProvider, string outputWindowPaneGuid, string outputWindowPaneName)
        {
            var outputWindow = serviceProvider.GetService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            if (outputWindow == null) throw new Exception("Unable to create an output pane.");

            var paneGuid = new Guid(outputWindowPaneGuid);
            outputWindow.GetPane(ref paneGuid, out _outputWindowPane);
            if (_outputWindowPane == null)
            {
                outputWindow.CreatePane(ref paneGuid, outputWindowPaneName, 1, 0);
                outputWindow.GetPane(ref paneGuid, out _outputWindowPane);
            }
        }

        public void Show()
        {
            _outputWindowPane.Activate();
        }

        public OutputWindowWriter(string outputWindowPaneGuid, string outputWindowPaneName)
        {
            var outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            if (outputWindow == null) throw new Exception("Unable to create an output pane.");

            var paneGuid = new Guid(outputWindowPaneGuid);
            outputWindow.GetPane(ref paneGuid, out _outputWindowPane);
            if (_outputWindowPane == null)
            {
                outputWindow.CreatePane(ref paneGuid, outputWindowPaneName, 1, 0);
                outputWindow.GetPane(ref paneGuid, out _outputWindowPane);
            }
        }

        public void ShowOutputPane(DTE dte)
        {
            var outputWindow = dte.Windows.Item("{34E76E81-EE4A-11D0-AE2E-00A0C90FFFC3}");
            if (outputWindow != null)
            {
                outputWindow.Visible = true;
            }
        }

        public void Write(string format, params object[] parameters)
        {
            if (_outputWindowPane == null || format == null) return;

            _outputWindowPane.OutputString(String.Format(format, parameters));
        }

        public void WriteLine(string format, params object[] parameters)
        {
            Write(format + Environment.NewLine, parameters);
        }

        public void Clear()
        {
            _outputWindowPane.Clear();
        }
    }
}
