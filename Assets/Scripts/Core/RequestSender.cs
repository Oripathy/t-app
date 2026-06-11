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
        private readonly Queue<Func<UniTask>> _requestsQueue = new Queue<Func<UniTask>>();
        private CancellationTokenSource _globalTokenSource;

        private bool _isProcessing;

        public void Initialize()
        {
            _globalTokenSource = new CancellationTokenSource();
        }

        public void Dispose()
        {
            _globalTokenSource?.Cancel();
            _globalTokenSource?.Dispose();
            _requestsQueue.Clear();
        }

        public UniTask<UnityWebRequestResult> SendRequest(Func<UnityWebRequest> requestFactory, CancellationToken token)
        {
            using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(_globalTokenSource.Token, token);
            var completionSource = new UniTaskCompletionSource<UnityWebRequestResult>();
            var linkedToken = linkedTokenSource.Token;
            _requestsQueue.Enqueue(TaskRoutine);
            ProcessQueue().Forget();
            return completionSource.Task;

            async UniTask TaskRoutine()
            {
                try
                {
                    linkedToken.ThrowIfCancellationRequested();
                    var request = requestFactory();
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
                while (_requestsQueue.TryDequeue(out var taskFactory))
                {
                    await taskFactory();
                    await UniTask.Yield();
                }
            }
            finally
            {
                _isProcessing = false;
            }
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