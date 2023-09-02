using Gym13.Application.Models;
using Gym13.Common.Resources;
using System.Net;

namespace Gym13.Application.Services
{
    public class BaseService
    {
        public static T Success<T>(T model = null, string message = null) where T : BaseResponseModel
        {
            if (model == null) model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.OK;
            model.Success = true;
            model.DeveloperMessage = message ?? Gym13Resources.OperationCompleted;
            model.UserMessage = message ?? Gym13Resources.OperationCompleted;

            return model;
        }

        public static T Success<T>() where T : BaseResponseModel
        {
            var model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.OK;
            model.Success = true;
            model.DeveloperMessage = Gym13Resources.OperationCompleted;
            model.UserMessage = Gym13Resources.OperationCompleted;

            return model;
        }

        public static T Fail<T>(T model = null, string message = null) where T : BaseResponseModel
        {
            if (model == null) model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.BadRequest;
            model.DeveloperMessage = message ?? Gym13Resources.OperationFailed;
            model.UserMessage = message ?? Gym13Resources.OperationFailed;

            return model;
        }

        public static T Fail<T>() where T : BaseResponseModel
        {
            var model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.BadRequest;
            model.DeveloperMessage = Gym13Resources.OperationFailed;
            model.UserMessage = Gym13Resources.OperationFailed;

            return model;
        }

        /// <summary>
        /// Packs the result with a Not found response parameters and specified message
        /// </summary>
        /// <typeparam name="T">Any class model that derives from ResponseBaseModel</typeparam>
        /// <param name="model"></param>
        /// <param name="message">custom repponse message</param>
        /// <returns></returns>
        public static T NotFound<T>(T model = null, string message = null) where T : BaseResponseModel
        {
            if (model == null) model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.NotFound;
            model.DeveloperMessage = message ?? $"{nameof(model)} not found.";
            model.UserMessage = message ?? Gym13Resources.RecordNotFound;

            return model;
        }
        public static T NotFound<T>() where T : BaseResponseModel
        {
            var model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.NotFound;
            model.DeveloperMessage = $"{nameof(model)} not found.";
            model.UserMessage = Gym13Resources.RecordNotFound;

            return model;
        }

        /// <summary>
        /// Packs the result with bad request response parameters
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static T BadRequest<T>(T model = null, string message = null) where T : BaseResponseModel
        {
            if (model == null) model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.BadRequest;
            model.DeveloperMessage = message ?? Gym13Resources.BadRequest;
            model.UserMessage = message ?? Gym13Resources.BadRequest;
            model.Success = false;

            return model;
        }

        public static T BadRequest<T>() where T : BaseResponseModel
        {
            var model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.BadRequest;
            model.DeveloperMessage = Gym13Resources.BadRequest;
            model.UserMessage = Gym13Resources.BadRequest;
            model.Success = false;

            return model;
        }

        public static T Unauthorized<T>(T model = null, string message = null) where T : BaseResponseModel
        {
            if (model == null) model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.Unauthorized;
            model.DeveloperMessage = message ?? Gym13Resources.Unauthorized;
            model.UserMessage = message ?? Gym13Resources.Unauthorized;
            model.Success = false;

            return model;
        }

        public static T Unauthorized<T>() where T : BaseResponseModel
        {
            var model = (T)Activator.CreateInstance(typeof(T), new object[] { });

            model.HttpStatusCode = (int)HttpStatusCode.Unauthorized;
            model.DeveloperMessage = Gym13Resources.Unauthorized;
            model.UserMessage = Gym13Resources.Unauthorized;
            model.Success = false;

            return model;
        }
    }
}
