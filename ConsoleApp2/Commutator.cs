using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    internal class Commutator
    {
        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            Task listCreator = new Task(() => CreateDictionariesOfDublicateIntegers(), _cancellationToken);
            Task verifier = new Task(() => FoundDublicateDictionary(), _cancellationToken);

            listCreator.Start();
            verifier.Start();

            Task.WaitAll(listCreator, verifier);
        }

        #region Private Methods

        private void CreateDictionariesOfDublicateIntegers()
        {
            while(!_cancellationToken.IsCancellationRequested)
            {
                List<int> newList = ListCreator.Instance.GetListOfIntegers(10);
                Dictionary<int, int> newListDublicates = newList.GetDublicatedIntegers();

                if (newListDublicates.Keys.Count == 0)
                    continue;

                lock (_locker)
                {
                    _nonVerifiedDictionaries.Add(newListDublicates);
                    Console.WriteLine($"Added new not verified dictionary: {string.Join("\n", newListDublicates.Select(item => $"Integer: {item.Key}, Dublicates count: {item.Value}."))}");
                }
            }
        }

        private void FoundDublicateDictionary()
        {
            while(!_cancellationToken.IsCancellationRequested)
            {
                lock (_locker)
                {
                    Dictionary<int, int> nonVerifiedDublicate = _nonVerifiedDictionaries.FirstOrDefault();
                    if (nonVerifiedDublicate == null)
                    {
                        //Console.WriteLine("System does not have non-verified dublicates.");
                        continue;
                    }

                    if (_verifiedDictionaries.Any(dublicate => dublicate.SequenceEqual(nonVerifiedDublicate)))
                    {
                        Console.WriteLine($"Found dublicate: {string.Join("\n", nonVerifiedDublicate.Select(item => $"Integer: {item.Key}, Dublicates count: {item.Value}."))}");
                        _cancellationTokenSource.Cancel();
                    }
                    else
                    {
                        _verifiedDictionaries.Add(nonVerifiedDublicate);
                        _nonVerifiedDictionaries.Remove(nonVerifiedDublicate);
                    }
                }
            }
        }

        #endregion

        #region Private fields

        List<Dictionary<int, int>> _verifiedDictionaries = new List<Dictionary<int, int>>();
        List<Dictionary<int, int>> _nonVerifiedDictionaries = new List<Dictionary<int, int>>();
        CancellationTokenSource _cancellationTokenSource;
        CancellationToken _cancellationToken;

        object _locker = new object();

        #endregion
    }
}
