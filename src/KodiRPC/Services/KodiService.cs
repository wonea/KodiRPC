﻿using System;
using System.Configuration;
using KodiRPC.Responses.VideoLibrary;
using KodiRPC.RPC.Connector;
using KodiRPC.RPC.Specifications;

namespace KodiRPC.Services
{
    public class KodiService
    {
        private readonly RpcConnector _rpcConnector;
        public readonly string ApiVersion = "v6";
        public readonly string Host = ConfigurationManager.AppSettings["KodiHost"];
        public readonly string Port = ConfigurationManager.AppSettings["KodiPort"];
        public readonly string Username = ConfigurationManager.AppSettings["KodiUsername"];
        public readonly string Password = ConfigurationManager.AppSettings["KodiPassword"];

        public KodiService()
        {
            _rpcConnector = new RpcConnector(this);
        }

        #region JSONRPC

        public string Ping()
        {
            return _rpcConnector.MakeRequest<string>(KodiMethods.Ping, new object());
        }

        #endregion

        #region VideoLibrary

        public GetTvShowDetailsResponse GetTvShowDetails(int tvShowId, string[] properties = null, string requestId="GetTvShowDetailsResponse")
        {
            properties = properties ?? new string[0];

            var parameters = new 
            {
                tvshowid = tvShowId,
                properties
            };

            return _rpcConnector.MakeRequest<GetTvShowDetailsResponse>(KodiMethods.GetTvShowDetails, parameters, requestId);
        }

        public GetMovieDetailsResponse GetMovieDetails(int movieId, string[] properties = null, string requestId="GetMovieDetailsResponse")
        {
            properties = properties ?? new string[0];

            var parameters = new 
            {
                movieid = movieId,
                properties
            };

            return _rpcConnector.MakeRequest<GetMovieDetailsResponse>(KodiMethods.GetMovieDetails, parameters, requestId);
        }
        #endregion
    }
}
