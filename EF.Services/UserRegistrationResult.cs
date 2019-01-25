using System.Collections.Generic;
using System.Linq;

namespace EF.Services 
{
    public class UserRegistrationResult 
    {
        public UserRegistrationResult() 
        {
            this.Errors = new List<string>();
        }

        public bool Success 
        {
            get { return !this.Errors.Any(); }
        }

        public void AddError(string error) 
        {
            this.Errors.Add(error);
        }

        public IList<string> Errors { get; set; }
    }
}
