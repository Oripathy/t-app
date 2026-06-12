using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Dogs.Model;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Dogs.Infrastructure
{
    public class BreedsRequester
    {
        private readonly RequestSender _requestSender;
        private readonly DogsConfiguration _dogsConfiguration;
        private readonly DogsModel _dogsModel;

        public BreedsRequester(RequestSender requestSender, DogsConfiguration dogsConfiguration, DogsModel dogsModel)
        {
            _requestSender = requestSender;
            _dogsConfiguration = dogsConfiguration;
            _dogsModel = dogsModel;
        }

        public async UniTask RequestBreeds(int page, CancellationToken token)
        {
            var uri = $"{_dogsConfiguration.Uri}/breeds?page[number]={page}&page[size]={_dogsConfiguration.PageSize}";
            var result = await _requestSender.SendRequest(() => GetRequest(uri), token);
            HandleBreedsCollection(result);
        }

        public async UniTask<string> RequestBreedData(string id, CancellationToken token)
        {
            var uri = $"{_dogsConfiguration.Uri}/breeds/{id}";
            var result = await _requestSender.SendRequest(() => GetRequest(uri), token);
            return HandleBreedData(result);
        }

        private UnityWebRequest GetRequest(string uri)
        {
            var request = UnityWebRequest.Get(uri);
            request.timeout = _dogsConfiguration.Timeout;
            return request;
        }

        private void HandleBreedsCollection(UnityWebRequestResult result)
        {
            try
            {
                var response = JsonConvert.DeserializeObject<DogsResponse<List<Breed>>>(result.Text);
                var dtos = response.Data.Select(x => new BreedDto(x.Id, x.Attributes.Name)).ToList();
                _dogsModel.AddBreeds(dtos);
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
            }
        }

        private string HandleBreedData(UnityWebRequestResult result)
        {
            var response = JsonConvert.DeserializeObject<DogsResponse<Breed>>(result.Text);
            return response.Data.Attributes.Description;
        }
    }
}