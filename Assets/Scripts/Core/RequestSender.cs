using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Zenject;

namespace Core
{
    public class RequestSender : IInitializable, IDisposable
    {
        private readonly LinkedList<RequestEntry> _requestsQueue = new LinkedList<RequestEntry>();
        private CancellationTokenSource _globalTokenSource;

        private bool _isProcessing;

        public event Action<string> RequestSent;
        public event Action<string> RequestProcessed;

        public void Initialize()
        {
            _globalTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _globalTokenSource?.Cancel();
            _globalTokenSource?.Dispose();
            foreach (var entry in _requestsQueue)
            {
                entry.Registration.Dispose();
                entry.CompletionSource.TrySetCanceled(entry.Token);
            }
            
            _requestsQueue.Clear();
        }

        public UniTask<UnityWebRequestResult> SendRequest(Func<UnityWebRequest> requestFactory, string name,
            CancellationToken token)
        {
            var completionSource = new UniTaskCompletionSource<UnityWebRequestResult>();
            if (token.IsCancellationRequested)
            {
                completionSource.TrySetCanceled(token);
                return completionSource.Task;
            }
            
            var entry = new RequestEntry()
            {
                CompletionSource = completionSource,
                Token = token,
                TaskFactory = TaskFactory
            };
            
            entry.Node = _requestsQueue.AddLast(entry);
            entry.Registration = token.Register(() =>
            {
                _requestsQueue.Remove(entry.Node);
                entry.Registration.Dispose();
                entry.CompletionSource.TrySetCanceled(token);
            });
            
            ProcessQueue().Forget();
            return completionSource.Task;

            async UniTask TaskFactory()
            {
                try
                {
                    using var linkedTokenSource =
                        CancellationTokenSource.CreateLinkedTokenSource(_globalTokenSource.Token, token);
                    var linkedToken = linkedTokenSource.Token;
                    linkedToken.ThrowIfCancellationRequested();
                    using var request = requestFactory();
                    RequestSent?.Invoke(name);
                    await request.SendWebRequest().ToUniTask(cancellationToken: linkedToken);
                    var result = new UnityWebRequestResult(
                        request.result == UnityWebRequest.Result.Success,
                        request.responseCode,
                        request.downloadHandler?.text,
                        request.downloadHandler?.data,
                        request.error);

                    completionSource.TrySetResult(result);
                }
                catch (OperationCanceledException e)
                {
                    completionSource.TrySetCanceled(e.CancellationToken);
                }
                catch (Exception e)
                {
                    completionSource.TrySetException(e);
                }
                finally
                {
                    RequestProcessed?.Invoke(name);
                }
            }
        }

        private async UniTaskVoid ProcessQueue()
        {
            if (_isProcessing)
            {
                return;
            }

            _isProcessing = true;
            try
            {
                while (_requestsQueue.Count > 0)
                {
                    var entry = _requestsQueue.First.Value;
                    _requestsQueue.RemoveFirst();
                    await entry.Registration.DisposeAsync();
                    await entry.TaskFactory();
                    await UniTask.Yield();
                }
            }
            finally
            {
                _isProcessing = false;
            }
        }

        private class RequestEntry
        {
            public Func<UniTask> TaskFactory { get; set; }
            public CancellationToken Token { get; set; }
            public UniTaskCompletionSource<UnityWebRequestResult> CompletionSource { get; set; }
            public LinkedListNode<RequestEntry> Node { get; set; }
            public CancellationTokenRegistration Registration { get; set; }
        }
    }

    public readonly struct UnityWebRequestResult
    {
        public bool IsSuccess { get; }
        public long ResponseCode { get; }
        public string Text { get; }
        public byte[] Data { get; }
        public string Error { get; }

        public UnityWebRequestResult(bool isSuccess, long responseCode, string text, byte[] data, string error)
        {
            IsSuccess = isSuccess;
            ResponseCode = responseCode;
            Text = text;
            Data = data;
            Error = error;
        }
    }
}