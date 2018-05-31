﻿using System;
using System.Collections.Generic;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Common;
using Palmmedia.ReportGenerator.Core.Logging;
using Palmmedia.ReportGenerator.Core.Reporting;

namespace Palmmedia.ReportGenerator.Core
{
    /// <summary>
    /// Provides all parameters that are required for report generation.
    /// </summary>
    public class ReportConfiguration : IReportConfiguration
    {
        /// <summary>
        /// The report files.
        /// </summary>
        private List<string> reportFiles = new List<string>();

        /// <summary>
        /// The report file pattern that could not be parsed.
        /// </summary>
        private List<string> invalidReportFilePatterns = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportConfiguration" /> class.
        /// </summary>
        /// <param name="reportFilePatterns">The report file patterns.</param>
        /// <param name="targetDirectory">The target directory.</param>
        /// <param name="historyDirectory">The history directory.</param>
        /// <param name="reportTypes">The report types.</param>
        /// <param name="assemblyFilters">The assembly filters.</param>
        /// <param name="classFilters">The class filters.</param>
        /// <param name="fileFilters">The file filters.</param>
        /// <param name="verbosityLevel">The verbosity level.</param>
        /// <param name="tag">The custom tag (e.g. build number).</param>
        public ReportConfiguration(
            IEnumerable<string> reportFilePatterns,
            string targetDirectory,
            string historyDirectory,
            IEnumerable<string> reportTypes,
            IEnumerable<string> assemblyFilters,
            IEnumerable<string> classFilters,
            IEnumerable<string> fileFilters,
            string verbosityLevel,
            string tag)
        {
            if (reportFilePatterns == null)
            {
                throw new ArgumentNullException(nameof(reportFilePatterns));
            }

            if (reportTypes == null)
            {
                throw new ArgumentNullException(nameof(reportTypes));
            }

            if (assemblyFilters == null)
            {
                throw new ArgumentNullException(nameof(assemblyFilters));
            }

            if (classFilters == null)
            {
                throw new ArgumentNullException(nameof(classFilters));
            }

            if (fileFilters == null)
            {
                throw new ArgumentNullException(nameof(fileFilters));
            }

            foreach (var reportFilePattern in reportFilePatterns)
            {
                try
                {
                    this.reportFiles.AddRange(FileSearch.GetFiles(reportFilePattern));
                }
                catch (Exception)
                {
                    this.invalidReportFilePatterns.Add(reportFilePattern);
                }
            }

            this.TargetDirectory = targetDirectory ?? throw new ArgumentNullException(nameof(targetDirectory));
            this.HistoryDirectory = historyDirectory;

            if (reportTypes.Any())
            {
                this.ReportTypes = reportTypes.ToList();
            }
            else
            {
                this.ReportTypes = new[] { "Html" };
            }

            this.AssemblyFilters = assemblyFilters.ToList();
            this.ClassFilters = classFilters.ToList();
            this.FileFilters = fileFilters.ToList();

            if (verbosityLevel != null)
            {
                VerbosityLevel parsedVerbosityLevel = VerbosityLevel.Verbose;
                this.VerbosityLevelValid = Enum.TryParse<VerbosityLevel>(verbosityLevel, true, out parsedVerbosityLevel);
                this.VerbosityLevel = parsedVerbosityLevel;
            }
            else
            {
                this.VerbosityLevelValid = true;
            }

            this.Tag = tag;
        }

        /// <summary>
        /// Gets the report files.
        /// </summary>
        public IReadOnlyCollection<string> ReportFiles => this.reportFiles;

        /// <summary>
        /// Gets the target directory.
        /// </summary>
        public string TargetDirectory { get; }

        /// <summary>
        /// Gets the history directory.
        /// </summary>
        public string HistoryDirectory { get; }

        /// <summary>
        /// Gets the type of the report.
        /// </summary>
        public IReadOnlyCollection<string> ReportTypes { get; }

        /// <summary>
        /// Gets the assembly filters.
        /// </summary>
        public IReadOnlyCollection<string> AssemblyFilters { get; }

        /// <summary>
        /// Gets the class filters.
        /// </summary>
        public IReadOnlyCollection<string> ClassFilters { get; }

        /// <summary>
        /// Gets the file filters.
        /// </summary>
        public IReadOnlyCollection<string> FileFilters { get; }

        /// <summary>
        /// Gets the verbosity level.
        /// </summary>
        public VerbosityLevel VerbosityLevel { get; }

        /// <summary>
        /// Gets the custom tag (e.g. build number).
        /// </summary>
        public string Tag { get; }

        /// <summary>
        /// Gets the invalid file patters supplied by the user.
        /// </summary>
        public IReadOnlyCollection<string> InvalidReportFilePatterns
        {
            get
            {
                return this.invalidReportFilePatterns;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the verbosity level was successfully parsed during initialization.
        /// </summary>
        public bool VerbosityLevelValid { get; private set; }
    }
}
