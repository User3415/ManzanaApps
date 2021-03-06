using System;
using System.ServiceModel.Configuration;

namespace ManzanaWCF.Logging
{
    public class ErrorHandlerBehavior : BehaviorExtensionElement
    {
        /// <summary>
        ///     Gets the type of behavior.
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(ErrorServiceBehavior); }
        }

        /// <summary>
        ///     Creates a behavior extension based on the current configuration settings.
        /// </summary>
        /// <returns>
        ///     The behavior extension.
        /// </returns>
        protected override object CreateBehavior()
        {
            return new ErrorServiceBehavior();
        }
    }
}