using CAPNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CAPNet
{
    /// <summary>
    /// 
    /// </summary>
    public class AlertValidator
    {

        private readonly Alert alert;

        /// <summary>
        /// 
        /// </summary>
        public Alert Alert
        {
            get { return alert; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alert"></param>
        public AlertValidator(Alert alert)
        {
            this.alert = alert;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsValid
        {
            get
            {
                return !Errors.Any();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<Error> Errors => GetErrors(alert);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alert"></param>
        /// <returns></returns>
        private static IEnumerable<Error> GetErrors(Alert alert)
        {
            return alert.GetErrorsFromAllEntityValidators();
        }
    }
}