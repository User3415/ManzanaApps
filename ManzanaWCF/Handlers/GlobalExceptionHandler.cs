using System;
using System.Reflection;
using log4net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace ManzanaWCF.Handlers
{
    public class GlobalExceptionHandler : IErrorHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        ///     Enables the creation of a custom <see cref="T:System.ServiceModel.FaultException`1" /> that is returned from an
        ///     exception in the course of a service method.
        /// </summary>
        /// <remarks>
        ///     This method is optionally used to transform standard exceptions into custom FaultException(Of TDetail) that
        ///     can be passed back to the service client.
        /// </remarks>
        /// <param name="error">The <see cref="T:System.Exception" /> object thrown in the course of the service operation.</param>
        /// <param name="version">The SOAP version of the message.</param>
        /// <param name="fault">
        ///     The <see cref="T:System.ServiceModel.Channels.Message" /> object that is returned to the client, or
        ///     service, in the duplex case.
        /// </param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault) 
        {
            var newEx = new FaultException($"Exception caught at Service Application  GlobalExceptionHandler {Environment.NewLine} Method: {error.TargetSite.Name} {Environment.NewLine} Message: {error.Message}");

            MessageFault msgFault = newEx.CreateMessageFault();
            fault = Message.CreateMessage(version, msgFault, newEx.Action);
        }

        /// <summary>
        ///     This method will be called whenever an exception occurs. Therefore,
        ///     we log it and then return false so the error can continue to propagate up the chain.
        /// </summary>
        /// <param name="error">Exception being raised.</param>
        /// <returns>False to let the error propagate up the chain, or True to stop the error here.</returns>
        public bool HandleError(Exception ex)
        {
            Log.Error(ex);
            return false;
        }
    }
}