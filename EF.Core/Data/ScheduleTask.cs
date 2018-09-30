using System;

namespace EF.Core.Data
{
    public partial class ScheduleTask : BaseEntity
    {

        public string Name { get; set; }

        public int Seconds { get; set; }

        public string Type { get; set; }

        public bool Enabled { get; set; }

        public bool StopOnError { get; set; }


        /// <summary>
        /// Gets or sets the machine name (instance) that leased this task. It's used when running in web farm (ensure that a task in run only on one machine). It could be null when not running in web farm.
        /// </summary>
        public string LeasedByMachineName { get; set; }
        /// <summary>
        /// Gets or sets the datetime until the task is leased by some machine (instance). It's used when running in web farm (ensure that a task in run only on one machine).
        /// </summary>
        public DateTime? LeasedUntilUtc { get; set; }

        /// <summary>
        /// Gets or sets the datetime when it was started last time
        /// </summary>
        public DateTime? LastStartUtc { get; set; }
        /// <summary>
        /// Gets or sets the datetime when it was finished last time (no matter failed ir success)
        /// </summary>
        public DateTime? LastEndUtc { get; set; }
        /// <summary>
        /// Gets or sets the datetime when it was sucessfully finished last time
        /// </summary>
        public DateTime? LastSuccessUtc { get; set; }

    }
}
