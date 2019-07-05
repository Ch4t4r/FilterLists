﻿using System;
using System.Threading.Tasks;
using FilterLists.Agent.Core.Interfaces.Clients;
using Microsoft.Extensions.Localization;
using RestSharp;

namespace FilterLists.Agent.Infrastructure.Clients
{
    public class FilterListsApiClient : IFilterListsApiClient
    {
        private const string FilterListsApiBaseUrl = "https://filterlists.com/api/v1";
        private readonly IStringLocalizer<FilterListsApiClient> _localizer;
        private readonly IRestClient _restClient;

        public FilterListsApiClient(IStringLocalizer<FilterListsApiClient> stringLocalizer)
        {
            _localizer = stringLocalizer;
            _restClient = new RestClient(FilterListsApiBaseUrl) {UserAgent = "FilterLists.Agent"};
        }

        public async Task<TResponse> ExecuteAsync<TResponse>(IRestRequest request)
        {
            var response = await _restClient.ExecuteTaskAsync<TResponse>(request);
            if (response.ErrorException == null)
                return response.Data;
            throw new ApplicationException(_localizer["Error retrieving response from the FilterLists API."],
                response.ErrorException);
        }
    }
}