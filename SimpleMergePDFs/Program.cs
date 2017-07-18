using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using iText.IO;
using iText.IO.Log;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Navigation;
using iText.Kernel.Utils;
using NLog;
using NLog.Config;
using NLog.Targets;
using SimpleMergePDFs.iTextLoggers;

namespace SimpleMergePDFs
{
    class Program
    {
        private static Dictionary<string, Stream> _files;
        private static Options _options;
        private static Logger _logger;
        static void Main(string[] args)
        {
            Setup(args);
            if (!String.IsNullOrEmpty(_options.InputFolder))
            {
                if (!_options.InputFolder.EndsWith("\\"))
                {
                    _options.InputFolder += "\\";
                }
                Verbose($"Looping Through Directory : {_options.InputFolder}" );
                string[] fileEntries = Directory.GetFiles(_options.InputFolder, "*.pdf", _options.Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                Verbose($"Found {fileEntries.Length} Files");
                _files = new Dictionary<string, Stream>();
                foreach (string fileName in fileEntries.ToList().OrderBy(f => f))
                {
                    var file = File.Open(fileName, FileMode.Open);
                    _files.Add(fileName, file);
                }
                var outputName = Output();
                File.WriteAllBytes(outputName,Process());
                Standard($"Writing to file {outputName}");

            }
            else
            {
                Standard("No Input Folder");
            }
            Standard("Done");
        }

        private static string Output()
        {
            if (!Directory.Exists(_options.InputFolder + "_Output\\"))
            {
                Directory.CreateDirectory(_options.InputFolder + "_Output\\");
            }
            var outputName = _options.InputFolder + "_Output\\" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".pdf";
            return outputName;
        }

        private static void Setup(string[] args)
        {
            SetupLogging();
            
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            _options = new Options(assemblyVersion);
            CommandLine.Parser.Default.ParseArguments(args, _options);
            if (_options.Verbose)
            {
                LoggerFactory.BindFactory(new VerboseLoggerFactory());
            }
            else
            {
                LoggerFactory.BindFactory(new DefaultLoggerFactory());
            }
          
        }

        private static void SetupLogging()
        {
            var config = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget();
            consoleTarget.UseDefaultRowHighlightingRules = true;
            config.AddTarget("console", consoleTarget);
            consoleTarget.Layout = @"${level:uppercase=true}|${message}";
            var rule1 = new LoggingRule("*", LogLevel.Trace, consoleTarget);
            config.LoggingRules.Add(rule1);
            LogManager.Configuration = config;
            _logger = LogManager.GetCurrentClassLogger();
        }


        private static byte[] Process()
        {
            var retVal = MergePdfForms();
            return retVal.ToArray();
        }

        private static byte[] MergePdfForms()
        {
            var dest = new MemoryStream();
            PdfDocument pdf = new PdfDocument(new PdfWriter(dest));
            PdfMerger merger = new PdfMerger(pdf);
            PdfOutline rootOutline = pdf.GetOutlines(false);
            // PdfOutline helloWorld = rootOutline.AddOutline("Root");

            int pages = 1;
            foreach (var keyValuePair in _files)
            {
                var myTi = new CultureInfo("en-US", false).TextInfo;
                var d = Path.GetFileNameWithoutExtension(keyValuePair.Key);
                d = myTi.ToTitleCase(d);
                Verbose($"Adding Document : {d}");


                var firstSourcePdf = new PdfDocument(new PdfReader(keyValuePair.Value));
                var subPages = firstSourcePdf.GetNumberOfPages();
                merger.Merge(firstSourcePdf, 1, subPages);
                firstSourcePdf.Close();
              
                var link1 = rootOutline.AddOutline(d);
                link1.AddDestination(PdfExplicitDestination.CreateFit(pdf.GetPage(pages)));
                pages += subPages;


            }





            pdf.Close();
            return dest.ToArray();
        }

        private static void Standard(string s)
        {
            _logger.Info(s);
        }

        private static void Verbose(string s)
        {
            if (!_options.Verbose)
            {
                return;
            }
            _logger.Trace(s);
        }
    }
}
