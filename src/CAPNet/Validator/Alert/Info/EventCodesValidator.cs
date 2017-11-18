using CAPNet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CAPNet
{
    /// <summary>
    /// Multiple instances MAY occur within an info block.
    /// </summary>
    public class EventCodesValidator : Validator<Info>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        public EventCodesValidator(Info info) : base(info) { }

        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable<Error> Errors
        {
            get
            {
                return from eventCode in Entity.EventCodes
                       from error in GetErrors(eventCode)
                       select error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsValid
        {
            get
            {
                return !Errors.Any();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventCode"></param>
        /// <returns></returns>
        private static IEnumerable<Error> GetErrors(EventCode eventCode)
        {
            return eventCode.GetErrorsFromAllEntityValidators();
        }
    }
}
