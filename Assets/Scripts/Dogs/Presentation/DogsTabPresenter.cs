using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Dogs.Infrastructure;
using Dogs.Model;
using MainUI;
using MainUI.Presentation;
using UnityEngine;

namespace Dogs.Presentation
{
    public class DogsTabPresenter : Presenter<DogsModel, DogsTabView>, ITab
    {
        private readonly BreedButton.Pool _breedButtonPool;
        private readonly BreedsRequester _breedsRequester;

        private readonly List<BreedButton> _buttons = new List<BreedButton>();
        private CancellationTokenSource _tokenSource;
        private string _processingId = string.Empty;

        public DogsTabPresenter(DogsModel model, BreedButton.Pool breedButtonPool, BreedsRequester breedsRequester) :
            base(model)
        {
            _breedButtonPool = breedButtonPool;
            _breedsRequester = breedsRequester;
        }

        public TabType TabType => TabType.Dogs;

        public void Activate()
        {
            View.gameObject.SetActive(true);
            _tokenSource =  new CancellationTokenSource();
            RequestBreeds().Forget();
        }

        public void Deactivate()
        {
            View.gameObject.SetActive(false);
            View.BreedPopup.Hide();
            _processingId = string.Empty;
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _tokenSource = null;
            foreach (var button in _buttons)
            {
                button.Button.onClick.RemoveAllListeners();
                _breedButtonPool.Despawn(button);
            }
            
            _buttons.Clear();
        }

        private async UniTaskVoid RequestBreeds()
        {
            if (Model.Breeds.Any())
            {
                CreateButtons();
                return;
            }
            
            try
            {
                var token = _tokenSource.Token;
                await _breedsRequester.RequestBreeds(1, token);
                token.ThrowIfCancellationRequested();
                CreateButtons();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void CreateButtons()
        {
            foreach (var breed in Model.Breeds)
            {
                var button = _breedButtonPool.Spawn();
                button.transform.SetParent(View.ButtonsContainer);
                button.transform.localScale = Vector3.one;
                button.transform.position = Vector3.zero;
                button.Initialize(breed.Name, breed.Id);
                button.Button.onClick.AddListener(() => ShowPopup(breed.Id).Forget());
                _buttons.Add(button);
            }
            
            View.SetScrollEnabled(_buttons.Count > 0);
        }

        private async UniTask ShowPopup(string id)
        {
            if (!Model.TryGetBreed(id, out var breed) || _processingId == id)
            {
                return;
            }

            _processingId = id;
            _tokenSource?.Cancel();
            _tokenSource?.Dispose();
            _tokenSource = new CancellationTokenSource();
            if (breed.Info != null)
            {
                _processingId = string.Empty;
                View.BreedPopup.Show(breed.Name, breed.Info.Description);
                return;
            }
            
            try
            {
                var description = await _breedsRequester.RequestBreedDescription(id, _tokenSource.Token);
                breed.AddInfo(description);
                View.BreedPopup.Show(breed.Name, description);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            finally
            {
                _processingId = string.Empty;
            }
        }
    }
}